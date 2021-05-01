using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NETTEST2.Models.Cars
{
    public class CarDetails
    {
        [Key()]
        [ForeignKey("Car")]
        public int CarID { get; set; }
        public int Power { get; set; }
        public int Torque { get; set; }
        public int TopSpeed { get; set; }

        public decimal AccelTime { get; set; }
        public decimal Price { get; set; }
        public decimal GasMileage { get; set; }
        public decimal CurbWeight { get; set; }

        public string GearboxType { get; set; }
        public string DriveType { get; set; }
        public string EngineType { get; set; }
        public string CarType { get; set; }

        public virtual Car Car { get; set; }
    }
}