using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
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
                    HttpContext.Session.SetInt32("UserInSession", user.Id);

                    return RedirectToAction("Account", new{id=user.Id});

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
                    HttpContext.Session.SetInt32("UserInSession", user.Id);
                    return RedirectToAction("Account", new{id=user.Id});
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

        [HttpGet("Accounts/{id}")]
        public IActionResult Account(int id)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserInSession");
            if(SessionId == null || SessionId != id)
            {
                TempData["AccessDenied"] = "You do not have permission to visit that page!";
                return RedirectToAction("Index");
            }
            else
            {
                User user = _context.user.FirstOrDefault(u => u.Id == id);

                ViewData["user"] = user.firstName;

                decimal Total = 0.00m;

                List<Transaction> transactions = _context.Transaction.Where(t => t.userId == user.Id).ToList();

                foreach(var transaction in transactions)
                {
                    decimal DeciVal = (decimal)transaction.value;
                    Total += DeciVal;
                }

                ViewData["balance"] = Total;

                return View("Account", transactions);
            }
        }

        [HttpPost("transactions")]
        public IActionResult Transactions(decimal value)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserInSession");
            if(SessionId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {

                decimal DeciVal = (decimal)value;
            Transaction transaction = new Transaction
            {
                value = DeciVal
            };
            _context.Transaction.Add(transaction);
            User user = _context.user.FirstOrDefault(u => u.Id == SessionId);

            user.transactions.Add(transaction);
            _context.SaveChanges();
            
            return RedirectToAction("Account", new{id = user.Id});
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
