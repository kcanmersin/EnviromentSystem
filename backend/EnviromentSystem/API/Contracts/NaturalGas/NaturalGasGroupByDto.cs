namespace API.Contracts.NaturalGas
{
    public class NaturalGasGroupByDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalUsage { get; set; }
    }
}
