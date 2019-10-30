using System.Collections.Generic;

namespace Mytheme.Templating.TemplateTypes
{
    public class TemplateTmp
    {
        public string TemplateName { get; set; }

        private List<string> Variables { get; set; }
    }
}
