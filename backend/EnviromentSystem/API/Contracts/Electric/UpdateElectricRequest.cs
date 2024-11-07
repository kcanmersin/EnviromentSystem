using System.ComponentModel;

namespace API.Contracts.Electric
{
    public class UpdateElectricRequest
    {
        [DefaultValue("7A7252C6-4558-4B12-BF21-1D2A26C1E8E9")]
        public Guid Id { get; set; }

        [DefaultValue("7A7252C6-4558-4B12-BF21-1D2A26C1E8E9")]
        public Guid BuildingId { get; set; }

        [DefaultValue("2022-01-31")]
        public DateTime Date { get; set; }


        [DefaultValue(335.395)]
        public decimal InitialMeterValue { get; set; }

        [DefaultValue(337.803)]
        public decimal FinalMeterValue { get; set; }

        [DefaultValue(2408.00)]
        public decimal Usage { get; set; }

        [DefaultValue(2408.0000000)]
        public decimal KWHValue { get; set; }
    }
}
