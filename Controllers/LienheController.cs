using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class LienheController : Controller
    {
        private readonly DbMyShopContext _context;
        public LienheController(DbMyShopContext context)
        {
            _context = context;
        }
        [Route("lien-he")]
        [HttpGet]
        public IActionResult Index()
        {
            var config = _context.Configs.FirstOrDefault() ?? new Config();

            ViewBag.Config = config;    
            return View();
        }

        [HttpPost("/lien-he")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contact model)
        {
            // validate thủ công
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                TempData["Error"] = "Vui lòng nhập họ và tên";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Tel))
            {
                TempData["Error"] = "Vui lòng nhập số điện thoại";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Mail))
            {
                TempData["Error"] = "Vui lòng nhập email";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Detail))
            {
                TempData["Error"] = "Vui lòng nhập nội dung liên hệ";
                return RedirectToAction("Index");
            }

            // lưu DB
            model.Date = DateTime.Now;
            model.Active = 1;
            model.Lang = "vi";
            model.Code = Guid.NewGuid().ToString("N");

            _context.Contacts.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Gửi liên hệ thành công!";
            return RedirectToAction("Index");
        }

    }
}
