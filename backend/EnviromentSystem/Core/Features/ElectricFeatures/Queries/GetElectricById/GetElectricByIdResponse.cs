namespace Core.Features.ElectricFeatures.Queries.GetElectricById
{
    public class GetElectricByIdResponse
    {
        public Guid Id { get; set; }
        public Guid SchoolInfoId { get; set; }
        public decimal Consumption { get; set; }
        public decimal Cost { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
