using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Typ")]
        public string CouponType { get; set; }
        
        public enum ECouponType { Procent = 0, PLN=1}

        [Required]
        [Display(Name = "Zniżka")]
        public double Discount { get; set; }

        [Required]
        [Display(Name = "Minimalna kwota")]
        public double MinimumAmount { get; set; }

        [Display(Name = "Aktywny")]
        public bool isActive { get; set; }
    }
}
