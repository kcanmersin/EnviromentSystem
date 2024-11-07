using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entity
{
    public class Building : EntityBase
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string E_MeterCode { get; set; }

        public ICollection<Electric> Electrics { get; set; } = new List<Electric>();
    }
}
