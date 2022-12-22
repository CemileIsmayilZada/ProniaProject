using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            return View(_context.SlideItems);
        }
    }
}
