using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytheme.Templating.TemplateTypes;

namespace Mytheme.Dal.Dto
{
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string TemplateBody { get; set; }

        public List<TemplateField> Fields { get; set; }
    }

   
    public class TemplateCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Name { get; set; }
    }

    public class TemplateField : IComparable<TemplateField>
    {
        public int Id { get; set; }
        public int TemplateForeignKey { get; set; }
        [Required] public int Order { get; set; }
        [Required] public TemplateFieldType FieldType { get; set; }
        
        [Required] public bool Valid { get; set; }
        [Required] public string Value { get; set; }
        [Required] public string TemplateJson { get; set; }

        [ForeignKey("TemplateForeignKey")]
        public Template Template { get; set; }

        public int CompareTo(TemplateField other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Order.CompareTo(other.Order);
        }
    }
}