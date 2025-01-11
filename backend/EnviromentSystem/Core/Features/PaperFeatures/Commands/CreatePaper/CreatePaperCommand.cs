using Core.Features.WaterFeatures.Commands.CreateWater;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Commands.CreatePaper
{
    public class CreatePaperCommand : IRequest<Result<CreatePaperResponse>>
    {
        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
    }
}
