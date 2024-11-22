using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;

namespace Core.Features.WaterFeatures.Commands.UpdateWater
{
    public class UpdateWaterHandler : IRequestHandler<UpdateWaterCommand, Result<UpdateWaterResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateWaterCommand> _validator;

        public UpdateWaterHandler(ApplicationDbContext context, IValidator<UpdateWaterCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<UpdateWaterResponse>> Handle(UpdateWaterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UpdateWaterResponse>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var water = await _context.Waters.FindAsync(request.Id);
            if (water == null)
            {
                return Result.Failure<UpdateWaterResponse>(new Error("NotFound", "Water record not found."));
            }

            water.Date = request.Date;
            water.InitialMeterValue = request.InitialMeterValue;
            water.FinalMeterValue = request.FinalMeterValue;
            water.Usage = request.Usage;
            water.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpdateWaterResponse
            {
                Id = water.Id,
                Date = water.Date,
                InitialMeterValue = water.InitialMeterValue,
                FinalMeterValue = water.FinalMeterValue,
                Usage = water.Usage
            });
        }
    }
}
