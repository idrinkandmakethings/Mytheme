using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class Campaign
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        

        [Required] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public List<Adventure> Adventures { get; set; }

        [NotMapped]
        public Adventure CampaignPages { get; set; }

        public Campaign()
        {
            Adventures = new List<Adventure>();
        }
    }

    public class Adventure
    {
        [Key]
        public string Id { get; set; }
        public string FK_Campaign { get; set; }

        [Required]
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_Campaign))]
        public Campaign Campaign { get; set; }

        public List<Page> Pages { get; set; }
        public List<MapPage> MapPages { get; set; }

        public Adventure()
        {
            Pages = new List<Page>();
            MapPages = new List<MapPage>();
        }
    }

    public class Page
    {
        [Key]
        public string Id { get; set; }
        public string FK_Adventure { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_Adventure))]
        public Adventure Adventure { get; set; }

    }

    public class MapPage
    {
        [Key]
        public string Id { get; set; }
        public string FK_Adventure { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public List<MapMarker> MapMarkers { get; set; }

        [ForeignKey(nameof(FK_Adventure))]
        public Adventure Adventure { get; set; }

        public MapPage()
        {
            MapMarkers = new List<MapMarker>();
        }
    }

    public class MapMarker
    {
        [Key]
        public string Id { get; set; }
        public string FK_MapPage { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_MapPage))]
        public MapPage MapPage { get; set; }
    }
}
