using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NETTEST2.Models;
using NETTEST2.Models.Context;
using NETTEST2.Models.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NETTEST2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
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
        public ActionResult Index()
        {
            var modelList=new List<UserPage>();
            var userIter = this.UserManager.Users.ToList();
            foreach (var userData in userIter)
            {
                if (userData == null)
                {
                    return View("Error");
                }
                var model = new UserPage();

                model.Email = userData.Email;
                model.Id = userData.Id;
                model.PasswordHash = userData.PasswordHash;
                model.UserName = userData.UserName;
                model.UserAuth = false;
                model.Offers = userData.Offers;

                if (!ModelState.IsValid)
                {
                    return new EmptyResult();
                }

                if (userData.Id == User.Identity.GetUserId())
                {
                    model.UserAuth = true;
                }
                modelList.Add(model);
            }
            return View(modelList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}