using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NETTEST2.Models.Offers;
using NETTEST2.Models.Cars;
using NETTEST2.Models.Context;
using NETTEST2.Models.CarViewModel;
using NETTEST2.Models;
using System.IO;

namespace NETTEST2.Controllers
{
    public class TestController : Controller
    {
        private ApplicationUserManager _userManager;

        public TestController()
        {
        }

        public TestController(ApplicationUserManager userManager)
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

        // GET: User
        [Route("User")]
        public async Task<ActionResult> ShowAsync(Models.UserPage model)
        {
            var userData = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (userData == null)
            {
                return View("Error");
            }
            model.Email = userData.Email;
            model.Id = userData.Id;
            model.PasswordHash = userData.PasswordHash;
            model.UserName = userData.UserName;
            model.UserAuth = true;
            model.Offers = userData.Offers;

            if (!ModelState.IsValid)
            {
                return new EmptyResult();
            }

            return View(model);
        }

        // GET: User
        [Route("User/{id}")]
        public async Task<ActionResult> ShowAsync(Models.UserPage model, string id)
        {
            var userData = await this.UserManager.FindByIdAsync(id);
            if (userData == null)
            {
                return View("Error");
            }

            model.Email         = userData.Email;
            model.Id            = userData.Id;
            model.PasswordHash  = userData.PasswordHash;
            model.UserName      = userData.UserName;
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

            return View(model);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveUser()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            await UserManager.DeleteAsync(user);
            HttpContext.GetOwinContext()
            .Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("ShowAll", "User");
        }

        [Route("Admin/All")]
        [Authorize(Users = "karoljez97@gmail.com")]
        public ActionResult ShowAll()
        {
            var modelList = new List<Models.UserPage>();
            var userDataList = UserManager.Users.ToList();
            if (userDataList == null)
            {
                return View("Error");
            }
            foreach (var userData in userDataList)
            {

                modelList.Add(new Models.UserPage()
                {
                    Email = userData.Email,
                    Id = userData.Id,
                    PasswordHash = userData.PasswordHash,
                    UserName = userData.UserName
                });
            }
            if (!ModelState.IsValid)
            {
                return new EmptyResult();
            }

            return View(modelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOffer(CarView view)
        {
            ImageModel model = new ImageModel();
            model.ImageFile= Request.Files["ImageFile"];
            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName.ToString());
            string extension = Path.GetExtension(model.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            model.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            model.ImageFile.SaveAs(fileName);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var details = new CarDetails()
            {
                AccelTime = Convert.ToDecimal(view.accel, System.Globalization.CultureInfo.InvariantCulture),
                CarType=view.cartype,
                CurbWeight= Convert.ToDecimal(view.curb, System.Globalization.CultureInfo.InvariantCulture),
                DriveType=view.drive,
                EngineType=view.engine,
                GasMileage= Convert.ToDecimal(view.gas, System.Globalization.CultureInfo.InvariantCulture),
                GearboxType=view.gear,
                Power= Convert.ToInt32(view.power),
                Price=Convert.ToDecimal(view.price, System.Globalization.CultureInfo.InvariantCulture),
                TopSpeed= Convert.ToInt32(view.top),
                Torque= Convert.ToInt32(view.torque)
            };

            var imagesList = new List<ImageModel>();
            imagesList.Add(model);
            var carList = new List<Car>();

            var car = new Car() 
            {
                Brand = view.brand,
                Model = view.model,
                YearOfManufacture = Convert.ToInt32(view.yom),
                CarDetails = details,
                ImageModels = imagesList
            };
            carList.Add(car);
            var offer = new Offer()
            {
                Name = view.name,
                Price = Convert.ToDecimal(view.price, System.Globalization.CultureInfo.InvariantCulture),
                Cars = carList
            };
            user.Offers.Add(offer);
            await UserManager.UpdateAsync(user);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveOffer(int indexid)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var element = user.Offers.Where(e => e.OfferID == indexid).FirstOrDefault();
            user.Offers.Remove(element);
            await UserManager.UpdateAsync(user);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}