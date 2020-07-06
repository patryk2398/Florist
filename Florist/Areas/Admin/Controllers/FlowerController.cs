using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Florist.Data;
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
    }
}
