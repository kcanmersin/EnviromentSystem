using Core.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.CreateNaturalGas
{
    public class CreateNaturalGasValidator : AbstractValidator<CreateNaturalGasCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateNaturalGasValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.BuildingId).NotEmpty().WithMessage("BuildingId is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");
            RuleFor(x => x.SM3Value).GreaterThanOrEqualTo(0).WithMessage("SM3Value cannot be negative.");

            RuleFor(x => x)
                .MustAsync(NotExistInSameMonthAndYear)
                .WithMessage("A record for this building and month already exists.");
        }

        private async Task<bool> NotExistInSameMonthAndYear(CreateNaturalGasCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("Checking if a record for this building and month already exists.");
            return !await _context.NaturalGasUsages.AnyAsync(ng =>
                ng.BuildingId == command.BuildingId &&
                ng.Date.Year == command.Date.Year &&
                ng.Date.Month == command.Date.Month,
                cancellationToken);
        }
    }
}
