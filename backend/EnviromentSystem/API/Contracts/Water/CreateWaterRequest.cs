using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Water
{
    public class CreateWaterRequest
    {
        [Required]
        [DefaultValue("2022-01-31")]
        public DateTime Date { get; set; }

        [Required]
        [DefaultValue(100.00)]
        public decimal InitialMeterValue { get; set; }

        [Required]
        [DefaultValue(200.00)]
        public decimal FinalMeterValue { get; set; }

        [Required]
        [DefaultValue(100.00)]
        public decimal Usage { get; set; }
    }
}
