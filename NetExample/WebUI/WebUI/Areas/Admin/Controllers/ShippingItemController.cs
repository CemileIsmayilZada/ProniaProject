using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingItemController : Controller
    {
        private AppDbContext _context;

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
            return View(model);
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(_context.ShippingItems);
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
            return View(_context.ShippingItems);
        }
    }
}
