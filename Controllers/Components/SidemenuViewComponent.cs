using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers.Components
{
    public class SidemenuViewComponent : ViewComponent
    {
        private readonly DbMyShopContext _context;
        public SidemenuViewComponent(DbMyShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Default");
        }
    }
}
