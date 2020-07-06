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
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Zdjęcie")]
        public string Image { get; set; }

        [Display(Name = "Urodziny")]
        public bool Birthday_Occasion { get; set; }
        [Display(Name = "Imieniny")]
        public bool NameDay_Occasion { get; set; }
        [Display(Name = "Ślub")]
        public bool Wedding_Occasion { get; set; }
        [Display(Name = "Narodziny")]
        public bool Birth_Occasion { get; set; }
        [Display(Name = "Podziękowania")]
        public bool Thanks_Occasion { get; set; }
        [Display(Name = "Gratulacje")]
        public bool Congratulations_Occasion { get; set; }
        [Display(Name = "Kondolencje")]
        public bool Condolence_Occasion { get; set; }
        [Display(Name = "Rocznica")]
        public bool Anniversary_Occasion { get; set; }

        [Display(Name = "Róże")]
        public bool Roses_Flowers { get; set; }
        [Display(Name = "Słoneczniki")]
        public bool Sunflowers_Flowers { get; set; }
        [Display(Name = "Gerbery")]
        public bool Gerbers_Flowers { get; set; }
        [Display(Name = "Bukiety mieszane")]
        public bool Bouquets_Flowers { get; set; }

        [Display(Name = "Flowerbox róże")]
        public bool Roses_Flowerbox { get; set; }
        [Display(Name = "Flowerbox goździki")]
        public bool Cloves_Flowerbox { get; set; }
        [Display(Name = "Flowerbox mix")]
        public bool Mix_Flowerbox { get; set; }

        [Display(Name = "Miś z róż")]
        public bool TeddyBear_Gift { get; set; }
        [Display(Name = "Słodki bukiet")]
        public bool SweetBouquets_Gift { get; set; }
        [Display(Name = "Kawy i herbaty")]
        public bool TeaAndCofee_Gift { get; set; }
        [Display(Name = "Narodziny")]
        public bool Basket_Gift { get; set; }
        [Display(Name = "Balony z helem")]
        public bool Balloon_Gift { get; set; }

        [Display(Name = "Cena")]
        [Range(1,int.MaxValue, ErrorMessage ="Cena powinna być większa niż 1zł")]
        public double Price { get; set; }
    }
}
