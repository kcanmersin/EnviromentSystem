using Core.Data.Entity;
using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.CreateNaturalGas
{
    public class CreateNaturalGasHandler : IRequestHandler<CreateNaturalGasCommand, Result<CreateNaturalGasResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateNaturalGasCommand> _validator;

        public CreateNaturalGasHandler(ApplicationDbContext context, IValidator<CreateNaturalGasCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CreateNaturalGasResponse>> Handle(CreateNaturalGasCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<CreateNaturalGasResponse>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));

            var naturalGas = new NaturalGas
            {
                BuildingId = request.BuildingId,
                Date = request.Date,
                InitialMeterValue = request.InitialMeterValue,
                FinalMeterValue = request.FinalMeterValue,
                Usage = request.Usage,
                SM3Value = request.SM3Value
            };

            _context.NaturalGasUsages.Add(naturalGas);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateNaturalGasResponse
            {
                Id = naturalGas.Id,
                BuildingId = naturalGas.BuildingId,
                Date = naturalGas.Date,
                InitialMeterValue = naturalGas.InitialMeterValue,
                FinalMeterValue = naturalGas.FinalMeterValue,
                Usage = naturalGas.Usage,
                SM3Value = naturalGas.SM3Value
            });
        }
    }

}
