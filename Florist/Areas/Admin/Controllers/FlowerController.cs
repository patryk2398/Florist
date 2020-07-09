using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Florist.Data;
using Florist.Models;
using Florist.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Florist.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FlowerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FlowerController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Flower flower { get; set; }

        #region Index
        public async Task<IActionResult> Index()
        {
            var Flower = await _db.Flower.ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Roses_Flowers()
        {
            var Flower = await _db.Flower.Where(t => t.Roses_Flowers == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Sunflowers_Flowers()
        {
            var Flower = await _db.Flower.Where(t => t.Sunflowers_Flowers == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Gerbers_Flowers()
        {
            var Flower = await _db.Flower.Where(t => t.Gerbers_Flowers == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Bouquets_Flowers()
        {
            var Flower = await _db.Flower.Where(t => t.Bouquets_Flowers == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Birthday_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Birthday_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_NameDay_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.NameDay_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Wedding_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Wedding_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Birth_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Birth_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Thanks_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Thanks_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Congratulations_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Congratulations_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Condolence_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Condolence_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Anniversary_Occasion()
        {
            var Flower = await _db.Flower.Where(t => t.Anniversary_Occasion == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Roses_Flowerbox()
        {
            var Flower = await _db.Flower.Where(t => t.Roses_Flowerbox == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Cloves_Flowerbox()
        {
            var Flower = await _db.Flower.Where(t => t.Cloves_Flowerbox == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Mix_Flowerbox()
        {
            var Flower = await _db.Flower.Where(t => t.Mix_Flowerbox == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_TeddyBear_Gift()
        {
            var Flower = await _db.Flower.Where(t => t.TeddyBear_Gift == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_SweetBouquets_Gift()
        {
            var Flower = await _db.Flower.Where(t => t.SweetBouquets_Gift == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_TeaAndCofee_Gift()
        {
            var Flower = await _db.Flower.Where(t => t.TeaAndCofee_Gift == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Basket_Gift()
        {
            var Flower = await _db.Flower.Where(t => t.Basket_Gift == true).ToListAsync();
            return View(Flower);
        }
        public async Task<IActionResult> Index_Balloon_Gift()
        {
            var Flower = await _db.Flower.Where(t => t.Balloon_Gift == true).ToListAsync();
            return View(Flower);
        }
        #endregion

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //CREATE - POST
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            _db.Flower.Add(flower);
            await _db.SaveChangesAsync();

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var flowerFromDb = await _db.Flower.FindAsync(flower.Id);

            if(files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "img");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads,flower.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                flowerFromDb.Image = @"\img\" + flower.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"img\" + SD.DefaultFlowerImage);
                System.IO.File.Copy(uploads, webRootPath + @"\img\" + flower.Id + ".png");
                flowerFromDb.Image = @"\img\" + flower.Id + ".png";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //EDIT - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var flower = await _db.Flower.FindAsync(id);
            if(flower==null)
            {
                return NotFound();
            }

            return View(flower);
        }

        //EDIT - POST
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string img = ImgFromDb(flower);

            _db.Attach(flower).State = EntityState.Modified;

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var FlowerFromdb = _db.Flower.Find(flower.Id);

            if (files.Count == 0)
            {
                flower.Image = img;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, "img");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, flower.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, flower.Id + extension));
                }

                extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
                using (var fileStream = new FileStream(Path.Combine(uploads, flower.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                flower.Image = @"\img\" + flower.Id + extension;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string ImgFromDb(Flower flower)
        {
            var FlowerFromdb = _db.Flower.Find(flower.Id);
            string img = FlowerFromdb.Image;
            var local = _db.Set<Flower>()
            .Local
            .FirstOrDefault(entry => entry.Id.Equals(flower.Id));
            if (local != null)
            {
                _db.Entry(local).State = EntityState.Detached;
            }
            _db.SaveChanges();
            return img;
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flowerFromDb = await _db.Flower.FindAsync(id);

            ShoppingCart cartObj = new ShoppingCart()
            {
                Flower = flowerFromDb,
                FlowerId = flowerFromDb.Id
            };

            return View(cartObj);
        }


        //POST - DETAILS
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart cartObj)
        {
            cartObj.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObj.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = await _db.ShoppingCart.Where(c => c.ApplicationUserId == cartObj.ApplicationUserId
                                          && c.FlowerId == cartObj.FlowerId).FirstOrDefaultAsync();
                
                if(cartFromDb == null)
                {
                    await _db.ShoppingCart.AddAsync(cartObj);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + cartObj.Count;
                }
                await _db.SaveChangesAsync();

                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == cartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count);

                return RedirectToAction("Index");
            }
            else
            {
                var flowerFromDb = await _db.Flower.FindAsync(cartObj.FlowerId);

                ShoppingCart cartObject = new ShoppingCart()
                {
                    Flower = flowerFromDb,
                    FlowerId = flowerFromDb.Id
                };
                return View(cartObject);
            }
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Flower.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var flower = await _db.Flower.FindAsync(id);

            if (flower == null)
            {
                return View();
            }
            _db.Flower.Remove(flower);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

