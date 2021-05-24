using NETTEST2.Models.Cars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NETTEST2.Models
{
    public class ImageModelUser
    {
        public int ImageModelUserID { get; set; }
        public String ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}