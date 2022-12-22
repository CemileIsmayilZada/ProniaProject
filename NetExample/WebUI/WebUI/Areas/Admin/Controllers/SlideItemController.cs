using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebUI.Utilities;
using WebUI.ViewModels.Slider;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideItemController : Controller
    {

        private readonly AppDbContext _context;
        public readonly  IWebHostEnvironment _env;
        public SlideItemController(AppDbContext context, IWebHostEnvironment env)
        {
            _context=context;   
            _env=env;
        }
        public IActionResult Index()
        {
            return View(_context.SlideItems);
        }
        public IActionResult Detail(int id)
        {
            var slide=_context.SlideItems.Find(id);
            if(slide == null) return NotFound();    
            return View(slide);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideCreateVM slide)
        {
            
           if(!ModelState.IsValid) return View(slide);
            if (slide.Photo == null) 
            {
                ModelState.AddModelError("Photo","Photo is null");
                return View(slide);
            }
            if (!slide.Photo.CheckFileSize(1))
            {
                ModelState.AddModelError("Photo", "Photo lenght is more than limits");
                return View(slide);
            }
            if (!slide.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "Content type must be Image!");
                return View(slide);
            }
            return Content("ok");


            var fileName = string.Empty;
            try
            {
                fileName = await slide.Photo.CopyFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            }
            catch (Exception)
            {
                return View(slide);
            }
            await slide.Photo.CopyFileAsync(_env.WebRootPath, "assets", "images", "website-images");
          
            SlideItem slideItem = new()
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
            return View(_context.ShippingItems);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(_context.ShippingItems);
        }
    }
}
