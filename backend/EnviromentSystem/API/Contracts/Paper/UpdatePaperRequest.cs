using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Paper
{
    public class UpdatePaperRequest
    {
        [Required]
        [DefaultValue("7A7252C6-4558-4B12-BF21-1D2A26C1E8E9")]
        public Guid Id { get; set; }

        [Required]
        [DefaultValue("2022-01-31")]
        public DateTime Date { get; set; }

        [Required]
        [DefaultValue(100.00)]
        public decimal Usage { get; set; }
    }
}
