using Microsoft.AspNetCore.Mvc;
using KisiselPortfoy.DataAccess.Context;

namespace KisiselPortfoy.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly PortfolioDbContext _context;
        public AdminController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Admins
                .FirstOrDefault(x => x.UserName == username && x.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminUser", username);
                return RedirectToAction("Dashboard", "Admin", new { area = "Admin" });
            }

            ViewBag.Error = "Kullanıcı adı veya şifre yanlış!";
            return View();
        }

        public IActionResult DashBoard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            ViewBag.TotalProjects = _context.Projects.Count();
            ViewBag.TotalFiles = _context.Projects.Count(x => !string.IsNullOrEmpty(x.CodeFileUrl));

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUser");
            return RedirectToAction("Login");
        }
    }
}
