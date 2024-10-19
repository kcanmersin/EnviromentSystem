namespace Core.Features.ElectricFeatures.Commands.CreateElectric
{
    public class CreateElectricResponse
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
