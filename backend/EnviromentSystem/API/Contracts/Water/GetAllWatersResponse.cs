namespace API.Contracts.Water
{
    public class GetAllWatersResponse
    {
        public List<WaterDto> Waters { get; set; } = new();
    }
}
