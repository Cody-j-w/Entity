using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.Models;

namespace ProductManager.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            CatchAll IndexView = new CatchAll();

            IndexView.AllProducts = _context.Products;
            IndexView.AllCategories = _context.Categories;
            return View(IndexView);
        }
    }
}