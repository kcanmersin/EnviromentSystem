namespace Core.Features.ElectricFeatures.Commands.CreateElectric
{
    public class CreateElectricResponse
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
        public decimal KWHValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
