using Core.Features.WaterFeatures.Commands.DeleteWater;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Commands.DeletePaper
{
    public class DeletePaperCommand : IRequest<Result<DeletePaperResponse>>
    {
        public Guid Id { get; set; }
    }
}
