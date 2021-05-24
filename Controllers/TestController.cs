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
using System.Diagnostics;

namespace NETTEST2.Controllers
{
    public class TestController : BaseController
    {

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
            model.Money = userData.Money;
            model.ImageModelUsers = userData.ImageModelsUser;
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
            model.Money         = userData.Money;
            model.ImageModelUsers = userData.ImageModelsUser;
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
        public async Task<ActionResult> RemoveUser(string userid)
        {
            bool iLikeCookies = false;
            if (userid == User.Identity.GetUserId())
                iLikeCookies = true;
            var user = await UserManager.FindByIdAsync(userid);
            await UserManager.DeleteAsync(user);
            if(iLikeCookies)
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("ShowAll", "User");
        }

        [Route("Admin/All")]
        [Authorize(Roles = "Admin")]
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
                    UserName = userData.UserName,
                    Money = userData.Money
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
        public async Task<ActionResult> AddPhoto(ImageModelUser model)
        {
            model.ImageFile = Request.Files["ImageFile"];
            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName.ToString());
            string extension = Path.GetExtension(model.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            model.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            model.ImageFile.SaveAs(fileName);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.ImageModelsUser.Add(model);
            await UserManager.UpdateAsync(user);
            return Redirect(Request.UrlReferrer.ToString());
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
        public async Task<ActionResult> RemoveOffer(string userid,int indexid)
        {
            var user = await UserManager.FindByIdAsync(userid);
            var element = user.Offers.Where(e => e.OfferID == indexid).FirstOrDefault();
            user.Offers.Remove(element);
            await UserManager.UpdateAsync(user);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BuyOffer(string userid, int indexid)
        {
            var buyer = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var seller = await UserManager.FindByIdAsync(userid);
            var offer = seller.Offers.Where(e => e.OfferID == indexid).FirstOrDefault();
            if (buyer.Money >= offer.Price)
            {
                seller.Money += offer.Price;
                buyer.Money -= offer.Price;
                seller.Offers.Remove(offer);
                buyer.Offers.Add(offer);
                await UserManager.UpdateAsync(seller);
                await UserManager.UpdateAsync(buyer);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}