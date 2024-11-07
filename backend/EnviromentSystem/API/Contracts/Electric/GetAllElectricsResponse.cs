namespace API.Contracts.Electric
{
    public class GetAllElectricsResponse
    {
        public List<ElectricDto> Electrics { get; set; } = new();
    }
}
