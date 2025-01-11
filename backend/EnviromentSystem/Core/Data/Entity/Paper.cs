using Core.Data.Entity.EntityBases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entity
{
    public class Paper : EntityBase
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Usage { get; set; } 

    }
}
