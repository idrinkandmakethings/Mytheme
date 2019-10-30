using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;
using Mytheme.Templating.TemplateTypes;
using Newtonsoft.Json;
using Serilog;

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
        private IRandomTableService randomTableService;
        private ITemplateService templateService;

        Regex fieldMatch = new Regex(@"\[\w{3}:.*?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private Dictionary<TemplateFieldType, Regex> validationRegexs;


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

        public ValidationResult ValidateTemplate(Template template)
        {
            var result = new ValidationResult{Template = template};
            
            var fieldText = fieldMatch.Match(template.TemplateBody).Groups.Values.Select(x => x.Value).ToList();

            var fields = GenerateFields(fieldText);

            result.ValidationErrors = ValidateFields(ref fields);

            result.Template.Fields = fields;
            return result;
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

        internal Dictionary<string, ValidationError> ValidateFields(ref List<TemplateField> fields)
        {
            var result = new Dictionary<string, ValidationError>();

            for (int i = 0; i < fields.Count; i++)
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
                        err = SetRandomNumberField(ref field, match);
                        break;
                    case TemplateFieldType.RandomTable:
                        err = SetRandomTableField(ref field, match);
                        break;
                    case TemplateFieldType.Template:
                        err = SetTemplateField(ref field, match);
                        break;
                    case TemplateFieldType.DieRoll:
                        err = SetDieRollField(ref field, match);
                        break;
                    case TemplateFieldType.List:
                        err = SetListField(ref field, match);
                        break;
                    case TemplateFieldType.Variable:
                        err = SetVariableField(ref field, match);
                        break;

                }

                if (err != ValidationError.None)
                {
                    result[field.Value] = err;
                }

                fields[i] = field;
            }

            return result;
        }

        private ValidationError SetRandomNumberField(ref TemplateField field, Match match)
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
                    rng.UpperBound = x;
                    rng.LowerBound = y;
                }

                field.TemplateJson = JsonConvert.SerializeObject(rng);
                field.Valid = true;
                return ValidationError.None;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception parsing {field.Value}. ex:{ex.Message}");
                Log.Debug(ex.StackTrace);
                field.Valid = false;
                return ValidationError.InvalidTag;
            }
        }

        private ValidationError SetRandomTableField(ref TemplateField field, Match match)
        {
            throw new NotImplementedException();
        }

        private ValidationError SetTemplateField(ref TemplateField field, Match match)
        {
            throw new NotImplementedException();
        }

        private ValidationError SetDieRollField(ref TemplateField field, Match match)
        {
            throw new NotImplementedException();
        }

        private ValidationError SetListField(ref TemplateField field, Match match)
        {
            throw new NotImplementedException();
        }

        private ValidationError SetVariableField(ref TemplateField field, Match match)
        {
            throw new NotImplementedException();
        }
    }

    public class ValidationResult
    {
        public Template Template { get; set; }

        public Dictionary<string, ValidationError> ValidationErrors { get; set; }
    }
}
