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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        //CREATE - POST
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
            var PlaceFromdb = _db.Flower.Find(flower.Id);

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
            var PlaceFromdb = _db.Flower.Find(flower.Id);
            string img = PlaceFromdb.Image;
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flower = await _db.Flower.FindAsync(id);
            if (flower == null)
            {
                return NotFound();
            }

            return View(flower);
        }
    }
}

