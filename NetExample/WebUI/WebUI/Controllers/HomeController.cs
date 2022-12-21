using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        //Test
        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                SlideItem = _context.SlideItems,
                ShippingItem = _context.ShippingItems
            };
            return View(homeVM);
        }

    }
}
