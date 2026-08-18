using ArtGallery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // PURCHASE HISTORY
        // ==========================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            int customerId = HttpContext.Session.GetInt32("CustomerId").Value;

            var orders = _context.Orders

                .Include(o => o.OrderItems)

                .ThenInclude(oi => oi.ArtWork)

                .Where(o => o.CustomerId == customerId)

                .OrderByDescending(o => o.OrderDate)

                .ToList();

            return View(orders);
        }
        // ==========================
        // ADMIN - VIEW ALL ORDERS
        // ==========================
        public IActionResult AdminIndex()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var orders = _context.Orders

                .Include(o => o.Customer)

                .Include(o => o.OrderItems)

                .ThenInclude(oi => oi.ArtWork)

                .OrderByDescending(o => o.OrderDate)

                .ToList();

            return View(orders);
        }
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var order = _context.Orders

                .Include(o => o.Customer)

                .Include(o => o.OrderItems)

                .ThenInclude(oi => oi.ArtWork)

                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
