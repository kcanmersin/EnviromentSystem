namespace Core.Features.WaterFeatures.Queries.GetWaterById
{
    public class GetWaterByIdResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
    }
}
