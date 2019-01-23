using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ClassicGarageAuth.Models;

namespace ClassicGarageAuth.DAL
{
    public class GarageContext : DbContext
    {
        public DbSet<CarModel> Car { get; set; }
        public DbSet<OwnerModel> OwnerModels { get; set; }
        public DbSet<AdsModel> AdsModels { get; set; }
        public DbSet<RepairModel> RepairModels { get; set; }
        public DbSet<PartModel> PartModels { get; set; }
    }
}