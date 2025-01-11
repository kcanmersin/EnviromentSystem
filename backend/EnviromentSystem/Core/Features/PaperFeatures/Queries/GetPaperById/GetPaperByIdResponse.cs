namespace Core.Features.PaperFeatures.Queries.GetPaperById
{
    public class GetPaperByIdResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
    }
}
