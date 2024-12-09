using Core.Data.Entity.EntityBases;
using System;

namespace Core.Data.Entity
{
    public class CampusVehicleEntry : EntityBase
    {
        // Number of cars managed by the university (owned and hired)
        public int CarsManagedByUniversity { get; set; } 

        // Number of cars entering the university on a particular day
        public int CarsEnteringUniversity { get; set; }

        // Number of motorcycles entering the university on a particular day
        public int MotorcyclesEnteringUniversity { get; set; } 


    }
}
