using Core.Data;
using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.DeleteNaturalGas
{
    public class DeleteNaturalGasHandler : IRequestHandler<DeleteNaturalGasCommand, Result<DeleteNaturalGasResponse>>
    {
        private readonly ApplicationDbContext _context;

        public DeleteNaturalGasHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DeleteNaturalGasResponse>> Handle(DeleteNaturalGasCommand request, CancellationToken cancellationToken)
        {
            var naturalGas = await _context.NaturalGasUsages.FindAsync(request.Id);
            if (naturalGas == null)
                return Result.Failure<DeleteNaturalGasResponse>(new Error("NotFound", "Natural gas usage not found."));

            _context.NaturalGasUsages.Remove(naturalGas);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteNaturalGasResponse { Id = request.Id });
        }
    }

}
