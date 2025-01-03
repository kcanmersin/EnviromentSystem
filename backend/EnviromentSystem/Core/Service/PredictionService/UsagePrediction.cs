﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.PredictionService
{
    public class UsagePrediction
    {
        public DateTime Date { get; set; }
        public double Predicted_Usage { get; set; }
    }

    public class UsagePredictionResponse
    {
        public List<UsagePrediction> Predictions { get; set; }
    }


}
