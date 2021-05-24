using NETTEST2.Models.CarViewModel;
using NETTEST2.Models.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NETTEST2.Models
{
    public class UserPage
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public decimal Money { get; set; }
        public List<ImageModelUser> ImageModelUsers { get; set; }
        public bool UserAuth { get; set; }
        public List<Offer> Offers { get; set; }
        public CarView CarView { get; set; }
    }
}