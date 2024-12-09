using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.PredictionService
{
    public class PredictionResponse
    {
        public DateTimeOffset Date { get; set; }
        public double PredictedUsage { get; set; }
    }

}
