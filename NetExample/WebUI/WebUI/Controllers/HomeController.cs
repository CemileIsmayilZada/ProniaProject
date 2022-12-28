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

            HttpContext.Session.SetString("name", "Metin");
            Response.Cookies.Append("surname", "Iskenderov");
            HomeVM homeVM = new()
            {
                SlideItem = _context.SlideItems,
                ShippingItem = _context.ShippingItems
            };
            return View(homeVM);
        }

        public IActionResult Test() 
        {
            var s=HttpContext.Session.GetString("name");
            var c = Request.Cookies["surname"];
            return Json(s + " " + c);
        }

    }
}
