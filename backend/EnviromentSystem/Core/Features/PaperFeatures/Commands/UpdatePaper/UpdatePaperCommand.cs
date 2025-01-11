using Core.Features.WaterFeatures.Commands.UpdateWater;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Commands.UpdatePaper
{
    public class UpdatePaperCommand : IRequest<Result<UpdatePaperResponse>>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
    }
}
