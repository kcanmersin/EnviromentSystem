using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoValidator : AbstractValidator<CreateSchoolInfoCommand>
    {
        public CreateSchoolInfoValidator()
        {
            RuleFor(x => x.NumberOfPeople).GreaterThan(0).WithMessage("Number of people must be greater than 0.");
            RuleFor(x => x.Year).InclusiveBetween(2000, DateTime.Now.Year).WithMessage("Year must be between 2000 and the current year.");
            RuleFor(x => x.CarsManagedByUniversity).GreaterThanOrEqualTo(0).WithMessage("Number of cars managed by the university must be non-negative.");
            RuleFor(x => x.CarsEnteringUniversity).GreaterThanOrEqualTo(0).WithMessage("Number of cars entering the university must be non-negative.");
            RuleFor(x => x.MotorcyclesEnteringUniversity).GreaterThanOrEqualTo(0).WithMessage("Number of motorcycles entering the university must be non-negative.");
        }
    }
}
