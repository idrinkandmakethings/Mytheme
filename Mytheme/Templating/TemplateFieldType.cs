namespace Mytheme.Templating
{
    public enum TemplateFieldType
    {
        RandomNumber,
        RandomTable,
        DieRoll,
        Template,
        Error
    }

    public enum ValidationError
    {
        InvalidTag,
        TableDoesNotExist,
        TemplateDoesNotExist
    }
}
