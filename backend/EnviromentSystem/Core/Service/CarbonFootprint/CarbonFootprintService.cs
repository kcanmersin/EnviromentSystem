using Core.Data;
using Core.Service.CarbonFootprint;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.CarbonFootprint
{
    public class CarbonFootprintService : ICarbonFootprintService
    {
        private const decimal ElectricityEmissionFactor = 0.84m;
        private const decimal ShuttleBusEmissionFactor = 0.01m;
        private const decimal CarEmissionFactor = 0.02m;
        private const decimal MotorcycleEmissionFactor = 0.01m;
        private const int WorkingDaysPerYear = 240;

        private readonly ApplicationDbContext _context;

        public CarbonFootprintService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to get the carbon footprint for a specific year
        public async Task<CarbonFootprintForYearDto> GetCarbonFootprintByYearAsync(int year)
        {
            var schoolInfo = await _context.SchoolInfos
     .Include(s => s.Vehicles)  // Include the Vehicles property (CampusVehicleEntry)
     .Where(s => s.Year == year)
     .FirstOrDefaultAsync();


            if (schoolInfo == null)
                return null;

            // Fetch electricity usage for the given year
            var electricityUsage = await _context.Electrics
                .Where(e => e.Date.Year == year)
                .SumAsync(e => e.KWHValue);

            // Get vehicle data (assuming these fields are populated correctly)
            var shuttleBusCount = 1;  // Example value, should be fetched based on your real data
            var totalTripsPerDay = 5; // Example value
            var shuttleBusTravelDistanceKm = 5;  // Example value
            var numberOfCars = schoolInfo.Vehicles.CarsEnteringUniversity;
            var carTravelDistanceKm = 6;  // Example value
            var numberOfMotorcycles = schoolInfo.Vehicles.MotorcyclesEnteringUniversity;
            var motorcycleTravelDistanceKm = 6;  // Example value

            // Calculate emissions for each source
            decimal electricityEmission = (electricityUsage / 1000) * ElectricityEmissionFactor;
            decimal shuttleBusEmission = (shuttleBusCount * totalTripsPerDay * shuttleBusTravelDistanceKm * WorkingDaysPerYear / 100) * ShuttleBusEmissionFactor;
            decimal carEmission = (numberOfCars * 2 * carTravelDistanceKm * WorkingDaysPerYear / 100) * CarEmissionFactor;
            decimal motorcycleEmission = (numberOfMotorcycles * 2 * motorcycleTravelDistanceKm * WorkingDaysPerYear / 100) * MotorcycleEmissionFactor;

            decimal totalEmission = electricityEmission + shuttleBusEmission + carEmission + motorcycleEmission;

            return new CarbonFootprintForYearDto
            {
                Year = year,
                ElectricityEmission = electricityEmission,
                ShuttleBusEmission = shuttleBusEmission,
                CarEmission = carEmission,
                MotorcycleEmission = motorcycleEmission,
                TotalEmission = totalEmission
            };
        }

        // Method to get the carbon footprint for all years
        public async Task<CarbonFootprintForAllYearsDto> GetCarbonFootprintForAllYearsAsync()
        {
            var schoolInfos = await _context.SchoolInfos
            .Include(s => s.Vehicles)
            .ToListAsync();

            var electricData = await _context.Electrics.ToListAsync();

            var yearlyFootprints = schoolInfos
                .GroupBy(s => s.Year)
                .Select(group =>
                {
                    var year = group.Key;

                    // Get the electricity usage for the year
                    var electricityUsage = electricData
                        .Where(e => e.Date.Year == year)
                        .Sum(e => e.KWHValue);

                    // Get vehicle data (shuttle, cars, motorcycles)
                    var schoolInfo = group.First();
                    var shuttleBusCount = 1;  // Example value, should be fetched based on your real data
                    var totalTripsPerDay = 5; // Example value
                    var shuttleBusTravelDistanceKm = 5;  // Example value
                    var numberOfCars = schoolInfo.Vehicles.CarsEnteringUniversity;
                    var carTravelDistanceKm = 6;  // Example value
                    var numberOfMotorcycles = schoolInfo.Vehicles.MotorcyclesEnteringUniversity;
                    var motorcycleTravelDistanceKm = 6;  // Example value

                    // Calculate emissions for each source
                    decimal electricityEmission = (electricityUsage / 1000) * ElectricityEmissionFactor;
                    decimal shuttleBusEmission = (shuttleBusCount * totalTripsPerDay * shuttleBusTravelDistanceKm * WorkingDaysPerYear / 100) * ShuttleBusEmissionFactor;
                    decimal carEmission = (numberOfCars * 2 * carTravelDistanceKm * WorkingDaysPerYear / 100) * CarEmissionFactor;
                    decimal motorcycleEmission = (numberOfMotorcycles * 2 * motorcycleTravelDistanceKm * WorkingDaysPerYear / 100) * MotorcycleEmissionFactor;

                    decimal totalEmission = electricityEmission + shuttleBusEmission + carEmission + motorcycleEmission;

                    return new CarbonFootprintForYearDto
                    {
                        Year = year,
                        ElectricityEmission = electricityEmission,
                        ShuttleBusEmission = shuttleBusEmission,
                        CarEmission = carEmission,
                        MotorcycleEmission = motorcycleEmission,
                        TotalEmission = totalEmission
                    };
                })
                .ToList();

            return new CarbonFootprintForAllYearsDto
            {
                YearlyFootprints = yearlyFootprints
            };
        }
    }
}
