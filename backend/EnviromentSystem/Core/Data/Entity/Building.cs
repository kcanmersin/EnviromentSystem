using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Core.Data.Entity
{
    public class Building : EntityBase
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string? E_MeterCode { get; set; } 

        [MaxLength(20)]
        public string? G_MeterCode { get; set; }

        public ICollection<Electric> Electrics { get; set; } = new List<Electric>();
        public ICollection<NaturalGas> NaturalGasUsages { get; set; } = new List<NaturalGas>();
    }
}
