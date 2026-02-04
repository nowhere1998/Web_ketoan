using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Controllers
{
	public class TintucController : Controller
	{
        private DbMyShopContext _context;
        public TintucController(DbMyShopContext context)
        {
            _context = context;
        }
        [Route("tin-tuc")]
		public IActionResult Index(int page = 1)
        {
            int pageSize = 12;

            var query = _context.News
                .Where(x => x.Active == 1)
                .OrderByDescending(x => x.Id);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var news = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(news);
        }

        [Route("tin-tuc-chi-tiet/{slug}")]
        public IActionResult Chitiet(string slug= "")
        {
            return View("tin-tuc-chi-tiet");
        }
    }
}
