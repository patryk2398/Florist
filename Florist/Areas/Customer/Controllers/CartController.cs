using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Florist.Data;
using Florist.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florist.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsCard detailsCard { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            detailsCard = new OrderDetailsCard()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailsCard.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if(cart != null)
            {
                detailsCard.listCart = cart.ToList();

            }

            foreach(var list in detailsCard.listCart)
            {
                list.Flower = await _db.Flower.FirstOrDefaultAsync(m => m.Id == list.FlowerId);
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotal + (list.Flower.Price * list.Count);
            }
            detailsCard.OrderHeader.OrderTotalOriginal = detailsCard.OrderHeader.OrderTotal;

            return View(detailsCard);
        }
    }
}
