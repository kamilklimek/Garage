using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassicGarageAuth.DAL;
using ClassicGarageAuth.Security;
using ClassicGarageAuth.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ClassicGarageAuth.Controllers
{
    public class AdsController : Controller
    {
        private GarageContext db = new GarageContext();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        private OwnerSecurityManager ownerSecurityManager = new OwnerSecurityManager();

        // GET: Ads
        public ActionResult Index()
        {
            var activeAds = db.AdsModels.Where(ads => ads.IsActive == true).ToList();
            ViewBag.MyCars = new List<CarModel>();
            if (Request.IsAuthenticated)
            {
                var Owner = db.OwnerModels.Where(o => o.UserID == user.Id).Single();
                var myCars = db.Car.Where(c => c.OwnerID == Owner.ID).ToList();
               
                ViewBag.MyCars = myCars;
            }
            return View(activeAds);
        }
        [Authorize]
        public ActionResult Owner()
        {
            var UserId = user.Id;
            var OwnerId = db.OwnerModels.Where(o => o.UserID == UserId).Single().ID;
            var myCars =db.Car.Where(c => c.OwnerID == OwnerId).ToList();
            var ads = db.AdsModels.ToList();
            var myAds = new List<AdsModel>();

            ads.ForEach(ad =>
            {
                if (myCars.Any(c => c.ID == ad.CarModelsID))
                {
                    myAds.Add(ad);
                }
            });

            return View(myAds);
        }

        [Authorize]
        public ActionResult State(int ?id)
        {
            var AD = db.AdsModels.Where(ad => ad.ID == id).Single();
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(AD.CarModels.OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            AD.IsActive = !AD.IsActive;
            db.SaveChanges();
            return RedirectToAction("Owner");
        }


        // GET: Ads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdsModel adsModel = db.AdsModels.Find(id);
            adsModel.visitCounter += 1;
            db.SaveChanges();

            ViewBag.OwnerLogged = adsModel.CarModels.Owner.UserID == user.Id;

            if (adsModel == null)
            {
                return HttpNotFound();
            }
            return View(adsModel);
        }

        [Authorize]
        // GET: Ads/Create
        public ActionResult Create()
        {
            var Cars = db.OwnerModels.Where(o => o.UserID == user.Id).Single().Car;
            var carsWithoutAssignedAd = new List<CarModel>();

            foreach (CarModel car in Cars)
            {
                if (!db.AdsModels.Any(ad => ad.CarModelsID == car.ID))
                {
                    carsWithoutAssignedAd.Add(car);
                }
            }

            ViewBag.Cars = carsWithoutAssignedAd;
            ViewBag.CarsNoExists = carsWithoutAssignedAd.Count() == 0;
            ViewBag.LoggedUserId = user.Id; 

            return View();
        }

        // POST: Ads/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsActive, title, price, CarModelsID")] AdsModel adsModel)
        {
            if (ModelState.IsValid)
            {
                db.AdsModels.Add(adsModel);
                db.SaveChanges();
                return RedirectToAction("Owner");
            }

            return View(adsModel);
        }

        // GET: Ads/Edit/5

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdsModel adsModel = db.AdsModels.Find(id);

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(adsModel.CarModels.OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (adsModel == null)
            {
                return HttpNotFound();
            }
            return View(adsModel);
        }

        // POST: Ads/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,CarModelsID,IsActive, title, price")] AdsModel adsModel)
        {

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(adsModel.CarModels.OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(adsModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Owner");
            }
            return View(adsModel);
        }

        // GET: Ads/Delete/5

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdsModel adsModel = db.AdsModels.Find(id);

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(adsModel.CarModels.OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (adsModel == null)
            {
                return HttpNotFound();
            }
            return View(adsModel);
        }

        // POST: Ads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            AdsModel adsModel = db.AdsModels.Find(id);

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(adsModel.CarModels.OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.AdsModels.Remove(adsModel);
            db.SaveChanges();
            return RedirectToAction("Owner");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
