namespace API.Contracts.Electric
{
    public class CreateElectricResponse
    {
        public Guid Id { get; set; }
        public Guid SchoolInfoId { get; set; }
        public decimal Consumption { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
