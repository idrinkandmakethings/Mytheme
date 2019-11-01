using System.Collections.Generic;

namespace Mytheme.Templating.TemplateTypes
{
    public class TemplateTbl
    {
        public string TableName { get; set; }
        public List<string> Variables { get; set; }

        public TemplateTbl()
        {
            Variables = new List<string>();
        }
    }
}
