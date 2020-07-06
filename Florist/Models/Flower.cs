using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Models
{
    public class Flower
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public string Image { get; set; }
        
        public bool Birthday_Occasion { get; set; }
        public bool NameDay_Occasion { get; set; }
        public bool Wedding_Occasion { get; set; }
        public bool Birth_Occasion { get; set; }
        public bool Thanks_Occasion { get; set; }
        public bool Congratulations_Occasion { get; set; }
        public bool Condolence_Occasion { get; set; }
        public bool Anniversary_Occasion { get; set; }

        public bool Roses_Flowers { get; set; }
        public bool Sunflowers_Flowers { get; set; }
        public bool Gerbers_Flowers { get; set; }
        public bool Bouquets_Flowers { get; set; }

        public bool Roses_Flowerbox { get; set; }
        public bool Cloves_Flowerbox { get; set; }
        public bool Mix_Flowerbox { get; set; }

        public bool TeddyBear_Gift { get; set; }
        public bool SweetBouquets_Gift { get; set; }
        public bool TeaAndCofee_Gift { get; set; }
        public bool Basket_Gift { get; set; }
        public bool Balloon_Gift { get; set; }

        [Range(1,int.MaxValue, ErrorMessage ="Cena powinna być większa niż 1zł")]
        public double Price { get; set; }
    }
}
