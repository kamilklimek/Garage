using System;
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
    public class RepairController : Controller
    {
        private GarageContext db = new GarageContext();
        private OwnerSecurityManager ownerSecurityManager = new OwnerSecurityManager();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        
        public ActionResult Owner()
        {
            var owner = db.OwnerModels.Where(o => o.UserID == user.Id).Single();
            ViewBag.TotalCostsOfRepair = getTotalCostOfParts(owner.Car);
            return View(owner.Car);
        }

        private double getTotalCostOfParts(ICollection<CarModel> cars)
        {
            List<RepairModel> repairs = new List<RepairModel>();
            foreach (CarModel car in cars)
            {
                repairs.AddRange(car.RepairModels);
            }

            double totalCosts = 0.0f;

            foreach (RepairModel repair in repairs)
            {
                totalCosts += repair.CostAmount;
            }

            return totalCosts;
        }

        // GET: RepairModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairModel repairModel = db.RepairModels.Find(id);
            if (repairModel == null)
            {
                return HttpNotFound();
            }
            return View(repairModel);
        }

        // GET: RepairModels/Create
        public ActionResult Create()
        {
            var Owner = db.OwnerModels.Where(owner => owner.UserID == user.Id).Single();
            ViewBag.Cars = Owner.Car;
            return View();
        }

        // POST: RepairModels/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CarID,Name,Description,CostAmount, CarModelID")] RepairModel repairModel)
        {
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == repairModel.CarModelID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.RepairModels.Add(repairModel);
                db.SaveChanges();
                return RedirectToAction("Owner");
            }

            return View(repairModel);
        }

        // GET: RepairModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairModel repairModel = db.RepairModels.Find(id);
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == repairModel.CarModelID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (repairModel == null)
            {
                return HttpNotFound();
            }
            return View(repairModel);
        }

        // POST: RepairModels/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CarModelID,Name,Description,CostAmount")] RepairModel repairModel)
        {
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == repairModel.CarModelID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Entry(repairModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Owner");
            }
            return View(repairModel);
        }

        // GET: RepairModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairModel repairModel = db.RepairModels.Find(id);
            if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == repairModel.CarModelID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (repairModel == null)
            {
                return HttpNotFound();
            }
            return View(repairModel);
        }

        // POST: RepairModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RepairModel repairModel = db.RepairModels.Find(id);
             if (ownerSecurityManager.shouldReturn403ForbiddenRequest(db.Car.Where(c => c.ID == repairModel.CarModelID).Single().OwnerID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.RepairModels.Remove(repairModel);
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
