using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // CHECKOUT PAGE
        // ==========================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            int customerId = HttpContext.Session.GetInt32("CustomerId").Value;

            var cart = _context.CartItems
                .Include(c => c.ArtWork)
                .ThenInclude(a => a.Category)
                .Where(c => c.CustomerId == customerId)
                .ToList();

            return View(cart);
        }

        // ==========================
        // COMPLETE PURCHASE
        // ==========================
        [HttpPost]
        public IActionResult CompletePurchase()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            int customerId = HttpContext.Session.GetInt32("CustomerId").Value;

            var cartItems = _context.CartItems
                .Include(c => c.ArtWork)
                .Where(c => c.CustomerId == customerId)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            decimal total = cartItems.Sum(c => c.ArtWork!.Price);

            Order order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Completed"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ArtId = item.ArtId,
                    Price = item.ArtWork!.Price
                };

                _context.OrderItems.Add(orderItem);

                // Mark artwork as sold
                item.ArtWork.Status = false;
            }

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();

            return RedirectToAction("Success");
        }

        // ==========================
        // SUCCESS PAGE
        // ==========================
        public IActionResult Success()
        {
            return View();
        }
    }
}
