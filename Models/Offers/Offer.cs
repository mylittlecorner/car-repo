using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NETTEST2.Models.Cars;

namespace NETTEST2.Models.Offers
{
    public class Offer
    {
        public int OfferID { get; set; }
        public string ApplicationUserId { get; set; }
        public int CarID { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required()]
        [Range(0, 100000000.00, ErrorMessage = "Invalid Target Price; Max 100000000.00")]
        public decimal Price { get; set; }
        public virtual List<Car> Cars { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}