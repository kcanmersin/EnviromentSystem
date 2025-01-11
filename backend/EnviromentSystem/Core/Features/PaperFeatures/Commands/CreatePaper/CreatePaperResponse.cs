namespace Core.Features.PaperFeatures.Commands.CreatePaper
{
    public class CreatePaperResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
    }
}
