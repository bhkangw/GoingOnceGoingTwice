using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using csharpbelt.Models;
using Microsoft.AspNetCore.Identity;

namespace csharpbelt.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterUser newUser) // takes in a RegisterUser object from register form
        {
            if (_context.users.Where(u => u.Username == newUser.Username).SingleOrDefault() != null)
                ModelState.AddModelError("Username", "Username already in use"); // checking if Username already exists in the db

            PasswordHasher<RegisterUser> hasher = new PasswordHasher<RegisterUser>(); // necessary for password hashing
            
            if (ModelState.IsValid) // if model passes validation, create new User instance from User class
            {
                User User = new User 
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Username = newUser.Username,
                    Password = hasher.HashPassword(newUser, newUser.Password),
                    Wallet = 1000,
                    HeldAmount =  0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Add(User); // add the new user object to the db
                _context.SaveChanges();

                HttpContext.Session.SetInt32("id", User.UserId); // save the user id in session
                return RedirectToAction("Index", "Auction"); // redirect to the Wall controller
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginUser logUser) // takes in the LoginUser from the login form
        {
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();

            User userToLog = _context.users.Where(u => u.Username == logUser.LogUsername).SingleOrDefault();
            // User userToLog = _context.users.SingleOrDefault(u => u.Username == logUser.LogUsername); // can use this or above?

            if(userToLog == null) // if Username is not in db..
                ModelState.AddModelError("LogUsername", "Cannot find Username");

            else if( hasher.VerifyHashedPassword(logUser, userToLog.Password, logUser.LogPassword) == 0) // if password does not match 
            {
                ModelState.AddModelError("LogPassword", "Wrong Password");
            }

            if(!ModelState.IsValid) // if form is empty or any other basic validation is failed
                return View("Index");

            HttpContext.Session.SetInt32("id", userToLog.UserId); // if pass all of the above, save the user id in session
            return RedirectToAction("Index", "Auction"); // and redirect to the Wall controller
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // clear all user data in session upon logout
            return RedirectToAction("Index");
        }
    }
}
