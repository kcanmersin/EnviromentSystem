using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.DeleteSchoolInfo
{
    public class DeleteSchoolInfoValidator : AbstractValidator<DeleteSchoolInfoCommand>
    {
        public DeleteSchoolInfoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
