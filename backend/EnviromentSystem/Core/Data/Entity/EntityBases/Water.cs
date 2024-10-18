using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Water : EntityBase
    {
        [Required]
        public decimal Consumption { get; set; }  // Consumption in cubic meters

        [Required]
        public decimal Cost { get; set; }  // Cost of water

        [Required]
        public Guid SchoolInfoId { get; set; }  // Foreign key to SchoolInfo

        // Navigation property
        [ForeignKey(nameof(SchoolInfoId))]
        public SchoolInfo SchoolInfo { get; set; }
    }
}
