using Core.Shared;
using MediatR;

namespace Core.Features.WaterFeatures.Commands.CreateWater
{
    public class CreateWaterCommand : IRequest<Result<CreateWaterResponse>>
    {
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
    }
}
