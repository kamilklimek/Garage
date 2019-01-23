using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClassicGarageAuth.Models
{
    public class CarModel
    {
        public int ID { get; set; }
        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year{ get; set; }
        
        [Required]
        public string VIN { get; set; }
        public string Picture { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }
        public double PurchaseAmount{ get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SalesDate{ get; set; }
        public double SalesAmount{ get; set; }
        public int OwnerID { get; set; }

        public virtual OwnerModel Owner { get; set; }
        public virtual ICollection<PartModel> PartModel { get; set; }
        public virtual ICollection<RepairModel> RepairModels { get; set; }
    }
}