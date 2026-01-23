using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly DbMyShopContext _context;
        static string Level = "";

        public UsersController(DbMyShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index(string? name, int page = 1, int pageSize = 30)
        {
            var query = _context.Users.OrderBy(x => x.Level).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Username.ToLower().Contains(name.ToLower().Trim())).OrderBy(x => x.Level);
            }
            // Tổng số bản ghi sau khi lọc
            var totalCount = await query.CountAsync();

            // Lấy dữ liệu từng trang
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi biến qua View
            ViewData["SearchName"] = name;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return View(data);
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create(string? strLevel)
        {
            if (strLevel != null)
                Level = strLevel;
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            model.Level = Level + model.Level;
            model.Level = Level + "00000";
            Level = "";
            if (ModelState.IsValid)
            {
                model.Password = Cipher.GenerateMD5(model.Password);
                model.Active = 1;
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            Level = user.Level.Substring(0, user.Level.Length - 5);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, string? CurrentPassword,string? NewPassword)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            user.Level = Level + user.Level;
            user    .Level = Level + "00000";
            Level = "";
            if (ModelState.IsValid)
            {
                try
                {
                    // lấy user gốc từ DB
                    var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                    if (dbUser == null) return NotFound();

                    // update các field thường
                    dbUser.Username = user.Username;
                    dbUser.Name = user.Name;
                    dbUser.Admin = user.Admin;
                    dbUser.Active = user.Active;
                    dbUser.Ord = user.Ord;
                    dbUser.Level = Level + "00000";
                    Level = "";

                    // 👉 xử lý mật khẩu
                    if (!string.IsNullOrWhiteSpace(NewPassword))
                    {
                        // chỉ đổi mật khẩu khi có nhập mật khẩu mới
                        dbUser.Password = Cipher.GenerateMD5(NewPassword);
                    }
                    // không nhập NewPassword → giữ nguyên mật khẩu cũ (KHÔNG ĐỘNG GÌ)

                    _context.Update(dbUser);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult CheckPassword(int id, string password)
        {
            var user = _context.Users.Find(id);
            if (user == null) return Json(false);

            bool isValid = Cipher.GenerateMD5(password) == user.Password;
            return Json(isValid);
        }

        // GET: Admin/Users/Delete/5
        public IActionResult Delete(int id)
        {
            if (!int.TryParse(User.FindFirstValue("UserId"), out int currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            // ❌ Không cho xoá chính mình
            if (id == currentUserId)
            {
                TempData["Error"] = "Không thể xoá tài khoản đang đăng nhập.";
                return RedirectToAction("Index");
            }

            var model = _context.Users.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();

            _context.Users.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
