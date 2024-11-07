using Core.Data.Entity.EntityBases;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Electric : EntityBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal InitialMeterValue { get; set; }

        [Required]
        public decimal FinalMeterValue { get; set; }

        [Required]
        public decimal Usage { get; set; }

        [Required]
        public decimal KWHValue { get; set; }

        [Required]
        public Guid BuildingId { get; set; } 

        [ForeignKey(nameof(BuildingId))]
        public Building Building { get; set; }
    }
}
