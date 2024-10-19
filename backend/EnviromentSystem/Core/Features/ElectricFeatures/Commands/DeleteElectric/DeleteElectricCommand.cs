using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Commands.DeleteElectric
{
    public class DeleteElectricCommand : IRequest<Result<DeleteElectricResponse>>
    {
        public Guid Id { get; set; }
    }
}
