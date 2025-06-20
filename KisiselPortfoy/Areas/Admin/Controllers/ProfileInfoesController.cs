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
    [Area("Admin")]
    public class ProfileInfoesController : Controller
    {
        private readonly PortfolioDbContext _context;

        public ProfileInfoesController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProfileInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProfileInfos.ToListAsync());
        }

        // GET: Admin/ProfileInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileInfo = await _context.ProfileInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileInfo == null)
            {
                return NotFound();
            }

            return View(profileInfo);
        }

        // GET: Admin/ProfileInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProfileInfoes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Title,Description,Skills,Experience,Goals,Email,Instagram,PhotoUrl")] ProfileInfo profileInfo, IFormFile photoFile)
        {
            ModelState.Remove("PhotoUrl"); // Id'yi modelden kaldır, çünkü yeni kayıt için gerekli değil
            ModelState.Remove("Email"); // Id'yi modelden kaldır, çünkü yeni kayıt için gerekli değil
            if (ModelState.IsValid)
            {
                // Eğer görsel yüklendiyse işleme al
                if (photoFile != null && photoFile.Length > 0)
                {
                    var ext = Path.GetExtension(photoFile.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    {
                        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile");
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        var uniqueName = $"profile_{Guid.NewGuid()}{ext}";
                        var filePath = Path.Combine(folder, uniqueName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(stream);
                        }
                        profileInfo.PhotoUrl = $"/images/profile/{uniqueName}";
                    }
                }

                _context.Add(profileInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileInfo);
        }


        // GET: Admin/ProfileInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileInfo = await _context.ProfileInfos.FindAsync(id);
            if (profileInfo == null)
            {
                return NotFound();
            }
            return View(profileInfo);
        }

        // POST: Admin/ProfileInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Title,Description,Skills,Experience,Goals,Email,Instagram,PhotoUrl")] ProfileInfo profileInfo)
        {
            if (id != profileInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileInfoExists(profileInfo.Id))
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
            return View(profileInfo);
        }

        // GET: Admin/ProfileInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileInfo = await _context.ProfileInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileInfo == null)
            {
                return NotFound();
            }

            return View(profileInfo);
        }

        // POST: Admin/ProfileInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profileInfo = await _context.ProfileInfos.FindAsync(id);
            if (profileInfo != null)
            {
                _context.ProfileInfos.Remove(profileInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileInfoExists(int id)
        {
            return _context.ProfileInfos.Any(e => e.Id == id);
        }
    }
}
