using System.Collections.Generic;

namespace Mytheme.Templating.TemplateTypes
{
    public class TemplateTmp
    {
        public string TemplateName { get; set; }

        public List<string> Variables { get; set; }

        public TemplateTmp()
        {
            Variables = new List<string>();
        }
    }
}
