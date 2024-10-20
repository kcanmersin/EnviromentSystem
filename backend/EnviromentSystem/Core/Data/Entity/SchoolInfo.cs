using Core.Data.Entity.EntityBases;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entity
{
    public class SchoolInfo : EntityBase
    {
        [Required]
        public int NumberOfPeople { get; set; } 

        [Required]
        public int Year { get; set; } 

        [Required]
        [MaxLength(15)]
        public string Month { get; set; } 

        public ICollection<Electric> Electrics { get; set; } = new List<Electric>();
        public ICollection<Water> Waters { get; set; } = new List<Water>();
        public ICollection<Paper> Papers { get; set; } = new List<Paper>();
    }
}
