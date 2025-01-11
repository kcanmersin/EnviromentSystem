using Core.Data.Entity.EntityBases;
using System;

namespace Core.Data.Entity
{
    public class CampusVehicleEntry : EntityBase
    {

        public int CarsManagedByUniversity { get; set; } 


        public int CarsEnteringUniversity { get; set; }


        public int MotorcyclesEnteringUniversity { get; set; } 


    }
}
