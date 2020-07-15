using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Florist.Data;
using Florist.Models;
using Florist.Models.ViewModel;
using Florist.Utility;
using Microsoft.AspNetCore.Http;
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

            if(HttpContext.Session.GetString(SD.ssCouponCode)!=null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }
            return View(detailsCard);
        }

        public async Task<IActionResult> Summary()
        {
            detailsCard = new OrderDetailsCard()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailsCard.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                detailsCard.listCart = cart.ToList();
            }

            foreach (var list in detailsCard.listCart)
            {
                list.Flower = await _db.Flower.FirstOrDefaultAsync(m => m.Id == list.FlowerId);
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotal + (list.Flower.Price * list.Count);
            }
            detailsCard.OrderHeader.OrderTotalOriginal = detailsCard.OrderHeader.OrderTotal;
            detailsCard.OrderHeader.PickupName = applicationUser.FirstName + " " + applicationUser.LastName;
            detailsCard.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            detailsCard.OrderHeader.PickupTime = DateTime.Now;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }

            return View(detailsCard);
        }

        public IActionResult AddCoupon()
        {
            if(detailsCard.OrderHeader.CouponCode==null)
            {
                detailsCard.OrderHeader.CouponCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, detailsCard.OrderHeader.CouponCode);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.SetString(SD.ssCouponCode, string.Empty);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if(cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                await _db.SaveChangesAsync();
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);

            _db.ShoppingCart.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            detailsCard.listCart = await _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();


            detailsCard.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailsCard.OrderHeader.OrderDate = DateTime.Now;
            detailsCard.OrderHeader.UserId = claim.Value;
            detailsCard.OrderHeader.Status = SD.PaymentStatusPending;
            detailsCard.OrderHeader.PickupTime = Convert.ToDateTime(detailsCard.OrderHeader.PickupDate.ToShortDateString() + " " + detailsCard.OrderHeader.PickupTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailsCard.OrderHeader);
            await _db.SaveChangesAsync();

            detailsCard.OrderHeader.OrderTotalOriginal = 0;


            foreach (var item in detailsCard.listCart)
            {
                item.Flower = await _db.Flower.FirstOrDefaultAsync(m => m.Id == item.FlowerId);
                OrderDetails orderDetails = new OrderDetails
                {
                    FlowerId = item.FlowerId,
                    OrderId = detailsCard.OrderHeader.Id,
                    Description = item.Flower.Description,
                    Name = item.Flower.Name,
                    Price = item.Flower.Price,
                    Count = item.Count
                };
                detailsCard.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);
            }
            
            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotalOriginal;
            }
            detailsCard.OrderHeader.CouponCodeDiscount = detailsCard.OrderHeader.OrderTotalOriginal = detailsCard.OrderHeader.OrderTotal;
            await _db.SaveChangesAsync();
            _db.ShoppingCart.RemoveRange(detailsCard.listCart);
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, 0);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
            return RedirectToAction("Confirm", "Order", new { id = detailsCard.OrderHeader.Id });
        }
    }
}
