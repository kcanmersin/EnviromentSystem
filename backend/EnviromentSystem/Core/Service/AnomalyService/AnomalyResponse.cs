using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.AnomalyService
{
    public class Anomaly
    {
        public double Anomaly_Error { get; set; }
        public DateTime Date { get; set; }
    }

    public class AnomalyResponse
    {
        public List<Anomaly> Anomalies { get; set; }
    }

}
