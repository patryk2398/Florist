using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set;}

        public int FlowerId { get; set; }
        [NotMapped]
        [ForeignKey("FlowerId")]
        public virtual Flower Flower { get; set; }

        [Range(1,int.MaxValue, ErrorMessage ="Wprowadź ilość większą lub równą 1")]
        public int Count { get; set; }
    }
}
