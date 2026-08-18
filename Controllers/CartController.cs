using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _context;


        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }





        // View Cart

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");


            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;


            var cartItems = _context.CartItems
                .Include(c => c.ArtWork)
                .ThenInclude(a => a.Category)
                .Where(c => c.CustomerId == customerId)
                .ToList();


            return View(cartItems);
        }






        // Add Artwork

        public IActionResult Add(int id)
        {

            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");



            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;




            bool exists =
                _context.CartItems.Any(c =>
                    c.CustomerId == customerId &&
                    c.ArtId == id);



            if (!exists)
            {

                CartItem item = new CartItem
                {
                    CustomerId = customerId,
                    ArtId = id
                };


                _context.CartItems.Add(item);

                _context.SaveChanges();

            }



            return RedirectToAction(nameof(Index));

        }






        // Remove item

        public IActionResult Remove(int id)
        {

            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");



            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;



            var item =
                _context.CartItems
                .FirstOrDefault(c =>
                    c.CartItemId == id &&
                    c.CustomerId == customerId);



            if (item != null)
            {

                _context.CartItems.Remove(item);

                _context.SaveChanges();

            }



            return RedirectToAction(nameof(Index));

        }


    }
}