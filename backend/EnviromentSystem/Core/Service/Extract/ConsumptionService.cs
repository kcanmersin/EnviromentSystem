using Core.Data;
using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Service.Extract
{
    public class ConsumptionService : IConsumptionService
    {
        private readonly ApplicationDbContext _context;

        public ConsumptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConsumptionDataDto>> GetConsumptionDataAsync(string consumptionType)
        {
            List<ConsumptionDataDto> data = new List<ConsumptionDataDto>();

            if (consumptionType == "Electric")
            {
                data = await _context.Electrics
                    .Select(e => new ConsumptionDataDto
                    {
                        Id = e.Id,
                        Date = e.Date,
                        InitialMeterValue = e.InitialMeterValue,
                        FinalMeterValue = e.FinalMeterValue,
                        Usage = e.Usage,
                        KWHValue = e.KWHValue,
                        BuildingName = e.Building.Name
                    })
                    .ToListAsync();
            }
            else if (consumptionType == "NaturalGas")
            {
                data = await _context.NaturalGasUsages
                    .Select(g => new ConsumptionDataDto
                    {
                        Id = g.Id,
                        Date = g.Date,
                        InitialMeterValue = g.InitialMeterValue,
                        FinalMeterValue = g.FinalMeterValue,
                        Usage = g.Usage,
                        SM3Value = g.SM3Value,
                        BuildingName = g.Building.Name
                    })
                    .ToListAsync();
            }
            else if (consumptionType == "Water")
            {
                data = await _context.Waters
                    .Select(w => new ConsumptionDataDto
                    {
                        Id = w.Id,
                        Date = w.Date,
                        InitialMeterValue = w.InitialMeterValue,
                        FinalMeterValue = w.FinalMeterValue,
                        Usage = w.Usage
                    })
                    .ToListAsync();
            }

            return data;
        }
    }
}