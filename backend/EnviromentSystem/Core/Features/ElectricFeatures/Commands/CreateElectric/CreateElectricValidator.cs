using Core.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ElectricFeatures.Commands.CreateElectric
{
    public class CreateElectricValidator : AbstractValidator<CreateElectricCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateElectricValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.BuildingId).NotEmpty().WithMessage("BuildingId is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");
            RuleFor(x => x.KWHValue).GreaterThanOrEqualTo(0).WithMessage("KWHValue cannot be negative.");

            RuleFor(x => x)
                .MustAsync(NotExistInSameMonthAndYear)
                .WithMessage("A record for this building and month already exists.");
        }

        private async Task<bool> NotExistInSameMonthAndYear(CreateElectricCommand command, CancellationToken cancellationToken)
        {
            return !await _context.Electrics.AnyAsync(e =>
                e.BuildingId == command.BuildingId &&
                e.Date.Year == command.Date.Year &&
                e.Date.Month == command.Date.Month,
                cancellationToken);
        }
    }
}
