using ClassicGarageAuth.DAL;
using ClassicGarageAuth.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ClassicGarageAuth.Security
{

    public class OwnerSecurityManager
    {
        private GarageContext db = new GarageContext();
        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        public bool shouldReturn403ForbiddenRequest(int ?OwnerID)
        {
            var owner = db.OwnerModels.Where(o => o.ID == OwnerID).Single();
            return owner.UserID != user.Id;
        }
    }




}