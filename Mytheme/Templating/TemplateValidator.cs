using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;

namespace Mytheme.Templating
{
    public class TemplateValidator
    {
        private IRandomTableService randomTableService;
        private ITemplateService templateService;

        Regex fieldMatch = new Regex(@"\[\w{3}:.*?\]", RegexOptions.IgnoreCase & RegexOptions.Compiled);

        private Dictionary<TemplateFieldType, Regex> validationRegexs;


        public TemplateValidator()
        {
            validationRegexs = new Dictionary<TemplateFieldType, Regex>
            {
                {TemplateFieldType.RandomNumber,new Regex(@"^\[rng:\d+-\d+\]$", RegexOptions.IgnoreCase & RegexOptions.Compiled)},
                {TemplateFieldType.RandomTable, new Regex(@"^\[tbl:([\w\s]+)\]$", RegexOptions.IgnoreCase & RegexOptions.Compiled)},
                {TemplateFieldType.DieRoll,new Regex(@"^\[die:([\dd+-]+)\]$", RegexOptions.IgnoreCase & RegexOptions.Compiled)},
                {TemplateFieldType.Template, new Regex(@"^\[tmp:([\w\s]+)\]$", RegexOptions.IgnoreCase & RegexOptions.Compiled)}
            };
            
        }

        public TemplateValidator(IRandomTableService randomTableService, ITemplateService templateService)
        {
            this.randomTableService = randomTableService;
            this.templateService = templateService;
        }

        public ValidationResult ValidateTemplate(Template template)
        {
            var result = new ValidationResult{Template = template};
            
            var fields = fieldMatch.Match(template.TemplateBody).Groups.Values.Select(x => x.Value).ToList();

            result.Template.Fields = GenerateFields(fields);

            result.ValidationErrors = ValidateFields(result.Template.Fields);

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
                    default:
                        field.FieldType = TemplateFieldType.Error;
                        break;
                }

                result.Add(field);
            }

            return result;
        }

        internal Dictionary<string, ValidationError> ValidateFields(List<TemplateField> fields)
        {
            var result = new Dictionary<string, ValidationError>();

            foreach (var VARIABLE in COLLECTION)
            {
                
            }
        }
    }

    public class ValidationResult
    {
        public Template Template { get; set; }

        public Dictionary<string, ValidationError> ValidationErrors { get; set; }
    }
}
