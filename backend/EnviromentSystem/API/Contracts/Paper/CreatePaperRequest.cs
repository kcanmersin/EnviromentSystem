using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Paper
{
    public class CreatePaperRequest
    {
        [Required]
        [DefaultValue("2022-01-31")]
        public DateTime Date { get; set; }

        [Required]
        [DefaultValue(100.00)]
        public decimal Usage { get; set; }
    }
}
