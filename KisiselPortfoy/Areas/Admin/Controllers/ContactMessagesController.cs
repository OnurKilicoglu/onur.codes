using KisiselPortfoy.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KisiselPortfoy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactMessagesController : Controller
    {
        private readonly PortfolioDbContext _context;

        public ContactMessagesController(PortfolioDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.ContactMessages
                                   .OrderByDescending(x => x.SentDate)
                                   .ToList();
            return View(messages);
        }

        public IActionResult Details(int id)
        {
            var msg = _context.ContactMessages.Find(id);
            if (msg == null)
                return NotFound();
            return View(msg);
        }

        public IActionResult Delete(int id)
        {
            var msg = _context.ContactMessages.Find(id);
            if (msg == null)
                return NotFound();
            return View(msg);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var msg = _context.ContactMessages.Find(id);
            if (msg != null)
            {
                _context.ContactMessages.Remove(msg);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
