using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Interfaces;
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
        private ISliderItemRepository _repository;
        
        public readonly IWebHostEnvironment _env;
        private int _count;
        public SlideItemController(ISliderItemRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            
            _env = env;
         //   _count = _repository.Count();
        }
        public IActionResult Index()
        {
            ViewBag.Count = _count;
            return View(_repository);
        }
        public IActionResult Detail(int id)
        {
            var slide = _repository.GetAsync(id);
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


            await _repository.CreateAsync(slideItem);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            var model =  await _repository.GetAsync(id);
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
            var model = await _repository.GetAsync(id);
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
           

            _repository.Update(model);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            if (_count == 1) return BadRequest();
            var slide = _repository.GetAsync(id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_count == 1) return BadRequest();
            var slide = await _repository.GetAsync(id);
            if (slide == null) return NotFound();

            //folderden silme

            Helper.DeleteFile(_env.WebRootPath, "assets", "images", "website-images", slide.Photo);

            //database-den silme
            _repository.Delete(slide);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
