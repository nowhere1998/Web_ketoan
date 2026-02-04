using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class ThuvienController : Controller
    {
        private readonly DbMyShopContext _context;
        public ThuvienController(DbMyShopContext context)
        {
            _context = context;
        }

        [Route("thu-vien")]
        [Route("thu-vien/{slug}")]
        public IActionResult Index(string slug = "")
        {
            return View();
        }
    }
}
