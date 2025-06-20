using KisiselPortfoy.Core.Entities;
using KisiselPortfoy.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KisiselPortfoy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectsController : Controller
    {
        private readonly PortfolioDbContext _context;

        public ProjectsController(PortfolioDbContext context)
        {
            _context = context;
        }

        private bool IsAdminLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser"));
        }

        public IActionResult Index()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            var projects = _context.Projects.OrderByDescending(x => x.CreateDate).ToList();
            return View(projects);
        }

        public IActionResult Create()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project,IFormFile ImageUrl, IFormFile codeFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("CodeFileUrl");
            // 1. Resim yüklemesi
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var imgExt = Path.GetExtension(ImageUrl.FileName).ToLower();
                if (imgExt != ".jpg" && imgExt != ".jpeg" && imgExt != ".png")
                {
                    ModelState.AddModelError("", "Sadece .jpg, .jpeg, .png dosya yükleyebilirsiniz.");
                    return View(project);
                }
                var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(imgFolder))
                    Directory.CreateDirectory(imgFolder);

                var imgFileName = $"{Guid.NewGuid()}{imgExt}";
                var imgPath = Path.Combine(imgFolder, imgFileName);

                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                // DB'de ImageUrl alanına kaydediyoruz:
                project.ImageUrl = $"/images/{imgFileName}";
            }
            else
            {
                project.ImageUrl = null; // Zorunlu ise hata ver, opsiyonel ise böyle bırak.
            }
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser")))
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            if (ModelState.IsValid)
            {
                // Dosya yüklendiyse
                if (codeFile != null && codeFile.Length > 0)
                {
                    // Sadece .rar ve .zip dosyası kabul et
                    var extension = Path.GetExtension(codeFile.FileName).ToLower();
                    if (extension != ".rar" && extension != ".zip")
                    {
                        ModelState.AddModelError("", "Sadece .rar veya .zip dosyası yükleyebilirsiniz.");
                        return View(project);
                    }

                    // Kayıt dizinini belirle
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Benzersiz dosya adı oluştur
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        codeFile.CopyTo(stream);
                    }

                    // DB'ye kaydedilecek dosya yolu
                    project.CodeFileUrl = $"/uploads/{uniqueFileName}";
                }
                else
                {
                    ModelState.AddModelError("", "RAR veya ZIP dosyası seçmeniz gerekiyor.");
                    return View(project);
                }

                project.CreateDate = DateTime.Now;
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }


        public IActionResult Edit(int id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Project project, IFormFile imageFile, IFormFile codeFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("CodeFileUrl");
            if (ModelState.IsValid)
            {
                var existingProject = _context.Projects.FirstOrDefault(x => x.Id == project.Id);
                if (existingProject == null) return NotFound();

                existingProject.Title = project.Title;
                existingProject.Description = project.Description;
                // 1. Yeni görsel yüklendiyse değiştir
                if (imageFile != null && imageFile.Length > 0)
                {
                    var ext = Path.GetExtension(imageFile.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    {
                        var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);
                        var uniqueName = $"{Guid.NewGuid()}{ext}";
                        var imgPath = Path.Combine(imgFolder, uniqueName);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }
                        existingProject.ImageUrl = $"/images/{uniqueName}";
                    }
                }
                // 2. Yeni kod dosyası yüklendiyse değiştir
                if (codeFile != null && codeFile.Length > 0)
                {
                    var ext = Path.GetExtension(codeFile.FileName).ToLower();
                    if (ext == ".rar" || ext == ".zip")
                    {
                        var codeFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(codeFolder)) Directory.CreateDirectory(codeFolder);
                        var codeFileName = $"{Guid.NewGuid()}{ext}";
                        var codePath = Path.Combine(codeFolder, codeFileName);
                        using (var stream = new FileStream(codePath, FileMode.Create))
                        {
                            codeFile.CopyTo(stream);
                        }
                        existingProject.CodeFileUrl = $"/uploads/{codeFileName}";
                    }
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }


        public IActionResult Delete(int id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Admin", new { area = "Admin" });

            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();
            return View(project);
        }
    }
}
