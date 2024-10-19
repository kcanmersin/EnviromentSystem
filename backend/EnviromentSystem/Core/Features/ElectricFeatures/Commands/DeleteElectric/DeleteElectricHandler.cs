using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Core.Features.ElectricFeatures.Commands.DeleteElectric
{
    public class DeleteElectricHandler : IRequestHandler<DeleteElectricCommand, Result<DeleteElectricResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<DeleteElectricCommand> _validator;

        public DeleteElectricHandler(ApplicationDbContext context, IValidator<DeleteElectricCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<DeleteElectricResponse>> Handle(DeleteElectricCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<DeleteElectricResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var electric = await _context.Electrics.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (electric == null)
            {
                return Result.Failure<DeleteElectricResponse>(
                    new Error("ElectricNotFound", "Electric record not found."));
            }

            _context.Electrics.Remove(electric);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteElectricResponse
            {
                Id = electric.Id,
                Success = true,
                Message = "Electric record deleted successfully."
            });
        }
    }
}
