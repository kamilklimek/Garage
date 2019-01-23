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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ClassicGarageAuth.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private GarageContext db = new GarageContext();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        
        // GET: Car/Owner/5

        [HttpGet]
        public ActionResult Owner()
        {
            var owner = db.OwnerModels.Where(o => o.UserID == user.Id).Single();
            return View(owner.Car.ToList());
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            CarModel carModel = db.Car.Where(c => c.ID == id).Single();
           
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
            }

        // GET: Car/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = db.OwnerModels.Where(owner => owner.UserID == user.Id).Single().ID;
            return View();
        }

        // POST: Car/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Make,Model,Year,VIN,Picture,PurchaseDate,PurchaseAmount, OwnerID")] CarModel carModel)
        {

            

            if (ModelState.IsValid)
            {
                carModel.SalesDate = new DateTime(1900, 1, 1);
                db.Car.Add(carModel);
                db.SaveChanges();
                return RedirectToAction("Owner");
            }

            return View(carModel);
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = db.Car.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = db.OwnerModels.Where(o => o.UserID == user.Id).Single().ID;
            return View(carModel);
        }

        // POST: Car/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Make,Model,Year,VIN,Picture,PurchaseDate,PurchaseAmount,SalesDate,SalesAmount,OwnerID")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Owner");
            }
            ViewBag.OwnerID = new SelectList(db.OwnerModels, "ID", "FirstName");
            return View(carModel);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = db.Car.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarModel carModel = db.Car.Find(id);
            db.Car.Remove(carModel);
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
