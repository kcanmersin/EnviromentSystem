namespace API.Contracts.NaturalGas
{
    public class GetAllNaturalGasGroupByResponse
    {
        public List<NaturalGasGroupByDto> GroupedNaturalGas { get; set; } = new();
    }

}
