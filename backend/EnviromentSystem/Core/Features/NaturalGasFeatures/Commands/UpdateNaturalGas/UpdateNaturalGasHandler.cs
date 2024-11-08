using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.UpdateNaturalGas
{
    public class UpdateNaturalGasHandler : IRequestHandler<UpdateNaturalGasCommand, Result<UpdateNaturalGasResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateNaturalGasCommand> _validator;

        public UpdateNaturalGasHandler(ApplicationDbContext context, IValidator<UpdateNaturalGasCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<UpdateNaturalGasResponse>> Handle(UpdateNaturalGasCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<UpdateNaturalGasResponse>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));

            var naturalGas = await _context.NaturalGasUsages.FirstOrDefaultAsync(ng => ng.Id == request.Id, cancellationToken);
            if (naturalGas == null)
                return Result.Failure<UpdateNaturalGasResponse>(new Error("NotFound", "Natural gas usage not found."));

            naturalGas.BuildingId = request.BuildingId;
            naturalGas.Date = request.Date;
            naturalGas.InitialMeterValue = request.InitialMeterValue;
            naturalGas.FinalMeterValue = request.FinalMeterValue;
            naturalGas.Usage = request.Usage;
            naturalGas.SM3Value = request.SM3Value;
            naturalGas.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpdateNaturalGasResponse
            {
                Id = naturalGas.Id,
                BuildingId = naturalGas.BuildingId,
                Date = naturalGas.Date,
                InitialMeterValue = naturalGas.InitialMeterValue,
                FinalMeterValue = naturalGas.FinalMeterValue,
                Usage = naturalGas.Usage,
                SM3Value = naturalGas.SM3Value,
                Success = true,
                Message = "Natural gas record updated successfully."
            });
        }
    }

}
