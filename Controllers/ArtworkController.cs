using ArtGallery.Data;
using ArtGallery.Models;
using ArtGallery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ArtworkController(ApplicationDbContext context,
                                 IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        // ==========================
        // CHECK LOGIN
        // ==========================

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("AdminId") != null;
        }



        // ==========================
        // INDEX
        // ==========================

        public IActionResult Index(string searchString)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            var artworks = _context.ArtWorks
                .Include(a => a.Category)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchString))
            {
                artworks = artworks.Where(a =>
                    a.Title.Contains(searchString) ||
                    a.ArtistName.Contains(searchString));
            }


            return View(artworks.ToList());
        }



        // ==========================
        // CREATE GET
        // ==========================

        public IActionResult Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            LoadCategories();

            return View();
        }



        // ==========================
        // CREATE POST
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArtworkViewModel model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            if (ModelState.IsValid)
            {

                string fileName = UploadImage(model.ImageFile);


                ArtWork artwork = new ArtWork
                {
                    Title = model.Title,

                    ArtistName = model.ArtistName,

                    Price = model.Price,

                    Description = model.Description,

                    Status = model.Status,

                    CategoryId = model.CategoryId,

                    ImageURL = "/images/" + fileName
                };


                _context.ArtWorks.Add(artwork);

                _context.SaveChanges();


                return RedirectToAction(nameof(Index));
            }


            LoadCategories(model.CategoryId);

            return View(model);
        }




        // ==========================
        // DETAILS
        // ==========================

        public IActionResult Details(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            var artwork = _context.ArtWorks
                .Include(a => a.Category)
                .FirstOrDefault(a => a.ArtId == id);


            if (artwork == null)
                return NotFound();


            return View(artwork);
        }




        // ==========================
        // EDIT GET
        // ==========================

        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            var artwork = _context.ArtWorks.Find(id);


            if (artwork == null)
                return NotFound();


            ArtworkViewModel model = new ArtworkViewModel
            {
                Title = artwork.Title,

                ArtistName = artwork.ArtistName,

                Price = artwork.Price,

                Description = artwork.Description,

                Status = artwork.Status,

                CategoryId = artwork.CategoryId
            };


            ViewBag.ArtId = artwork.ArtId;

            ViewBag.OldImage = artwork.ImageURL;


            LoadCategories(artwork.CategoryId);


            return View(model);
        }





        // ==========================
        // EDIT POST
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ArtworkViewModel model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            var artwork = _context.ArtWorks.Find(id);


            if (artwork == null)
                return NotFound();



            if (ModelState.IsValid)
            {

                artwork.Title = model.Title;

                artwork.ArtistName = model.ArtistName;

                artwork.Price = model.Price;

                artwork.Description = model.Description;

                artwork.Status = model.Status;

                artwork.CategoryId = model.CategoryId;



                // Replace image only if new one selected

                if (model.ImageFile != null)
                {
                    string fileName = UploadImage(model.ImageFile);

                    artwork.ImageURL = "/images/" + fileName;
                }



                _context.ArtWorks.Update(artwork);

                _context.SaveChanges();


                return RedirectToAction(nameof(Index));

            }


            LoadCategories(model.CategoryId);

            ViewBag.ArtId = id;

            ViewBag.OldImage = artwork.ImageURL;


            return View(model);
        }





        // ==========================
        // DELETE GET
        // ==========================

        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Admin");


            var artwork = _context.ArtWorks
                .Include(a => a.Category)
                .FirstOrDefault(a => a.ArtId == id);


            if (artwork == null)
                return NotFound();


            return View(artwork);
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


            var artwork = _context.ArtWorks.Find(id);


            if (artwork != null)
            {
                _context.ArtWorks.Remove(artwork);

                _context.SaveChanges();
            }


            return RedirectToAction(nameof(Index));

        }





        // ==========================
        // LOAD CATEGORY DROPDOWN
        // ==========================

        private void LoadCategories(int selected = 0)
        {
            ViewBag.CategoryId =
                new SelectList(
                    _context.Categories,
                    "CategoryId",
                    "CategoryName",
                    selected
                );
        }





        // ==========================
        // IMAGE UPLOAD METHOD
        // ==========================

        private string UploadImage(IFormFile image)
        {

            string uploadsFolder =
                Path.Combine(
                    _environment.WebRootPath,
                    "images"
                );


            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            string fileName =
                Guid.NewGuid().ToString()
                +
                Path.GetExtension(image.FileName);



            string filePath =
                Path.Combine(
                    uploadsFolder,
                    fileName
                );



            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }



            return fileName;
        }

    }
}
