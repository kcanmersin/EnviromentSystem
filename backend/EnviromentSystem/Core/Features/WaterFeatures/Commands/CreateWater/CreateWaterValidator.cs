using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Core.Data;

namespace Core.Features.WaterFeatures.Commands.CreateWater
{
    public class CreateWaterValidator : AbstractValidator<CreateWaterCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateWaterValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");

            // Add the rule to check for records in the same month and year
             RuleFor(x => x)
                .MustAsync(NotExistInSameMonthAndYear)
                .WithMessage("A record for this month already exists.");
        }

        // Asynchronous method to check if a record for the same building and month already exists
        private async Task<bool> NotExistInSameMonthAndYear(CreateWaterCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("A record for this month already exists.");
            return !await _context.Waters.AnyAsync(w =>
                 w.Date.Year == command.Date.Year &&
                w.Date.Month == command.Date.Month,
                cancellationToken);
        }

    }
}
