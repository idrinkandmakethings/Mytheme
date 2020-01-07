using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class TemplateCategoryDal : BaseDal<TemplateCategory>
    {
        public TemplateCategoryDal(string connectionString) : base(connectionString)
        {
        }
    }

    public class TemplateDal : BaseDal<Template>
    {
        public TemplateDal(string connectionString) : base(connectionString)
        {
        }
    }

    public class TemplateFieldDal : BaseDal<TemplateField>
    {
        public TemplateFieldDal(string connectionString) : base(connectionString)
        {
        }
    }
}

