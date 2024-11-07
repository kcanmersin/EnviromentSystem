using Core.Features.ElectricFeatures.Commands.CreateElectric;
using Core.Shared;
using MediatR;

public class CreateElectricCommand : IRequest<Result<CreateElectricResponse>>
{
    public Guid BuildingId { get; set; }
    public decimal InitialMeterValue { get; set; }
    public decimal FinalMeterValue { get; set; }
    public decimal Usage { get; set; }
    public decimal KWHValue { get; set; }
    public DateTime Date { get; set; }
}
