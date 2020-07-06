using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Florist.Data;
using Florist.Models;
using Florist.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            var Flower = await _db.Flower.ToListAsync();
            return View(Flower);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST(Flower flower)
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
    }
}
