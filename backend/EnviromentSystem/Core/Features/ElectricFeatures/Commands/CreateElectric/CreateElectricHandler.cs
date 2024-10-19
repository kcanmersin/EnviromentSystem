using Core.Data;
using Core.Data.Entity;
using Core.Shared;
using MediatR;
using FluentValidation;

namespace Core.Features.ElectricFeatures.Commands.CreateElectric
{
    public class CreateElectricHandler : IRequestHandler<CreateElectricCommand, Result<CreateElectricResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateElectricCommand> _validator;

        public CreateElectricHandler(ApplicationDbContext context, IValidator<CreateElectricCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CreateElectricResponse>> Handle(CreateElectricCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateElectricResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var electric = new Electric
            {
                SchoolInfoId = request.SchoolInfoId,
                Consumption = request.Consumption,
                Cost = request.Cost,
                //Year = request.Year,
                //Month = request.Month,
                CreatedDate = DateTime.UtcNow
            };

            _context.Electrics.Add(electric);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateElectricResponse
            {
                Id = electric.Id,
                SchoolInfoId = electric.SchoolInfoId,
                Consumption = electric.Consumption,
                Cost = electric.Cost,
                //Year = electric.Year,
                //Month = electric.Month,
                CreatedDate = electric.CreatedDate
            };

            return Result.Success(response);
        }
    }
}
