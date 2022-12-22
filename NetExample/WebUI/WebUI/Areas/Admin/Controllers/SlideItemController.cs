using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideItemController : Controller
    {

        private readonly AppDbContext _context;
        public SlideItemController(AppDbContext context)
        {
            _context=context;   
        }
        public IActionResult Index()
        {
            return View(_context.SlideItems);
        }
        public IActionResult Detail(int id)
        {
            var model=_context.SlideItems.Find(id);
            if(model == null) return NotFound();    
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SlideItem slideItem)
        {
           if(!ModelState.IsValid) return View(slideItem);
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
