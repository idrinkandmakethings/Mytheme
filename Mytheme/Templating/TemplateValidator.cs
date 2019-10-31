using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;
using Mytheme.Templating.TemplateTypes;
using Newtonsoft.Json;
using Serilog;


[assembly: InternalsVisibleTo("TemplatingTests")]
namespace Mytheme.Templating
{
    /*
     * [lst:{male,female}]
     * [var:{"name":"race","type":"tbl","value":"Races","display":true}]
     * [var:{"name":"gender","type":"lst","value":{male,female},"display":true}]
     * 
     * [tbl:{race} {gender} names]
     * 
     * [tbl:{race} backgrounds]
     */

    public class TemplateValidator
    {
        private readonly IRandomTableService randomTableService;
        private readonly ITemplateService templateService;

        readonly Regex fieldMatch = new Regex(@"\[\w{3}:.*?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        internal Dictionary<TemplateFieldType, Regex> validationRegexs;


        public TemplateValidator(IRandomTableService randomTableService, ITemplateService templateService)
        {
            this.randomTableService = randomTableService;
            this.templateService = templateService;

            validationRegexs = new Dictionary<TemplateFieldType, Regex>
            {
                {TemplateFieldType.RandomNumber,new Regex(@"^\[rng:(\+?\d+|\-?\d+):(\+?\d+|\-?\d+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.RandomTable, new Regex(@"^\[tbl:([\w\s]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.DieRoll,new Regex(@"^\[die:((\d*)d(\d+)(\+\d+|\-\d+)?)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.Template, new Regex(@"^\[tmp:([\w\s]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.List, new Regex(@"^\[lst:\{([\w\s\d,]+)\}\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.Variable, new Regex(@"^\[var:([\{\}\w\s]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)}
            };
            
        }

        public async Task<ValidationResult> ValidateTemplate(Template template)
        {
            var fieldText = fieldMatch.Match(template.TemplateBody).Groups.Values.Select(x => x.Value).ToList();

            var fields = GenerateFields(fieldText);

            var result = await ValidateFields(fields);

            template.Fields = result.fields;

            return new ValidationResult
            {
                Template = template,
                ValidationErrors = result.errors
            };
        }

        internal List<TemplateField> GenerateFields(List<string> fieldMatches)
        {
            var result = new List<TemplateField>();

            for (int i = 0; i < fieldMatches.Count; i++)
            {
                var field = new TemplateField{Order = i, Value = fieldMatches[i]};

                var sub = fieldMatches[i].Substring(1, 3).ToLower();

                switch (sub)
                {
                    case "rng":
                        field.FieldType = TemplateFieldType.RandomNumber;
                        break;
                    case "tbl":
                        field.FieldType = TemplateFieldType.RandomTable;
                        break;
                    case "die":
                        field.FieldType = TemplateFieldType.DieRoll;
                        break;
                    case "tmp":
                        field.FieldType = TemplateFieldType.Template;
                        break;
                    case "lst":
                        field.FieldType = TemplateFieldType.List;
                        break;
                    case "var":
                        field.FieldType = TemplateFieldType.Variable;
                        break;
                    default:
                        field.FieldType = TemplateFieldType.Error;
                        break;
                }

                result.Add(field);
            }

            return result;
        }

        internal async Task<(Dictionary<string, ValidationError> errors, List<TemplateField> fields)> ValidateFields(List<TemplateField> fields)
        {
            var result = new Dictionary<string, ValidationError>();

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                 
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    result[field.Value] = ValidationError.InvalidTag;
                    field.Valid = false;
                    fields[i] = field;
                    continue;
                }

                var err = ValidationError.None;

                switch (field.FieldType)
                {
                    case TemplateFieldType.RandomNumber:
                        var rng = SetRandomNumberField(field, match);
                        field = rng.field;
                        err = rng.error;
                        break;
                    case TemplateFieldType.RandomTable:
                        var tbl = await SetRandomTableField(field, match);
                        field = tbl.field;
                        err = tbl.error;
                        break;
                    case TemplateFieldType.Template:
                        var tmp = await SetTemplateField(field, match);
                        field = tmp.field;
                        err = tmp.error;
                        break;
                    case TemplateFieldType.DieRoll:
                        var die = SetDieRollField(field, match);
                        field = die.field;
                        err = die.error;
                        break;
                    case TemplateFieldType.List:
                        var lst = SetListField(field, match);
                        field = lst.field;
                        err = lst.error;
                        break;
                    case TemplateFieldType.Variable:
                        var var = SetVariableField(field, match);
                        field = var.field;
                        err = var.error;
                        break;
                }

                if ( err != ValidationError.None)
                {
                    result[field.Value] = err;
                }

                fields[i] = field;
            }

            return (result, fields);
        }

        internal (ValidationError error, TemplateField field) SetRandomNumberField(TemplateField field, Match match)
        {
            try
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);

                var rng = new TemplateRng();

                if (x > y)
                {
                    rng.UpperBound = x;
                    rng.LowerBound = y;
                }
                else
                {
                    rng.UpperBound = y;
                    rng.LowerBound = x;
                }

                field.TemplateJson = JsonConvert.SerializeObject(rng);
                field.Valid = true;
                return (ValidationError.None, field);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                return (ValidationError.InvalidTag, field);
            }
        }

        internal async Task<(ValidationError error, TemplateField field)> SetRandomTableField(TemplateField field, Match match)
        {
            var table = match.Groups[1].Value;

            var result = await randomTableService.TableExists(table);

            if (!result.Result)
            {
                field.Valid = false;
                field.TemplateJson = table;
                return (ValidationError.TableDoesNotExist, field);
            }

            field.Valid = true;
            field.TemplateJson = table;

            return (ValidationError.None, field);
        }

        internal async Task<(ValidationError error, TemplateField field)> SetTemplateField(TemplateField field, Match match)
        {
            var template = match.Groups[1].Value;

            var result = await templateService.TemplateExists(template);

            if (!result.Result)
            {
                field.Valid = false;
                field.TemplateJson = template;
                return (ValidationError.TemplateDoesNotExist, field);
            }

            field.Valid = true;
            field.TemplateJson = template;

            return (ValidationError.None, field);
        }

        internal (ValidationError error, TemplateField field) SetDieRollField(TemplateField field, Match match)
        {
            try
            {
                var dieCount = 1;
                if (!string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    dieCount = int.Parse(match.Groups[1].Value);
                }
                
                var dieSize = int.Parse(match.Groups[2].Value);
                var modifier = int.Parse(match.Groups[2].Value);

                var die = new TemplateDie
                {
                    DieCount = dieCount,
                    DieSize = dieSize,
                    Modifier = modifier
                };
                
                field.TemplateJson = JsonConvert.SerializeObject(die);
                field.Valid = true;
                return (ValidationError.None, field);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                return (ValidationError.InvalidTag, field);
            }
        }

        internal (ValidationError error, TemplateField field) SetListField(TemplateField field, Match match)
        {
            try
            {
                var lst = match.Groups[1].Value.Split(',').ToList();

                field.TemplateJson = JsonConvert.SerializeObject(lst);
                field.Valid = true;
                return (ValidationError.None, field);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                return (ValidationError.InvalidTag, field);
            }
        }

        internal (ValidationError error, TemplateField field) SetVariableField(TemplateField field, Match match)
        {
            try
            {
                var var = JsonConvert.DeserializeObject<TemplateVar>(match.Groups[1].Value);

                field.TemplateJson = match.Groups[1].Value;
                field.Valid = true;
                return (ValidationError.None, field);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                return (ValidationError.InvalidTag, field);
            }
        }
    }

    public class ValidationResult
    {
        public Template Template { get; set; }

        public Dictionary<string, ValidationError> ValidationErrors { get; set; }
    }
}
