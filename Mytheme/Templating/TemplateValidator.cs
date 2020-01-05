using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
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
        readonly Regex variableMatch = new Regex(@"\{([\w\s\d]*?)\}", RegexOptions.Compiled);

        internal Dictionary<TemplateFieldType, Regex> validationRegexs;


        public TemplateValidator(IRandomTableService randomTableService, ITemplateService templateService)
        {
            this.randomTableService = randomTableService;
            this.templateService = templateService;

            validationRegexs = new Dictionary<TemplateFieldType, Regex>
            {
                {TemplateFieldType.RandomNumber,new Regex(@"^\[rng:(\+?\d+|\-?\d+):(\+?\d+|\-?\d+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.RandomTable, new Regex(@"^\[tbl:([\{\}\w\s]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.DieRoll,new Regex(@"^\[die:((\d*)d(\d+)(\+\d+|\-\d+)?)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.Template, new Regex(@"^\[tmp:([\{\}\w\s]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.List, new Regex(@"^\[lst:([\w\s\d,]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)},
                {TemplateFieldType.Variable, new Regex(@"^\[var:([\{\}\w\s\:\[\]\"",]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled)}
            };
        }

        public async Task<ValidationResult> ValidateTemplate(Template template)
        {
            var matches = fieldMatch.Matches(template.TemplateBody);

            var matchList = new List<string>();

            foreach (Match match in matches)
            {
                matchList.Add(match.Groups[0].Value);
            }

            var fields = GenerateFields(matchList);

            var (errors, list) = await SetFields(fields);

            var (templateFields, variables) = VerifyVariables(list);

            template.Fields = templateFields;
            template.TemplateVariables = variables;

            return new ValidationResult
            {
                Template = template,
                ValidationErrors = errors
            };
        }

        internal List<TemplateField> GenerateFields(List<string> fieldMatches)
        {
            var result = new List<TemplateField>();

            for (int i = 0; i < fieldMatches.Count; i++)
            {
                var field = AssignFieldType(fieldMatches[i], i);

                result.Add(field);
            }

            return result;
        }

        internal async Task<(Dictionary<string, ValidationError> errors, List<TemplateField> fields)> SetFields(List<TemplateField> fields)
        {
            var errors = new Dictionary<string, ValidationError>();

            for (var i = 0; i < fields.Count; i++)
            {
                var (err, field) = await SetFieldByType(fields[i]);

                if (err != ValidationError.None)
                {
                    errors[field.Value] = err;
                }

                fields[i] = field;
            }

            return (errors, fields);
        }

        private (List<TemplateField> fields, Dictionary<string, TemplateField> variables) VerifyVariables(List<TemplateField> fields)
        {
            var variables = new Dictionary<string, TemplateField>();
            var outFields = new List<TemplateField>();
            foreach (var field in fields)
            {
                if (field.FieldType == TemplateFieldType.Variable && !string.IsNullOrEmpty(field.VariableName))
                {
                    variables[field.VariableName] = field;
                }
                else
                {
                    outFields.Add(field);
                }
            }

            outFields.Sort();

            // re-number fields
            for (int i = 0; i < outFields.Count; i++)
            {
                outFields[i].Sort = i;
            }

            return (outFields, variables);
        }


        internal TemplateField AssignFieldType(string val, int order)
        {
            var field = new TemplateField { Sort = order, Value = val };

            var sub = val.Substring(1, 3).ToLower();

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

            return field;
        }


        internal async Task<(ValidationError err, TemplateField field)> SetFieldByType(TemplateField field)
        {
            var err = ValidationError.None;

            switch (field.FieldType)
            {
                case TemplateFieldType.RandomNumber:
                    var rng = SetRandomNumberField(field);
                    field = rng.field;
                    err = rng.error;
                    break;
                case TemplateFieldType.RandomTable:
                    var tbl = await SetRandomTableField(field);
                    field = tbl.field;
                    err = tbl.error;
                    break;
                case TemplateFieldType.Template:
                    var tmp = await SetTemplateField(field);
                    field = tmp.field;
                    err = tmp.error;
                    break;
                case TemplateFieldType.DieRoll:
                    var die = SetDieRollField(field);
                    field = die.field;
                    err = die.error;
                    break;
                case TemplateFieldType.List:
                    var lst = SetListField(field);
                    field = lst.field;
                    err = lst.error;
                    break;
                case TemplateFieldType.Variable:
                    var var = await SetVariableField(field);
                    field = var.field;
                    err = var.error;
                    break;
            }

            return (err, field);
        }

        internal (ValidationError error, TemplateField field) SetRandomNumberField(TemplateField field)
        {
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

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

        internal async Task<(ValidationError error, TemplateField field)> SetRandomTableField(TemplateField field)
        {
            ValidationError err = ValidationError.None;
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

                var table = match.Groups[1].Value.Trim();

                var json = new TemplateTbl
                {
                    TableName = table
                };

                // if table uses variables don't check the db
                if (table.Contains('{') || table.Contains('}'))
                {
                    var (valid, vars) = ParseVariableNames(table);
                    
                    err = valid ? ValidationError.None : ValidationError.InvalidTag;
                    field.Valid = valid;
                    json.Variables = vars;
                }
                else
                {
                    var result = await randomTableService.TableExists(table);

                    if (!result.Result)
                    {
                        field.Valid = false;
                        err = ValidationError.TableDoesNotExist;
                    }
                }

                field.TemplateJson = JsonConvert.SerializeObject(json);

                field.Valid = true;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                err = ValidationError.InvalidTag;
            }

            return (err, field);
        }

        internal async Task<(ValidationError error, TemplateField field)> SetTemplateField(TemplateField field)
        {
            ValidationError err = ValidationError.None;
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

                var template = match.Groups[1].Value.Trim();

                var json = new TemplateTmp{TemplateName = template};

                if (template.Contains('{') || template.Contains('}'))
                {
                    var (valid, vars) = ParseVariableNames(template);

                    err = valid ? ValidationError.None : ValidationError.InvalidTag;
                    field.Valid = valid;
                    json.Variables = vars;
                }
                else
                {
                    var result = await templateService.TemplateExists(template);
                    if (!result.Result)
                    {
                        field.Valid = false;
                        err = ValidationError.TemplateDoesNotExist;
                    }
                }

                field.TemplateJson = JsonConvert.SerializeObject(json);
                field.Valid = true;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                err = ValidationError.InvalidTag;
            }

            return (err, field);
        }

        internal (ValidationError error, TemplateField field) SetDieRollField(TemplateField field)
        {
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

                var dieCount = 1;
                var modifier = 0;
                if (!string.IsNullOrEmpty(match.Groups[2].Value))
                {
                    dieCount = int.Parse(match.Groups[2].Value);
                }
                
                var dieSize = int.Parse(match.Groups[3].Value);
                
                if (!string.IsNullOrEmpty(match.Groups[4].Value))
                {
                    modifier = int.Parse(match.Groups[4].Value);
                }

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

        internal (ValidationError error, TemplateField field) SetListField(TemplateField field)
        {
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

                var lst = new TemplateLst
                {
                    Values = match.Groups[1].Value.Split(',').Select(x => x.TrimStart(' ').TrimEnd(' ')).ToList()
                };
            
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

        internal async Task<(ValidationError error, TemplateField field)> SetVariableField(TemplateField field)
        {
            var err = ValidationError.None;
            try
            {
                var match = validationRegexs[field.FieldType].Match(field.Value);

                if (!match.Success)
                {
                    field.Valid = false;
                    return (ValidationError.InvalidTag, field);
                }

                var variable = JsonConvert.DeserializeObject<TemplateVar>(match.Groups[1].Value);

                field.VariableName = variable.Name;
                var childField = AssignFieldType($"[{variable.Value}]", 0);
                var setResult = await SetFieldByType(childField);

                variable.TemplateObjectJson = JsonConvert.SerializeObject(setResult.field);
                variable.TemplateObjectType = setResult.field.FieldType;

                if (setResult.err != ValidationError.None)
                {
                    err = setResult.err;
                    field.Valid = false;
                }
                else
                {
                    field.Valid = true;
                }
                
                field.TemplateJson = JsonConvert.SerializeObject(variable);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                err = ValidationError.InvalidTag;
            }

            return (err, field);
        }

        private (bool valid, List<string> vars) ParseVariableNames(string table)
        {
            var vars = new List<string>();

            var right = table.Count(x => x == '}');
            var left = table.Count(x => x == '{');

            if (right != left)
            {
                return (false, vars);
            }

            var matches = variableMatch.Matches(table);

            if (matches.Count == right)
            {
                foreach (Match var in matches)
                {
                    vars.Add(var.Groups[1].ToString());
                }

                return (true, vars);
            }

            return (false, vars);
        }
    }

    public class ValidationResult
    {
        public Template Template { get; set; }

        public Dictionary<string, ValidationError> ValidationErrors { get; set; }
    }
}
