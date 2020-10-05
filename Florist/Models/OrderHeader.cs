using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string PayUId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Data odbioru")]
        public DateTime OrderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Cena całkowita")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name = "Czas odioru")]
        public DateTime PickupTime { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Data odbioru")]
        public DateTime PickupDate { get; set; }

        [Display(Name = "Kupon")]
        public string CouponCode { get; set; }

        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }

        [Display(Name = "Odbiorca")]
        public string PickupName { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        public string TransactionId { get; set; }
    }
}
