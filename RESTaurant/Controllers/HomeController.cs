using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTaurant.Models;

namespace RESTaurant.Controllers
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
            Restaur test = new Restaur();
            return View(test);
        }

        [HttpPost("Submit")]
        public IActionResult Submit(Restaur restaur)
        {
            Console.WriteLine("################");
            Console.WriteLine(restaur.Name);
            Console.WriteLine("################");
            if(ModelState.IsValid)
            {
                Restaur newReview = new Restaur
                {
                    Name = restaur.Name,
                    Location = restaur.Location,
                    Review = restaur.Review,
                    Visit = restaur.Visit,
                    Rating = restaur.Rating
                };

                _context.Add(restaur);

                _context.SaveChanges();
                
                return RedirectToAction("Reviews");
            }
            else
            {
                Console.WriteLine("#############################");
                Console.WriteLine(ModelState.ErrorCount);
                

                return View("Index", restaur);
            }
        }

        [HttpGet("Reviews")]
        public IActionResult Reviews()
        {
            IEnumerable<Restaur> AllReviews = _context.Restaurants;

            return View("Reviews", AllReviews);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
