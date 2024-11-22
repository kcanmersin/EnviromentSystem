using Core.Data;
using Core.Shared;
using MediatR;

namespace Core.Features.WaterFeatures.Commands.DeleteWater
{
    public class DeleteWaterHandler : IRequestHandler<DeleteWaterCommand, Result<DeleteWaterResponse>>
    {
        private readonly ApplicationDbContext _context;

        public DeleteWaterHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DeleteWaterResponse>> Handle(DeleteWaterCommand request, CancellationToken cancellationToken)
        {
            var water = await _context.Waters.FindAsync(request.Id);
            if (water == null)
            {
                return Result.Failure<DeleteWaterResponse>(new Error("NotFound", "Water record not found."));
            }

            _context.Waters.Remove(water);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteWaterResponse { Id = water.Id });
        }
    }
}
