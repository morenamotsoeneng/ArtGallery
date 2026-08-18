using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check Admin Login
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("AdminId") != null;
        }

        // ==========================
        // INDEX
        // ==========================
        public IActionResult Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            var categories = _context.Categories.ToList();

            return View(categories);
        }

        // ==========================
        // DETAILS
        // ==========================
        public IActionResult Details(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ==========================
        // CREATE GET
        // ==========================
        public IActionResult Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            return View();
        }

        // ==========================
        // CREATE POST
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // ==========================
        // EDIT GET
        // ==========================
        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ==========================
        // EDIT POST
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // ==========================
        // DELETE GET
        // ==========================
        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ==========================
        // DELETE POST
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");

            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
