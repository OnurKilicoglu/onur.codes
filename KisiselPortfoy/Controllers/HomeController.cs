using System.Diagnostics;
using KisiselPortfoy.Core.Entities;
using KisiselPortfoy.DataAccess.Context;
using KisiselPortfoy.Models;
using Microsoft.AspNetCore.Mvc;

namespace KisiselPortfoy.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortfolioDbContext _context;


        public HomeController(PortfolioDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var info = _context.HomePageInfos.FirstOrDefault();
            // Son projeleri de çekebilirsin (önceki kodun gibi)
            ViewBag.Hero = info;
            // Örnek: Son 3 projeyi ana sayfada göster
            var lastProjects = _context.Projects
                .OrderByDescending(x => x.CreateDate)
                .Take(3)
                .ToList();

            ViewBag.Projects = lastProjects;
            return View();
        }

        public IActionResult Projects()
        {
            var projects = _context.Projects
                .OrderByDescending(x => x.CreateDate)
                .ToList();
            return View(projects);
        }

        public IActionResult ProjectDetails(int id)
        {
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
                return NotFound();

            return View(project);
        }
        public IActionResult About()
        {
            // Sadece ilk kayýt alýnýr (tek satýrlýk yapý için)
            var profile = _context.ProfileInfos.FirstOrDefault();
            return View(profile);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(string Name, string Email, string Message)
        {
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Message))
            {
                var contactMsg = new ContactMessage
                {
                    Name = Name,
                    Email = Email,
                    Message = Message,
                    SentDate = DateTime.Now
                };
                _context.ContactMessages.Add(contactMsg);
                _context.SaveChanges();
                ViewBag.Success = true;
                return View();
            }
            ViewBag.Success = false;
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
