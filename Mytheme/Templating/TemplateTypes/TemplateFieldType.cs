namespace Mytheme.Templating.TemplateTypes
{
    public enum TemplateFieldType
    {
        RandomNumber, //rng
        RandomTable, //tbl
        DieRoll, //die
        Template, //tmp
        List, //lst
        Variable, //var
        Error
    }

    public enum ValidationError
    {
        None,
        InvalidTag,
        TableDoesNotExist,
        TemplateDoesNotExist
    }
}
