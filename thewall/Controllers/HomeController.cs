using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using thewall.Models;

namespace thewall.Controllers
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

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
           if(HttpContext.Session.GetInt32("UserInSession") != null)
           {
               ViewModel DashView = new ViewModel();
               DashView.User = _context.user.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserInSession"));
               List<Message> AllMessages = _context.message.Include(m => m.comments).ThenInclude(c => c.User).ToList();
               DashView.Messages = AllMessages;
               return View("dashboard", DashView);
           }
           else
           {
               TempData["AccessDenied"] = "Please log in!";
               return RedirectToAction("Index");
           }

            
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
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

        [HttpPost("login")]
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

        [HttpPost("submitMessage")]
        public IActionResult Submit(string post)
        {
            if(post == null)
            {
                TempData["MessageError"] = "Please enter a message";
            }
            else if(post.Length < 5)
            {
                TempData["MessageError"] = "Message must be at least five characters";
            }
            else if(post.Length > 250)
            {
                TempData["MessageError"] = "Message must be less than 250 characters";
            }
            else
            {
                int? SessionId = HttpContext.Session.GetInt32("UserInSession");

                User user = _context.user.FirstOrDefault(u => u.UserId == SessionId);
                Message newMessage = new Message()
                {
                    message = post
                };
                user.messages.Add(newMessage);
                _context.SaveChanges();
                
            }
            return RedirectToAction("Dashboard");
            
        }

        [HttpPost("submitComment")]
        public IActionResult Comment(string post, int messageid)
        {
            if(post == null)
            {
                TempData["CommentError"] = "Please enter a comment";
                TempData["ErrorId"] = messageid;
            }
            else if(post.Length < 5)
            {
                TempData["CommentError"] = "Comment must be at least five characters";
                TempData["ErrorId"] = messageid;
            }
            else if(post.Length > 250)
            {
                TempData["CommentError"] = "Comment must be less than 250 characters";
                TempData["ErrorId"] = messageid;
            }
            else
            {
                int? SessionId = HttpContext.Session.GetInt32("UserInSession");
                User user = _context.user.FirstOrDefault(u => u.UserId == SessionId);
                Message message = _context.message.FirstOrDefault(m => m.MessageId == messageid);

                Comment newComment = new Comment()
                {
                    comment = post,
                    User = user,
                    Message = message
                };

                _context.comment.Add(newComment);
                _context.SaveChanges();
                
            };
            return RedirectToAction("Dashboard");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
