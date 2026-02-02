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
            var pagesL1 = _context.Pages
                .Where(l1 =>
                    l1.Level != null &&
                    l1.Level.Length == 5 &&
                    l1.Active == 1 &&
                    (l1.Position == 2 || l1.Position == 4 || l1.Position == 5))
                .OrderBy(l1 => l1.Ord)
                .Select(l1 => new PageL1
                {
                    Page = l1,

                    // Có cấp 2
                    HasChild = _context.Pages.Any(l2 =>
                        l2.Level != null &&
                        l2.Level.Length == 10 &&
                        l2.Level.StartsWith(l1.Level) &&
                        l2.Active == 1 &&
                        (l2.Position == 2 || l2.Position == 4 || l2.Position == 5)
                    ),

                })
                .ToList();

            var pagesL2 = _context.Pages
                .OrderBy(x => x.Ord)
                .Where(x => x.Level != null
                    && x.Level.Length == 10
                    && x.Active == 1
                    && (x.Position == 2 || x.Position == 4 || x.Position == 5))
                .ToList();

            var config = _context.Configs.FirstOrDefault() ?? new Config();
            var logo = _context.Advertises
                .Where(x => x.Position == 6 && x.Active)
                .OrderBy(x => x.Ord)
                .FirstOrDefault() ?? new Advertise();

            ViewBag.PagesL1 = pagesL1;
            ViewBag.PagesL2 = pagesL2;
            ViewBag.Config = config;
            ViewBag.Logo = logo;
            return View("Default");
        }
    }
}
