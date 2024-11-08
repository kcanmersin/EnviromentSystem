using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace API.Contracts.NaturalGas
{
    public class CreateNaturalGasRequest
    {
        [Required, DefaultValue("7A7252C6-4558-4B12-BF21-1D2A26C1E8E9")]
        public Guid BuildingId { get; set; }

        [Required, DefaultValue("2023-01-31")]
        public DateTime Date { get; set; }

        [Required, DefaultValue(315.903)]
        public decimal InitialMeterValue { get; set; }

        [Required, DefaultValue(327.915)]
        public decimal FinalMeterValue { get; set; }

        [Required, DefaultValue(12012.00)]
        public decimal Usage { get; set; }

        [Required, DefaultValue(16098.7414651)]
        public decimal SM3Value { get; set; }
    }
}
