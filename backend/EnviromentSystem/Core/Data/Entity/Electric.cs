using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Electric : EntityBase
    {
        [Required]
        public decimal Consumption { get; set; } 

        [Required]
        public decimal Cost { get; set; }  

        [Required]
        public Guid SchoolInfoId { get; set; }  

        [ForeignKey(nameof(SchoolInfoId))]
        public SchoolInfo SchoolInfo { get; set; }
    }
}
