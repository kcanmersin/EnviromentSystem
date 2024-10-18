using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Electric : EntityBase
    {
        [Required]
        public decimal Consumption { get; set; }  // Consumption in KW

        [Required]
        public decimal Cost { get; set; }  // Cost of electricity

        [Required]
        public Guid SchoolInfoId { get; set; }  // Foreign key to SchoolInfo

        // Navigation property
        [ForeignKey(nameof(SchoolInfoId))]
        public SchoolInfo SchoolInfo { get; set; }
    }
}
