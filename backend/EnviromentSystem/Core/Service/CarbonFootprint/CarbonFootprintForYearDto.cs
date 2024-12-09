﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.CarbonFootprint
{
        public class CarbonFootprintForYearDto
        {
            public int Year { get; set; }
            public decimal ElectricityEmission { get; set; }
            public decimal ShuttleBusEmission { get; set; }
            public decimal CarEmission { get; set; }
            public decimal MotorcycleEmission { get; set; }
            public decimal TotalEmission { get; set; }
    }

}
