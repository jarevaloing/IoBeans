using System;
using System.Collections.Generic;

namespace IoBeans.Models
{
    public partial class SensorDatum
    {
        public int ReadingId { get; set; }
        public int SensorId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? SoilMoisture { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
