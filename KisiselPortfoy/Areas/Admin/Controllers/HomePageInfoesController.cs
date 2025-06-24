using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisiselPortfoy.Core.Entities;
using KisiselPortfoy.DataAccess.Context;

namespace KisiselPortfoy.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class HomePageInfoesController : Controller
    {
        private readonly PortfolioDbContext _context;

        public HomePageInfoesController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: Admin/HomePageInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomePageInfos.ToListAsync());
        }

        // GET: Admin/HomePageInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homePageInfo = await _context.HomePageInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePageInfo == null)
            {
                return NotFound();
            }

            return View(homePageInfo);
        }

        // GET: Admin/HomePageInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/HomePageInfoes/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( HomePageInfo homePageInfo, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var ext = Path.GetExtension(imageFile.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    {
                        var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);
                        var uniqueName = $"hero_{Guid.NewGuid()}{ext}";
                        var imgPath = Path.Combine(imgFolder, uniqueName);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }
                        homePageInfo.ImageUrl = $"/images/{uniqueName}";
                    }
                }
                _context.Add(homePageInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homePageInfo);
        }

        // GET: Admin/HomePageInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homePageInfo = await _context.HomePageInfos.FindAsync(id);
            if (homePageInfo == null)
            {
                return NotFound();
            }
            return View(homePageInfo);
        }

        // POST: Admin/HomePageInfoes/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WelcomeTitle,WelcomeText,ButtonText,ButtonUrl,ImageUrl")] HomePageInfo homePageInfo)
        {
            if (id != homePageInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homePageInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageInfoExists(homePageInfo.Id))
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
            return View(homePageInfo);
        }

        // GET: Admin/HomePageInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homePageInfo = await _context.HomePageInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePageInfo == null)
            {
                return NotFound();
            }

            return View(homePageInfo);
        }

        // POST: Admin/HomePageInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homePageInfo = await _context.HomePageInfos.FindAsync(id);
            if (homePageInfo != null)
            {
                _context.HomePageInfos.Remove(homePageInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomePageInfoExists(int id)
        {
            return _context.HomePageInfos.Any(e => e.Id == id);
        }
    }
}
