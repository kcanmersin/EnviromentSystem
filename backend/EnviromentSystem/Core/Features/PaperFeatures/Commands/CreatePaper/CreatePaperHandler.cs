using Core.Data.Entity;
using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using Core.Features.WaterFeatures.Commands.CreateWater;

namespace Core.Features.PaperFeatures.Commands.CreatePaper
{
    public class CreatePaperHandler : IRequestHandler<CreatePaperCommand, Result<CreatePaperResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreatePaperCommand> _validator;

        public CreatePaperHandler(ApplicationDbContext context, IValidator<CreatePaperCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CreatePaperResponse>> Handle(CreatePaperCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreatePaperResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var paper = new Paper
            {
                Date = request.Date,
                Usage = request.Usage,
                CreatedDate = DateTime.UtcNow
            };

            _context.Add(paper);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreatePaperResponse
            {
                Id = paper.Id,
                Date = paper.Date,
                Usage = paper.Usage
            };

            return Result.Success(response);
        }
    }
}
