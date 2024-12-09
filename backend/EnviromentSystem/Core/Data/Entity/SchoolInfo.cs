using Core.Data.Entity.EntityBases;
using System.Collections.Generic;

namespace Core.Data.Entity
{
    public class SchoolInfo : EntityBase
    {
        public int NumberOfPeople { get; set; }

        public Guid? CampusVehicleEntryId { get; set; }

        public CampusVehicleEntry Vehicles { get; set; }

        public int Year { get; set; }
    }
}
