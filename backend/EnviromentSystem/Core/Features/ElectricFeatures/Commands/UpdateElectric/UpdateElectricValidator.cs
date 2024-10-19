using FluentValidation;

namespace Core.Features.ElectricFeatures.Commands.UpdateElectric
{
    public class UpdateElectricValidator : AbstractValidator<UpdateElectricCommand>
    {
        public UpdateElectricValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.SchoolInfoId).NotEmpty().WithMessage("SchoolInfoId is required.");
            RuleFor(x => x.Consumption).GreaterThan(0).WithMessage("Consumption must be greater than 0.");
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.");
            RuleFor(x => x.Year).InclusiveBetween(2000, DateTime.Now.Year).WithMessage("Year must be valid.");
            RuleFor(x => x.Month)
                .NotEmpty()
                .Must(BeAValidMonth).WithMessage("Month must be a valid month name.");
        }

        private bool BeAValidMonth(string month)
        {
            var validMonths = new[]
            {
                "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
            };
            return validMonths.Contains(month);
        }
    }
}
