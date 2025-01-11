using Core.Data;
using Core.Features.WaterFeatures.Commands.UpdateWater;
using Core.Shared;
using FluentValidation;
using MediatR;

namespace Core.Features.PaperFeatures.Commands.UpdatePaper
{
    public class UpdatePaperHandler : IRequestHandler<UpdatePaperCommand, Result<UpdatePaperResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdatePaperCommand> _validator;

        public UpdatePaperHandler(ApplicationDbContext context, IValidator<UpdatePaperCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<UpdatePaperResponse>> Handle(UpdatePaperCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UpdatePaperResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var paper = await _context.Papers.FindAsync(request.Id);
            if (paper == null)
            {
                return Result.Failure<UpdatePaperResponse>(new Error("NotFound", "Paper record not found."));
            }

            paper.Date = request.Date;
            paper.Usage = request.Usage;
            paper.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpdatePaperResponse
            {
                Id = paper.Id,
                Date = paper.Date,
                Usage = paper.Usage
            });
        }
    }
}
