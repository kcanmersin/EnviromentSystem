using Core.Data.Entity;
using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;

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
                BuildingId = request.BuildingId,
                Date = request.Date,
                InitialMeterValue = request.InitialMeterValue,
                FinalMeterValue = request.FinalMeterValue,
                Usage = request.Usage,
                KWHValue = request.KWHValue,
                CreatedDate = DateTime.UtcNow
            };

            _context.Electrics.Add(electric);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateElectricResponse
            {
                Id = electric.Id,
                BuildingId = electric.BuildingId,
                Date = electric.Date,
                InitialMeterValue = electric.InitialMeterValue,
                FinalMeterValue = electric.FinalMeterValue,
                Usage = electric.Usage,
                KWHValue = electric.KWHValue,
                CreatedDate = electric.CreatedDate
            };

            return Result.Success(response);
        }
    }
}
