﻿using Core.Data.Entity.EntityBases;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entity
{
    public class Water : EntityBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal InitialMeterValue { get; set; }

        [Required]
        public decimal FinalMeterValue { get; set; }

        [Required]
        public decimal Usage { get; set; }
    }
}
