using System.Collections.Generic;
using Mytheme.Dal.Dto;
using Mytheme.Templating.TemplateTypes;

namespace Mytheme.Templating
{
    public class TemplateRenderer
    {
        

        public string RenderTemplateToMarkDown(Template template)
        {
            // generate var fields first
            var generatedVars = new Dictionary<string, string>();

            foreach (var templateVariable in template.TemplateVariables)
            {
                
            }
        }

        private string RenderTemplateField(TemplateField field)
        {
            switch (field.FieldType)
            {
                case TemplateFieldType.DieRoll:
                    break;
                case TemplateFieldType.List:
                    break;
                case TemplateFieldType.RandomNumber:
                    break;
                case TemplateFieldType.RandomTable:
                    break;
                case TemplateFieldType.Template:
                    break;
                case TemplateFieldType.DieRoll:
                    break;
                case TemplateFieldType.DieRoll:
                    break;
                case TemplateFieldType.DieRoll:
                    break;

            }
        }
    }
}
