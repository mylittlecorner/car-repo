using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NETTEST2.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationUserManager _userManager;

        public BaseController()
        {
        }

        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}