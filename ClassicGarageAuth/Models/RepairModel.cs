using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassicGarageAuth.Models
{
    public class RepairModel
    {

        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public double CostAmount { get; set; }
        public int CarModelID { get; set; }

        public virtual CarModel CarModel { get; set; }
    }
}