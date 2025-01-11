using Core.Data;
using Core.Features.WaterFeatures.Commands.DeleteWater;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Commands.DeletePaper
{
    public class DeletePaperHandler : IRequestHandler<DeletePaperCommand, Result<DeletePaperResponse>>
    {
        private readonly ApplicationDbContext _context;

        public DeletePaperHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DeletePaperResponse>> Handle(DeletePaperCommand request, CancellationToken cancellationToken)
        {
            var paper = await _context.Papers.FindAsync(request.Id);
            if (paper == null)
            {
                return Result.Failure<DeletePaperResponse>(new Error("NotFound", "Paper record not found."));
            }

            _context.Papers.Remove(paper);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeletePaperResponse { Id = paper.Id });
        }
    }
}
