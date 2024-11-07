using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ElectricFeatures.Commands.UpdateElectric
{
    public class UpdateElectricHandler : IRequestHandler<UpdateElectricCommand, Result<UpdateElectricResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateElectricCommand> _validator;

        public UpdateElectricHandler(ApplicationDbContext context, IValidator<UpdateElectricCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<UpdateElectricResponse>> Handle(UpdateElectricCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UpdateElectricResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var electric = await _context.Electrics.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (electric == null)
            {
                return Result.Failure<UpdateElectricResponse>(
                    new Error("ElectricNotFound", "Electric record not found."));
            }

            electric.BuildingId = request.BuildingId;
            electric.Date = request.Date;
            electric.InitialMeterValue = request.InitialMeterValue;
            electric.FinalMeterValue = request.FinalMeterValue;
            electric.Usage = request.Usage;
            electric.KWHValue = request.KWHValue;
            electric.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            var response = new UpdateElectricResponse
            {
                Id = electric.Id,
                BuildingId = electric.BuildingId,
                Date = electric.Date,
                InitialMeterValue = electric.InitialMeterValue,
                FinalMeterValue = electric.FinalMeterValue,
                Usage = electric.Usage,
                KWHValue = electric.KWHValue,
                Success = true,
                Message = "Electric record updated successfully."
            };

            return Result.Success(response);
        }
    }
}
