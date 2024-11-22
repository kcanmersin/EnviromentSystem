using Core.Shared;
using MediatR;

namespace Core.Features.WaterFeatures.Commands.DeleteWater
{
    public class DeleteWaterCommand : IRequest<Result<DeleteWaterResponse>>
    {
        public Guid Id { get; set; }
    }
}
