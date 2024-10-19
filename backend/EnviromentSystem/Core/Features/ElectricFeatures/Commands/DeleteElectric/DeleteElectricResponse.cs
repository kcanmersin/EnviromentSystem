namespace Core.Features.ElectricFeatures.Commands.DeleteElectric
{
    public class DeleteElectricResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
