public class ConsumptionDataDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal InitialMeterValue { get; set; }
    public decimal FinalMeterValue { get; set; }
    public decimal Usage { get; set; }
    public decimal? KWHValue { get; set; } // Optional for Electric
    public decimal? SM3Value { get; set; } // Optional for NaturalGas
    public string BuildingName { get; set; } // Optional for Water
}
