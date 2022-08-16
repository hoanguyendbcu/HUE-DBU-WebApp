using DBCU_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly DBUWebContext _context;

        public OrganizationsController(DBUWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Organizations
        public async Task<IActionResult> Index()
        {
            var DBUWebContext = _context.Organization.Include(o => o.ParentOrganization);
            return View(await DBUWebContext.ToListAsync());
        }

        // GET: Admin/Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .Include(o => o.ParentOrganization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Admin/Organizations/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["ParentId"] = new SelectList(_context.Organization, "Id", "OganizationNameEN");
            var listOrganization = await _context.Organization.ToListAsync();
            listOrganization.Insert(0, new Organization()
            {
                OrganizationName = "",
                Id = -1
            });
            ViewData["ParentId"] = new MultiSelectList(listOrganization, "Id", "OrganizationName"); // new SelectList(await GetItemsSelectCategorie(), "Id", "Title", -1);
            return View();
        }

        // POST: Admin/Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrganizationCode,ParentId,OrganizationName,OganizationNameEN")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                if (organization.ParentId.Value == -1)
                    organization.ParentId = null;
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Organization, "Id", "OrganizationName", organization.ParentId);
            return View(organization);
        }

        // GET: Admin/Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Organization, "Id", "OrganizationName", organization.ParentId);
            return View(organization);
        }

        // POST: Admin/Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationCode,ParentId,OrganizationName,OganizationNameEN")] Organization organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (organization.ParentId == -1)
                    {
                        organization.ParentId = null;
                    }
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
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
            ViewData["ParentId"] = new SelectList(_context.Organization, "Id", "OganizationNameEN", organization.ParentId);
            return View(organization);
        }

        // GET: Admin/Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .Include(o => o.ParentOrganization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Admin/Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _context.Organization.FindAsync(id);
            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }
    }
}
