using NETTEST2.Models.Cars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NETTEST2.Models
{
    public class ImageModel
    {
        public int ImageModelID { get; set; }
        public String ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual Car Car { get; set; }
    }
}