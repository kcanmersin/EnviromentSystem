namespace API.Contracts.Electric
{
    public class UpdateElectricRequest
    {
        public Guid Id { get; set; }
        public decimal Consumption { get; set; }
        public decimal Cost { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
    }
}
