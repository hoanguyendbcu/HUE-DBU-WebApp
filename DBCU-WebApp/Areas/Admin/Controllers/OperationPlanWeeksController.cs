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
    public class OperationPlanWeeksController : Controller
    {
        private readonly DBCU_WebContext _context;

        public OperationPlanWeeksController(DBCU_WebContext context)
        {
            _context = context;
        }

        // GET: Admin/OperationPlanWeeks
        public async Task<IActionResult> Index()
        {
            return View(await _context.OperationPlanWeek.ToListAsync());
        }

        // GET: Admin/OperationPlanWeeks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationPlanWeek = await _context.OperationPlanWeek
                .FirstOrDefaultAsync(m => m.OperationPlanWeekID == id);
            if (operationPlanWeek == null)
            {
                return NotFound();
            }

            return View(operationPlanWeek);
        }

        // GET: Admin/OperationPlanWeeks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/OperationPlanWeeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationPlanWeekID,Week,Year,FromDate,ToDate")] OperationPlanWeek operationPlanWeek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operationPlanWeek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(operationPlanWeek);
        }

        // GET: Admin/OperationPlanWeeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationPlanWeek = await _context.OperationPlanWeek.FindAsync(id);
            if (operationPlanWeek == null)
            {
                return NotFound();
            }
            return View(operationPlanWeek);
        }

        // POST: Admin/OperationPlanWeeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperationPlanWeekID,Week,Year,FromDate,ToDate")] OperationPlanWeek operationPlanWeek)
        {
            if (id != operationPlanWeek.OperationPlanWeekID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operationPlanWeek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationPlanWeekExists(operationPlanWeek.OperationPlanWeekID))
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
            return View(operationPlanWeek);
        }

        // GET: Admin/OperationPlanWeeks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationPlanWeek = await _context.OperationPlanWeek
                .FirstOrDefaultAsync(m => m.OperationPlanWeekID == id);
            if (operationPlanWeek == null)
            {
                return NotFound();
            }

            return View(operationPlanWeek);
        }

        // POST: Admin/OperationPlanWeeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operationPlanWeek = await _context.OperationPlanWeek.FindAsync(id);
            _context.OperationPlanWeek.Remove(operationPlanWeek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationPlanWeekExists(int id)
        {
            return _context.OperationPlanWeek.Any(e => e.OperationPlanWeekID == id);
        }
    }
}
