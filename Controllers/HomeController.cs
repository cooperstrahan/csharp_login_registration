using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private UserContext dbContext;
        public HomeController(UserContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(Register createdUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == createdUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    User newUser = new User(createdUser);
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    LoggedIn.SetLogin(HttpContext, newUser.UserID);
                    return RedirectToAction("Success");
                }
            }
            else
            {
            return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(Login user)
        {
            if(ModelState.IsValid)
            {
                User toValidate = dbContext.Users.FirstOrDefault(u => u.Email == user.LogEmail);
                if(toValidate == null)
                {
                    ModelState.AddModelError("LogEmail", "Email is not correct!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                var validate = Hasher.VerifyHashedPassword(toValidate, toValidate.Password, user.LogPassword);
                if(validate == 0)
                {
                    ModelState.AddModelError("LogPassword", "Password is not correct!");
                    return View("Index");
                }
                else
                {
                    LoggedIn.SetLogin(HttpContext, toValidate.UserID);
                    return RedirectToAction("Success");
                }  
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserLoggedIn = LoggedIn.GetUserID(HttpContext);
            if(UserLoggedIn != 0)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("Email", "You Must be logged in to access that page!");
                return View("Index");
            }   
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
