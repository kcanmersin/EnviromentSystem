using System.Collections.Generic;
using Core.Features.NaturalGasFeatures.Queries.GetNaturalGasById;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGas
{
    public class GetAllNaturalGasResponse
    {
        public List<GetNaturalGasByIdResponse> NaturalGas { get; set; } = new();
    }
}
