using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassicGarageAuth.Models
{
    public class PartModel
    {
    
        [Required]
        public int ID { get; set; }
        [Required]
        public string PartName { get; set; }
        public string CatalogNumber { get; set; }
        public double PurchaseAmount { get; set; }
        public double SalesAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SalesDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }

        public int CarModelsID { get; set; }
        public virtual CarModel CarModels { get; set; }
    }
}