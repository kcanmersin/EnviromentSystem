namespace API.Contracts.Paper
{
    public class UpdatePaperResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
