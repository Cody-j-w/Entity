using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
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
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            
            if(ModelState.IsValid)
            {
                User CheckEmail = _context.user.SingleOrDefault(x => x.email == user.email);
                if(CheckEmail == null)
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.password = Hasher.HashPassword(user, user.password);

                    User newUser = new User
                    {
                        firstName = user.firstName,
                        lastName = user.lastName,
                        email = user.email,
                        password = user.password
                    };

                    
                    _context.user.Add(newUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserInSession", user.UserId);
                    HttpContext.Session.SetString("UserName", user.firstName);

                    return RedirectToAction("Dashboard");

                }
                else
                {
                    ModelState.AddModelError("email", "This email is already taken!");
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(string EmailLogin, string PassLogin)
        {

            var user = _context.user.SingleOrDefault(use => use.email == EmailLogin) as User;
            if(user != null && PassLogin != null)
            {

                var Hasher = new PasswordHasher<User>();
                var result = Hasher.VerifyHashedPassword(user, user.password, PassLogin);
                if(result != 0)
                {
                    HttpContext.Session.SetInt32("UserInSession", user.UserId);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["LoginErrors"] = "Login information invalid!";
                    return View("Index");
                }
            }
            else
            {
                TempData["LoginErrors"] = "Login information invalid!";
                return View("Index");
            }
        }

        [HttpGet("weddings/new")]
        public IActionResult NewWedding()
        {
            if(HttpContext.Session.GetInt32("UserInSession") == null)
            {
                TempData["AccessDenied"] = "Please log in first!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost("weddings/create")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            
            if(ModelState.IsValid)
            {
                int? SessionId = HttpContext.Session.GetInt32("UserInSession");
                Wedding NewWedding = new Wedding()
                {
                    wedderOne = wedding.wedderOne,
                    wedderTwo = wedding.wedderTwo,
                    weddingDay = wedding.weddingDay,
                    address = wedding.address
                };

                User user = _context.user.FirstOrDefault(u => u.UserId == SessionId);
                user.weddings.Add(NewWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("NewWedding");
            }
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("UserInSession") == null)
            {
                TempData["AccessDenied"] = "Please log in first!";
                return RedirectToAction("Index");
            }
            else
            {
                List<Wedding> AllWeddings = new List<Wedding>();
                ViewModel DashView = new ViewModel();
                DashView.PersonalId = HttpContext.Session.GetInt32("UserInSession");
                AllWeddings = _context.wedding.Include(w => w.guests).ThenInclude(g => g.User).ToList();
                List<User> AllUsers = new List<User>();
                AllUsers = _context.user.Include(u => u.RSVPs).ThenInclude(r => r.Wedding).ToList();


                DashView.Weddings = AllWeddings;
                


           return View("Dashboard", DashView);
            }
        }

        [HttpPost("weddings/RSVP")]
        public IActionResult RSVP(int RSVPer, int RSVPed)
        {

            User user = _context.user.SingleOrDefault(u => u.UserId == RSVPer);
            Wedding wedding = _context.wedding.SingleOrDefault(w => w.WeddingId == RSVPed);

            RSVP rsvp = new RSVP();

            rsvp.User = user;
            rsvp.Wedding = wedding;

            _context.rsvp.Add(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("wedding/{id}")]
        public IActionResult OneWedding(int id)
        {

            Wedding wedding = _context.wedding.Include(w => w.guests).ThenInclude(g => g.User).SingleOrDefault(w => w.WeddingId == id);
            if(wedding == null)
            {
                TempData["RSVPError"] = "We cannot find that wedding!";
                return RedirectToAction("Dashboard");
            }
            else{
            ViewModel WedView = new ViewModel();
            WedView.Wedding = wedding;
            return View("WeddingDetails", WedView);
            }

        }

        [HttpPost("weddings/cancel")]
        public IActionResult UnRSVP(int weddingId)
        {
            User user = _context.user.Include(u => u.RSVPs).SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserInSession"));
            Wedding wedding = _context.wedding.Include(w => w.guests).SingleOrDefault(w => w.WeddingId == weddingId);

            if(user != null && wedding != null)
            {
                RSVP RSVPToRemove = _context.rsvp.SingleOrDefault(r => r.UserId == user.UserId && r.WeddingId == wedding.WeddingId);

                _context.rsvp.Remove(RSVPToRemove);
                _context.SaveChanges();
                
                return RedirectToAction("Dashboard");
                
            }
            else
            {
                TempData["RSVPError"] = "That wedding does not exist!";
                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
