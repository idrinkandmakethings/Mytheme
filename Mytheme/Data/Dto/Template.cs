using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DapperExtensions.Mapper;
using Mytheme.Templating.TemplateTypes;

namespace Mytheme.Data.Dto
{

    public sealed class TemplateMapper : ClassMapper<Template>
    {
        public TemplateMapper()
        {
            Table("Template");
            Map(r => r.Id).Column("Id").Key(KeyType.Guid);
            Map(r => r.Name).Column("Name");
            Map(r => r.Category).Column("Category");
            Map(r => r.Description).Column("Description");
            Map(r => r.Enabled).Column("Enabled");
            Map(r => r.TemplateBody).Column("TemplateBody");
            Map(r => r.Fields).Ignore();
            Map(r => r.TemplateVariables).Ignore();
        }
    }

    public class Template : DtoObject
    {
        public Template()
        {
            Fields = new List<TemplateField>();
            TemplateVariables = new Dictionary<string, TemplateField>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string TemplateBody { get; set; }

        public List<TemplateField> Fields { get; set; }

        // not pulled from DB
        public Dictionary<string, TemplateField> TemplateVariables { get; set; }

        public void SetVariables()
        {
            var fields = new List<TemplateField>();

            TemplateVariables.Clear();
            foreach (var field in Fields)
            {
                if (field.FieldType == TemplateFieldType.Variable)
                {
                    TemplateVariables[field.VariableName] = field;
                }
                else
                {
                    fields.Add(field);
                }
            }

            Fields = fields;
        }

        public void SaveVariables()
        {
            foreach (var key in TemplateVariables.Keys)
            {
                 Fields.Add(TemplateVariables[key]);
            }
        }
    }

   
    public class TemplateCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class TemplateField : IComparable<TemplateField>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FK_Template { get; set; }
        public int Sort { get; set; }
        public TemplateFieldType FieldType { get; set; }
        public bool Valid { get; set; }
        public string VariableName { get; set; }
        public string Value { get; set; }
        public string TemplateJson { get; set; }
        
        public int CompareTo(TemplateField other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Sort.CompareTo(other.Sort);
        }
    }
}