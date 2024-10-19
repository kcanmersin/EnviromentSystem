namespace API.Contracts.Electric
{
    public class CreateElectricRequest
    {
        public Guid SchoolInfoId { get; set; }
        public decimal Consumption { get; set; }
        public decimal Cost { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
    }
}
