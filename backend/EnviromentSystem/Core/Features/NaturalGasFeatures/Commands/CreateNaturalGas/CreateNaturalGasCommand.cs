﻿using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.CreateNaturalGas
{
    public class CreateNaturalGasCommand : IRequest<Result<CreateNaturalGasResponse>>
    {
        public Guid BuildingId { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
        public decimal SM3Value { get; set; }
    }

}
