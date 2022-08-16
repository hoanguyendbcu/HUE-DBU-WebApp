using DBCU_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ActivityMAsController : Controller
    {
        private readonly DBUWebContext _context;

        public ActivityMAsController(DBUWebContext context)
        {
            _context = context;
        }

        // GET: Admin/ActivityMAs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActivityMA.ToListAsync());
        }

        // GET: Admin/ActivityMAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMA = await _context.ActivityMA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityMA == null)
            {
                return NotFound();
            }

            return View(activityMA);
        }

        // GET: Admin/ActivityMAs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ActivityMAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityCode,ActivityName,ActivityNameEN")] ActivityMA activityMA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityMA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityMA);
        }

        // GET: Admin/ActivityMAs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMA = await _context.ActivityMA.FindAsync(id);
            if (activityMA == null)
            {
                return NotFound();
            }
            return View(activityMA);
        }

        // POST: Admin/ActivityMAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityCode,ActivityName,ActivityNameEN")] ActivityMA activityMA)
        {
            if (id != activityMA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityMA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityMAExists(activityMA.Id))
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
            return View(activityMA);
        }

        // GET: Admin/ActivityMAs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityMA = await _context.ActivityMA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityMA == null)
            {
                return NotFound();
            }

            return View(activityMA);
        }

        // POST: Admin/ActivityMAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityMA = await _context.ActivityMA.FindAsync(id);
            _context.ActivityMA.Remove(activityMA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityMAExists(int id)
        {
            return _context.ActivityMA.Any(e => e.Id == id);
        }
    }
}
