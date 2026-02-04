using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index(string slug = "", int page = 1)
        {
            int pageSize = 9; // 3 cột x 3 dòng
            if (page < 1) page = 1;

            var query = _context.Libraries
                .Include(x => x.GroupLibrary)
                .Where(x => x.Active == 1);

            if (slug == "videos")
            {
                query = query.Where(x => x.GroupLibrary.Name.ToLower() == "videos");
            }
            else
            {
                query = query.Where(x => x.GroupLibrary.Name.ToLower() == "hình ảnh");
            }

            int totalItems = query.Count();

            var libraries = query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Slug = slug;

            return View(libraries);
        }

    }
}
