using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NETTEST2.Models.Offers;

namespace NETTEST2.Models.Cars
{
    public class Car
    {
        public int CarID { get; set; }
        public int ImageModelID { get; set; }
        [Required()]
        [StringLength(25)]
        public string Brand { get; set; }
        [Required()]
        [StringLength(25)]
        public string Model { get; set; }
        [Range(1850, 2031, ErrorMessage = "Please enter valid integer Number")]
        public int YearOfManufacture { get; set; }
        public virtual List<ImageModel> ImageModels { get; set; }
        public virtual CarDetails CarDetails { get; set; }
        public virtual Offer Offer { get; set; }
    }
}