namespace API.Contracts.Electric
{
    public class GetElectricByIdResponse
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; }
        public DateTime Date { get; set; }
        public string E_MeterCode { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
        public decimal KWHValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
