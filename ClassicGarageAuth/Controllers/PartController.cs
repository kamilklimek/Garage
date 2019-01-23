using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassicGarageAuth.DAL;
using ClassicGarageAuth.Models;
using ClassicGarageAuth.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ClassicGarageAuth.Controllers
{
    [Authorize]
    public class PartController : Controller
    {
        private GarageContext db = new GarageContext();
    
        private OwnerSecurityManager ownerSecurityManager = new OwnerSecurityManager();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        // GET: Car/Owner/5

        [HttpGet]
        public ActionResult Owner()
        {
            var owner = db.OwnerModels.Where(o => o.UserID == user.Id).Single();
            ViewBag.TotalCostOfParts = getTotalCostOfParts(owner.Car);
            return View(owner.Car);
        }

        private double getTotalCostOfParts(ICollection<CarModel> cars)
        {
            List<PartModel> parts = new List<PartModel>();
            foreach (CarModel car in cars)
            {
                parts.AddRange(car.PartModel);
            }

            double totalCosts = 0.0f;

            foreach (PartModel part in parts)
            {
                totalCosts += part.SalesAmount;
                totalCosts -= part.PurchaseAmount;              
            }

            return totalCosts;
        }


        // GET: Part/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartModel partModel = db.PartModels.Find(id);
            if (partModel == null)
            {
                return HttpNotFound();
            }
            return View(partModel);
        }

        // GET: Part/Create
        public ActionResult Create()
        {
            var Owner = db.OwnerModels.Where(owner => owner.UserID == user.Id).Single();
            ViewBag.Cars = Owner.Car;
            return View();
        }

        // POST: Part/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PartName,CatalogNumber,PurchaseAmount,SalesAmount,SalesDate,PurchaseDate,CarModelsID")] PartModel partModel)
        {
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == partModel.CarModelsID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.PartModels.Add(partModel);
                db.SaveChanges();
                return RedirectToAction("Owner");
            }

            return View(partModel);
        }

        // GET: Part/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartModel partModel = db.PartModels.Find(id);

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == partModel.CarModelsID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (partModel == null)
            {
                return HttpNotFound();
            }
            return View(partModel);
        }

        // POST: Part/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PartName,CatalogNumber,PurchaseAmount,SalesAmount,SalesDate,PurchaseDate,CarModelsID")] PartModel partModel)
        {
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == partModel.CarModelsID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(partModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Owner");
            }
            return View(partModel);
        }

        // GET: Part/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartModel partModel = db.PartModels.Find(id);

            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == partModel.CarModelsID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (partModel == null)
            {
                return HttpNotFound();
            }
            return View(partModel);
        }

        // POST: Part/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartModel partModel = db.PartModels.Find(id);
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == partModel.CarModelsID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.PartModels.Remove(partModel);
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
