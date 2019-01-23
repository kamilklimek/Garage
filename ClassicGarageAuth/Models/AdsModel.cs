using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClassicGarageAuth.Models
{
    public class AdsModel
    {
        [Required]
        public int ID { set; get; }

        [Required]
        public bool IsActive { set; get; }
        [Required]
        public String title { set; get; }
        [Required]
        public Double price { set; get; }
        
        public int visitCounter { set; get; }
        public int CarModelsID { set; get; }
        public virtual CarModel CarModels { set; get; }

    }
}