namespace API.Contracts.Paper
{
    public class GetAllPapersResponse
    {
        public List<PaperDto> Papers { get; set; } = new();
    }
}
