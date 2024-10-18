using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Paper : EntityBase
    {
        [Required]
        public decimal Consumption { get; set; }  // Consumption in tons

        [Required]
        public decimal Cost { get; set; }  // Cost of paper usage

        [Required]
        public Guid SchoolInfoId { get; set; }  // Foreign key to SchoolInfo

        // Navigation property
        [ForeignKey(nameof(SchoolInfoId))]
        public SchoolInfo SchoolInfo { get; set; }
    }
}
