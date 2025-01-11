namespace API.Contracts.Paper
{
    public class CreatePaperResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
