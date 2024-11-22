using Core.Data.Entity;
using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;

namespace Core.Features.WaterFeatures.Commands.CreateWater
{
    public class CreateWaterHandler : IRequestHandler<CreateWaterCommand, Result<CreateWaterResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateWaterCommand> _validator;

        public CreateWaterHandler(ApplicationDbContext context, IValidator<CreateWaterCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CreateWaterResponse>> Handle(CreateWaterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateWaterResponse>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var water = new Water
            {
                Date = request.Date,
                InitialMeterValue = request.InitialMeterValue,
                FinalMeterValue = request.FinalMeterValue,
                Usage = request.Usage,
                CreatedDate = DateTime.UtcNow
            };

            _context.Add(water);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateWaterResponse
            {
                Id = water.Id,
                Date = water.Date,
                InitialMeterValue = water.InitialMeterValue,
                FinalMeterValue = water.FinalMeterValue,
                Usage = water.Usage
            };

            return Result.Success(response);
        }
    }
}
