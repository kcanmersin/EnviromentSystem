using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Queries.GetElectricById
{
    public class GetElectricByIdQuery : IRequest<Result<GetElectricByIdResponse>>
    {
        public Guid Id { get; set; }
    }
}
