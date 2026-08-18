using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class FavouriteController : Controller
    {

        private readonly ApplicationDbContext _context;


        public FavouriteController(ApplicationDbContext context)
        {
            _context = context;
        }




        // ==========================
        // VIEW FAVOURITES
        // ==========================

        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");



            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;




            var favourites = _context.Favourites

                .Include(f => f.ArtWork)

                .ThenInclude(a => a.Category)

                .Where(f => f.CustomerId == customerId)

                .Where(f => f.ArtWork != null)

                .AsNoTracking()

                .ToList();




            return View(favourites);

        }







        // ==========================
        // ADD FAVOURITE
        // ==========================

        public IActionResult Add(int id)
        {

            if (HttpContext.Session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Customer");



            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;




            var artworkExists =
                _context.ArtWorks.Any(a => a.ArtId == id);



            if (!artworkExists)
            {
                return NotFound();
            }






            bool alreadyExists =
                _context.Favourites.Any(f =>
                    f.CustomerId == customerId &&
                    f.ArtId == id);





            if (!alreadyExists)
            {

                Favourite favourite = new Favourite
                {
                    CustomerId = customerId,
                    ArtId = id
                };



                _context.Favourites.Add(favourite);

                _context.SaveChanges();

            }




            // Go directly to favourites page

            return RedirectToAction(nameof(Index));

        }







        // ==========================
        // REMOVE FAVOURITE
        // ==========================

        public IActionResult Remove(int id)
        {


            if (HttpContext.Session.GetInt32("CustomerId") == null)

                return RedirectToAction("Login", "Customer");




            int customerId =
                HttpContext.Session.GetInt32("CustomerId").Value;





            var favourite =
                _context.Favourites
                .FirstOrDefault(f =>
                    f.FavouriteId == id &&
                    f.CustomerId == customerId);





            if (favourite != null)
            {

                _context.Favourites.Remove(favourite);

                _context.SaveChanges();

            }





            return RedirectToAction(nameof(Index));

        }


    }
}
