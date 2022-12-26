using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingItemController : Controller
    {
        private readonly IShippingItemRepository _repository;

        public ShippingItemController(IShippingItemRepository repository)
        {
            _repository = repository;
        }

        public async Task< IActionResult> Index()
        {
            return View( await _repository.GetAllAsync());
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _repository.GetAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var model = await _repository.GetAsync(id);
            if(model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,ShippingItem item)
        {
            if (id != item.Id) return BadRequest();
            if(!ModelState.IsValid) return View(item);
            var model = await _repository.GetAsync(id);
            if (model == null) return NotFound();
            model.Title = item.Title;
            model.Description = item.Description;
            model.Image=item.Image;
            _repository.Update(model);
            await _repository.GetAllAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingItem shipping)
        {
            if (!ModelState.IsValid) return View(shipping);
            await _repository.CreateAsync(shipping);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _repository.GetAsync(id);
            if (model == null) return NotFound();
            return View(model) ;
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var model = await _repository.GetAsync(id);
            if (model == null) return NotFound();
            _repository.Delete(model);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
