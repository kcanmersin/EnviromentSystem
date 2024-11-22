﻿namespace Core.Features.WaterFeatures.Commands.UpdateWater
{
    public class UpdateWaterResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialMeterValue { get; set; }
        public decimal FinalMeterValue { get; set; }
        public decimal Usage { get; set; }
    }
}