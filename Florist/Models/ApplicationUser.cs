using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Florist.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr domu")]
        public string HouseNumber { get; set; }

        [Display(Name = "Nr mieszkania")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
    }
}
