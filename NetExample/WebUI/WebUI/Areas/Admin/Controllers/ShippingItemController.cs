using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingItemController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingItemController(AppDbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            return View(_context.ShippingItems);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _context.ShippingItems.FindAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
        public async Task<IActionResult> Update(int id)
        {
            var model=await _context.ShippingItems.FindAsync(id);
            if(model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,ShippingItem item)
        {
            if (id != item.Id) return BadRequest();
            if(!ModelState.IsValid) return View(item);
            var model = await _context.ShippingItems.FindAsync(id);
            if (model == null) return NotFound();
            model.Title = item.Title;
            model.Description = item.Description;
            model.Image=item.Image;
            await _context.SaveChangesAsync();
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
            await _context.ShippingItems.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _context.ShippingItems.FindAsync(id);
            if (model == null) return NotFound();
            return View(model) ;
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var model = await _context.ShippingItems.FindAsync(id);
            if (model == null) return NotFound();
            _context.ShippingItems.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
