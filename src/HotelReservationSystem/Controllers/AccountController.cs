using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace HotelReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //check if email is already taken
            if (_db.Users.Any(u=> u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            //create user with a BCrypt-hashed password
            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Guest"
            };
            _db.Users.Add(user);
            _db.SaveChanges();

            //create the linked guest record
            var guest = new Guest
            {
                UserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone
            };
            _db.Guests.Add(guest);
            _db.SaveChanges();

            //log a guest in by storing his/her ID in the session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserName", guest.FirstName);

            //if there's a saved return url, go there instead of home
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.Remove("ReturnUrl");
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //find the user by email
            var user = _db.Users.FirstOrDefault(u=>u.Email == model.Email);

            //verify password with BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            //store session data
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);

            var guest = _db.Guests.FirstOrDefault(g => g.UserId == user.Id);
            if (guest != null)
                HttpContext.Session.SetString("UserName", guest.FirstName);
            else
            {
                var staff = _db.Staff.FirstOrDefault(s => s.UserId == user.Id);
                if (staff != null)
                    HttpContext.Session.SetString("UserName", staff.FirstName);
            }

            //check for return url
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.Remove("ReturnUrl");
                return Redirect(returnUrl); //takes the guest exactly where they left off
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }




    }
}
