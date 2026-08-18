using ArtGallery.Data;
using ArtGallery.Models;
using ArtGallery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================================
        // CUSTOMER - CONTACT FORM
        // ===========================================

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            return View();
        }

        // ===========================================
        // SEND MESSAGE
        // ===========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            string subject = Request.Form["Subject"];
            string messageText = Request.Form["Message"];

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(messageText))
            {
                ModelState.AddModelError("", "Please fill in all fields.");

                return View("Index");
            }

            ContactMessage message = new ContactMessage
            {
                CustomerId = HttpContext.Session.GetInt32("CustomerId").Value,
                Subject = subject,
                Message = messageText,
                DateSent = DateTime.Now
            };

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            TempData["Success"] = "Your message has been sent successfully.";

            return RedirectToAction(nameof(Index));
        }
        // ===========================================
        // ADMIN - VIEW ALL MESSAGES
        // ===========================================

        public IActionResult AdminIndex()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var messages = _context.ContactMessages
                .Include(c => c.Customer)
                .OrderByDescending(c => c.DateSent)
                .ToList();

            return View(messages);
        }

        // ===========================================
        // VIEW MESSAGE
        // ===========================================

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var message = _context.ContactMessages
                .Include(c => c.Customer)
                .FirstOrDefault(c => c.ContactMessageId == id);

            if (message == null)
                return NotFound();

            return View(message);
        }

        // ===========================================
        // DELETE MESSAGE
        // ===========================================

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var message = _context.ContactMessages.Find(id);

            if (message == null)
                return NotFound();

            _context.ContactMessages.Remove(message);
            _context.SaveChanges();

            TempData["Success"] = "Message deleted successfully.";

            return RedirectToAction(nameof(AdminIndex));
        }
        public IActionResult Reply(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            var message = _context.ContactMessages
                .Include(c => c.Customer)
                .FirstOrDefault(c => c.ContactMessageId == id);

            if (message == null)
                return NotFound();

            var model = new ReplyMessageViewModel
            {
                ContactMessageId = message.ContactMessageId,
                CustomerName = message.Customer?.FullName,
                Subject = message.Subject,
                Message = message.Message,
                AdminReply = message.AdminReply
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reply()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
                return RedirectToAction("Login", "Admin");

            int contactMessageId = Convert.ToInt32(Request.Form["ContactMessageId"]);
            string adminReply = Request.Form["AdminReply"];

            if (string.IsNullOrWhiteSpace(adminReply))
            {
                TempData["Error"] = "Please enter a reply.";

                return RedirectToAction(nameof(Reply), new { id = contactMessageId });
            }

            var message = _context.ContactMessages
                .FirstOrDefault(c => c.ContactMessageId == contactMessageId);

            if (message == null)
                return NotFound();

            message.AdminReply = adminReply;
            message.ReplyDate = DateTime.Now;
            message.IsReplied = true;

            _context.SaveChanges();

            TempData["Success"] = "Reply sent successfully.";

            return RedirectToAction(nameof(AdminIndex));
        }
        // ===========================================
        // CUSTOMER - MY MESSAGES
        // ===========================================

        public IActionResult MyMessages()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");

            int customerId = HttpContext.Session.GetInt32("CustomerId").Value;

            var messages = _context.ContactMessages
                .Where(c => c.CustomerId == customerId)
                .OrderByDescending(c => c.DateSent)
                .ToList();

            return View(messages);
        }
    }
}
