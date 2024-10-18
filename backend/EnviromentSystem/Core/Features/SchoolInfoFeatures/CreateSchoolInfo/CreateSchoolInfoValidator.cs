using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.CreateSchoolInfo
{
    public class CreateSchoolInfoValidator : AbstractValidator<CreateSchoolInfoCommand>
    {
        public CreateSchoolInfoValidator()
        {
            RuleFor(x => x.NumberOfPeople).GreaterThan(0).WithMessage("Number of people must be greater than 0.");
            RuleFor(x => x.Year).InclusiveBetween(2000, DateTime.Now.Year).WithMessage("Year must be between 2000 and the current year.");
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
