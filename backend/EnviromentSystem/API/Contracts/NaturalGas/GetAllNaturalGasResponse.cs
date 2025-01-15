namespace API.Contracts.NaturalGas
{
    public class GetAllNaturalGasResponse
    {
        public List<NaturalGasDto> NaturalGas { get; set; } = new();
    }

}
