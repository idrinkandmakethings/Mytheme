using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytheme.Templating.TemplateTypes;

namespace Mytheme.Dal.Dto
{
    public class Template : DtoObject
    {
        public Template()
        {
            Fields = new List<TemplateField>();
            TemplateVariables = new Dictionary<string, TemplateField>();
        }

        [Key]
        public string Id { get; set; }

        [Required] 
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string TemplateBody { get; set; }

        public List<TemplateField> Fields { get; set; }

        [NotMapped]
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
        public string Id { get; set; }

        [Required] public string Name { get; set; }
    }

    public class TemplateField : IComparable<TemplateField>
    {
        [Key]
        public int Id { get; set; }
        public string FK_Template { get; set; }
        [Required] public int Order { get; set; }
        [Required] public TemplateFieldType FieldType { get; set; }
        
        [Required] public bool Valid { get; set; }
        public string VariableName { get; set; }
        [Required] public string Value { get; set; }
        [Required] public string TemplateJson { get; set; }

        [ForeignKey(nameof(FK_Template))]
        public Template Template { get; set; }

        public int CompareTo(TemplateField other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Order.CompareTo(other.Order);
        }
    }
}