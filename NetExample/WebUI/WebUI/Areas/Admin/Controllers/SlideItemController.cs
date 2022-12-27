using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.IO;
using System.Linq;
using System.Text;
using WebUI.Utilities;
using WebUI.ViewModels.Slider;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideItemController : Controller
    {

        private readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;
        private int _count;
        public SlideItemController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _count = _context.SlideItems.Count();
        }
        public IActionResult Index()
        {
            ViewBag.Count = _count;
            return View(_context.SlideItems);
        }
        public IActionResult Detail(int id)
        {
            var slide = _context.SlideItems.Find(id);
            if (slide == null) return NotFound();
            return View(slide);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SlideCreateVM slide)
        {

            if (!ModelState.IsValid) return View(slide);
            if (slide.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo is null");
                return View(slide);
            }
            if (!slide.Photo.CheckFileSize(100))
            {
                ModelState.AddModelError("Photo", "Photo lenght is more than limits (100)");
                return View(slide);
            }
            if (!slide.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "Content type must be Image!");
                return View(slide);
            }



            var fileName = string.Empty;
            try
            {
                fileName = await slide.Photo.CopyFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            }
            catch (Exception)
            {
                return View(slide);
            }

            SlideItem slideItem = new SlideItem()
            {
                Title = slide.Title,
                Description = slide.Description,
                Offer = slide.Offer,
                Photo = fileName

            };


            await _context.SlideItems.AddAsync(slideItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            var model = _context.SlideItems.Find(id);
            if (model == null) return View(model);

            SlideUpdateVM slider = new()
            {

                Title = model.Title,
                Description = model.Description,
                Offer = model.Offer,
                ImagePath = model.Photo
            };
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SlideUpdateVM slide)
        {
            if (id != slide.Id) return BadRequest();
            if (!ModelState.IsValid) return View(slide);
            var model = _context.SlideItems.Find(id);
            if (model == null) return View(model);

            //SlideItem slideItem = new();
            model.Title = slide.Title;
            model.Description = slide.Description;
            model.Offer = slide.Offer;
            if (slide.Photo != null)
            {
                Helper.DeleteFile(_env.WebRootPath, "assets", "images", "website-images", model.Photo);
                if (!slide.Photo.CheckFileSize(100))
                {
                    ModelState.AddModelError("Photo", "Photo lenght is more than limits (100)");
                    return View(slide);
                }
                if (!slide.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "Content type must be Image!");
                    return View(slide);
                }

                var fileName = string.Empty;
                try
                {
                    fileName = await slide.Photo.CopyFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                }
                catch (Exception)
                {
                    return View(slide);
                }
                model.Photo = fileName;

            }
           

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            if (_count == 1) return BadRequest();
            var slide = _context.SlideItems.Find(id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_count == 1) return BadRequest();
            var slide = _context.SlideItems.Find(id);
            if (slide == null) return NotFound();

            //folderden silme

            Helper.DeleteFile(_env.WebRootPath, "assets", "images", "website-images", slide.Photo);

            //database-den silme
            _context.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
