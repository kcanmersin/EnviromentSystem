namespace API.Contracts.Electric
{
    public class GetAllElectricGroupByResponse
    {
        public List<ElectricGroupByDto> GroupedElectrics { get; set; } = new();
    }

}
