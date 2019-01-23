using ClassicGarageAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassicGarageAuth.DAL;

namespace ClassicGarageAuth.Controllers
{
    public class HomeController : Controller
    {
        GarageContext db = new GarageContext();

        public List<AdsModel> getTheMostPopularAds()
        {
            var ads = db.AdsModels.OrderBy(ad => ad.visitCounter);
            var activeAds = ads.Where(ad => ad.IsActive == true).ToList();

            return activeAds;
        }
        public ActionResult Index()
        {
            ViewBag.PopularAds = getTheMostPopularAds();
            return View();
        }

    }
}