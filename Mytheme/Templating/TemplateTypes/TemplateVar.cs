namespace Mytheme.Templating.TemplateTypes
{
    public class TemplateVar
    {
        public string Name { get; set; }
        public TemplateFieldType Type { get; set; }
        public object Value { get; set; }
        public bool Display { get; set; }
    }
}
