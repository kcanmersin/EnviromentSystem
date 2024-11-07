using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Commands.UpdateElectric
{
    public class UpdateElectricCommand : IRequest<Result<UpdateElectricResponse>>
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
        public decimal KWHValue { get; set; }
    }
}
