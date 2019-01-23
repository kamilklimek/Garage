using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClassicGarageAuth.Models
{
    public class OwnerModel
    {
        
        public int ID { get; set; }
        [Required]
        [MaxLength(45)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(45)]
        public string LastName { get; set; }
        [MaxLength(9)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public String UserID { get; set; }
        
        public virtual ICollection<CarModel> Car { get; set; }
    }
}