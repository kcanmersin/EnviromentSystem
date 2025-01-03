﻿using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.UpdateSchoolInfo
{
    public class UpdateSchoolInfoValidator : AbstractValidator<UpdateSchoolInfoCommand>
    {
        public UpdateSchoolInfoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.NumberOfPeople).GreaterThan(0);
            RuleFor(x => x.Year).InclusiveBetween(2000, DateTime.UtcNow.Year);
            RuleFor(x => x.Month).NotEmpty().MaximumLength(15);
            RuleFor(x => x.CarsManagedByUniversity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CarsEnteringUniversity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.MotorcyclesEnteringUniversity).GreaterThanOrEqualTo(0);
        }
    }
}
