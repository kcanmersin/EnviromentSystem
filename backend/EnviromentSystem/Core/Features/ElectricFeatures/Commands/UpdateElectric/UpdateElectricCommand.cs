using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Commands.UpdateElectric
{
    public class UpdateElectricCommand : IRequest<Result<UpdateElectricResponse>>
    {
        public Guid Id { get; set; }
        public Guid SchoolInfoId { get; set; }
        public decimal Consumption { get; set; }
        public decimal Cost { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
    }
}
