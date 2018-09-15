using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginAndReg.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginAndReg.Controllers
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

        [HttpGet("Success")]
        public IActionResult Success()
        {

            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            
            

            if(ModelState.IsValid){
                
                var CheckEmail = _context.User.SingleOrDefault(check => check.Email == user.Email);
                if(CheckEmail == null)
                {

                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);

                    User newUser = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password
                    };

                    
                    _context.Add(newUser);
                    _context.SaveChanges();

                    return RedirectToAction("Success");

                }
                
                else
                {
                    ModelState.AddModelError("Email", "This email is already taken!");
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

            var user = _context.User.SingleOrDefault(use => use.Email == EmailLogin) as User;
            if(user != null && PassLogin != null)
            {

                var Hasher = new PasswordHasher<User>();
                var result = Hasher.VerifyHashedPassword(user, user.Password, PassLogin);
                if(result != 0)
                {
                return RedirectToAction("Success");
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
