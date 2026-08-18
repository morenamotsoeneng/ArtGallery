using ArtGallery.Data;
using ArtGallery.Models;
using ArtGallery.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // LOGIN (GET)
        // ===========================
        public IActionResult Login()
        {
            // If already logged in, go straight to dashboard
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            return View();
        }

        // ===========================
        // LOGIN (POST)
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Admins.FirstOrDefault(a =>
                    a.Email == model.Email &&
                    a.Password == model.Password);

                if (admin != null)
                {
                    // Create Session
                    HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                    HttpContext.Session.SetString("AdminName", admin.FullName);

                    TempData["Success"] = "Welcome, " + admin.FullName + "!";

                    return RedirectToAction(nameof(Dashboard));
                }

                ViewBag.Error = "Invalid email or password.";
            }

            return View(model);
        }

        // ===========================
        // REGISTER (GET)
        // ===========================
        public IActionResult Register()
        {
            return View();
        }

        // ===========================
        // REGISTER (POST)
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Admins.Any(a => a.Email == model.Email);

                if (emailExists)
                {
                    ModelState.AddModelError("Email",
                        "An account with this email already exists.");

                    return View(model);
                }

                Admin admin = new Admin
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password
                };

                _context.Admins.Add(admin);
                _context.SaveChanges();

                TempData["Success"] = "Registration successful. Please login.";

                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

        // ===========================
        // DASHBOARD
        // ===========================
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
            {
                return RedirectToAction("Login");
            }

            DashboardViewModel model = new DashboardViewModel
            {
                TotalArtworks = _context.ArtWorks.Count(),

                TotalCategories = _context.Categories.Count(),

                AvailableArtworks = _context.ArtWorks.Count(a => a.Status),

                SoldArtworks = _context.ArtWorks.Count(a => !a.Status)
            };

            return View(model);
        }

        // ===========================
        // LOGOUT
        // ===========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }
        //==========================
        // FORGOT PASSWORD
        //==========================

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = _context.Admins
                .FirstOrDefault(a => a.Email == model.Email);

            if (admin == null)
            {
                ViewBag.Error = "No administrator account was found with this email.";

                return View(model);
            }

            return RedirectToAction(
                "ResetPassword",
                new { email = model.Email }
            );
        }//==========================
         // RESET PASSWORD
         //==========================

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            ResetPasswordViewModel model =
                new ResetPasswordViewModel
                {
                    Email = email
                };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = _context.Admins
                .FirstOrDefault(a => a.Email == model.Email);

            if (admin == null)
            {
                ViewBag.Error = "Administrator account not found.";

                return View(model);
            }

            admin.Password = model.NewPassword;

            _context.SaveChanges();

            TempData["Success"] =
                "Password changed successfully. Please login.";

            return RedirectToAction(nameof(Login));
        }
    }
}