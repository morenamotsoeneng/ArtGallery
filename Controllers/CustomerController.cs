using ArtGallery.Data;
using ArtGallery.Models;
using ArtGallery.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        //==========================
        // REGISTER (GET)
        //==========================

        public IActionResult Register()
        {
            return View();
        }

        //==========================
        // REGISTER (POST)
        //==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Customers
                    .Any(c => c.Email == model.Email);

                if (emailExists)
                {
                    ViewBag.Error = "Email address already exists.";

                    return View(model);
                }

                Customer customer = new Customer
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };

                _context.Customers.Add(customer);

                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        //==========================
        // LOGIN (GET)
        //==========================

        public IActionResult Login()
        {
            return View();
        }

        //==========================
        // LOGIN (POST)
        //==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(CustomerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.FirstOrDefault(c =>
                    c.Email == model.Email &&
                    c.Password == model.Password);

                if (customer != null)
                {
                    HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);

                    HttpContext.Session.SetString("CustomerName", customer.FullName);

                    return RedirectToAction("Dashboard", "Customer");
                }

                ViewBag.Error = "Invalid email or password.";
            }

            return View(model);
        }

        //==========================
        // LOGOUT
        //==========================

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomerId");

            HttpContext.Session.Remove("CustomerName");

            return RedirectToAction("Login");
        }
        //==========================
        // Dashboard
        //==========================

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");


            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;



            ViewBag.Favourites = _context.Favourites
                .Count(f => f.CustomerId == customerId);



            ViewBag.CartItems = _context.CartItems
                .Count(c => c.CustomerId == customerId);



           ViewBag.Purchases = _context.Orders
                .Count(o => o.CustomerId == customerId);



            return View();

        }

        //==========================
        // Profile
        //==========================

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            int customerId = HttpContext.Session.GetInt32("CustomerId").Value;

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
                return NotFound();

            return View(customer);
        }
        [HttpPost]
        public IActionResult Profile(Customer customer)
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            // Remove password validation
            ModelState.Remove(nameof(Customer.Password));

            if (!ModelState.IsValid)
                return View(customer);

            var existingCustomer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer == null)
                return NotFound();

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            // Only update password if entered
            if (!string.IsNullOrWhiteSpace(customer.Password))
            {
                existingCustomer.Password = customer.Password;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("CustomerName", existingCustomer.FullName);

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Profile));
        }// ===============================
         // FORGOT PASSWORD
         // ===============================

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


            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == model.Email);



            if (customer == null)
            {

                ViewBag.Error =
                "No account was found with this email.";

                return View(model);

            }



            return RedirectToAction(
                "ResetPassword",
                new { email = model.Email }
            );

        }
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



            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == model.Email);



            if (customer == null)
            {

                ViewBag.Error =
                "Customer account not found.";

                return View(model);

            }



            customer.Password = model.NewPassword;



            _context.SaveChanges();



            TempData["Success"] =
            "Password changed successfully. Please login.";



            return RedirectToAction("Login");

        }
    }
}


