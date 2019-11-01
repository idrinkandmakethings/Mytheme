namespace Mytheme.Templating.TemplateTypes
{
    public class TemplateVar
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public string Value { get; set; }

        public TemplateFieldType TemplateObjectType { get; set; }
        public string TemplateObjectJson { get; set; }
    }
}
