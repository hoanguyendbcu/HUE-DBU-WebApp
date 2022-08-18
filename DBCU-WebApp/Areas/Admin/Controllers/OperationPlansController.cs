using DBCU_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OperationPlansController : Controller
    {
        private readonly DBUWebContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OperationPlansController(DBUWebContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/OperationPlans
        public async Task<IActionResult> Index(int WeekID)
        {
            var operationplanweek = await _context.OperationPlanWeek
            .Select(
             p => new
             {
                 Week = p.OperationPlanWeekID,
                 WeekName = "Week " + p.Week.ToString() + " From " + p.FromDate.ToString("dd/MM/yyyy") + " to " + p.ToDate.ToString("dd/MM/yyyy"),
                 Year = p.Year,
                 WeekID = p.Week
             })
            .OrderByDescending(c => c.Year).ThenByDescending(c => c.WeekID)
            .ToListAsync();

            ViewData["operationplanweek"] = new MultiSelectList(operationplanweek, "Week", "WeekName");

            if (WeekID == 0 && _context.OperationPlanWeek.Any())
            {
                WeekID = _context.OperationPlanWeek.OrderByDescending(c => c.Year).ThenByDescending(c => c.Week).Max(p => p.OperationPlanWeekID);
            }

            var data = await _context.OperationPlans
                  .Include(p => p.OperationPlanWeek)
                  .Include(p => p.Organization)
                  .Include(p => p.Teams)
                  .Include(p => p.ActivityMA)
                  .Include(p => p.Province)
                  .Include(p => p.District)
                  .Include(p => p.Commune)
                  .Include(p => p.Village)
                  .Where(p => p.Week == WeekID)
                   .OrderByDescending(p => p.Id)

                  .ToListAsync();

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_OperationPlans", data);

            return View(data);
        }

        // GET: Admin/OperationPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationPlans = await _context.OperationPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationPlans == null)
            {
                return NotFound();
            }

            return View(operationPlans);
        }

        // GET: Admin/OperationPlans/Create
        public async Task<IActionResult> Create()
        {
            var operationplanweek = await _context.OperationPlanWeek
             .Select(
              p => new
              {
                  Week = p.OperationPlanWeekID,
                  WeekName = "Week " + p.Week.ToString() + " From " + p.FromDate.ToString("dd/MM/yyyy") + " to " + p.ToDate.ToString("dd/MM/yyyy"),
                  Year = p.Year,
                  WeekID = p.Week
              })
             .OrderByDescending(c => c.Year).ThenByDescending(c => c.WeekID)
             .ToListAsync();
            ViewData["operationplanweek"] = new MultiSelectList(operationplanweek, "Week", "WeekName");

            var listOrganization = await _context.Organization
                .Where(p => p.ParentId == null)
                .ToListAsync();
            listOrganization.Insert(0, new Organization { Id = 0, OrganizationName = "Select" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "Id", "OrganizationName");

            var activityam = await _context.ActivityMA.ToListAsync();
            activityam.Insert(0, new ActivityMA { Id = 0, ActivityName = "Select" });
            ViewData["activityam"] = new MultiSelectList(activityam, "Id", "ActivityName");

            var listprovince = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == null)
                .ToListAsync();
            ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

            var listdistrict = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == "c0a8-003e-17e712d40f3-c946ffe9-3-81bf")
                .ToListAsync();
            listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });
            ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");

            return View();
        }

        // POST: Admin/OperationPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Week,Org,Team,Activity,ProvinceID,DistictID,CommuneID,VillageID,TaskID,Areas,StartDate,EndDate,Comment,UserCreated,DateCreated,DateUpdated")] OperationPlans operationPlans)
        {
            if (operationPlans.Org == 0)
            {
                ModelState.AddModelError(string.Empty, "Must be choise Org");
            }
            if (operationPlans.Team == 0)
            {
                ModelState.AddModelError(string.Empty, "Must be choise Team");
            }
            if (operationPlans.Activity == 0)
            {
                ModelState.AddModelError(string.Empty, "Must be choise Activity");
            }
            if (operationPlans.ProvinceID == "0")
            {
                ModelState.AddModelError(string.Empty, "Must be choise Province");
            }

            if (ModelState.IsValid)
            {
                operationPlans.DateCreated = DateTime.Now;
                operationPlans.DateUpdated = null;

                if (operationPlans.StartDate != null  )
                {
                    operationPlans.StartDate = DateTime.ParseExact(operationPlans.StartDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                if (operationPlans.EndDate != null)
                {
                    operationPlans.EndDate = DateTime.ParseExact(operationPlans.EndDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                operationPlans.UserCreated = _userManager.GetUserName(User);

                _context.Add(operationPlans);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var operationplanweek = await _context.OperationPlanWeek
              .Select(
               p => new
               {
                   Week = p.OperationPlanWeekID,
                   WeekName = "Week " + p.Week.ToString() + " From " + p.FromDate.ToString("dd/MM/yyyy") + " to " + p.ToDate.ToString("dd/MM/yyyy"),
                   Year = p.Year,
                   WeekID = p.Week
               })
              .OrderByDescending(c => c.Year).ThenByDescending(c => c.WeekID)
              .ToListAsync();
            ViewData["operationplanweek"] = new MultiSelectList(operationplanweek, "Week", "WeekName");

            var listOrganization = await _context.Organization
                .Where(p => p.ParentId == null)
                .ToListAsync();
            listOrganization.Insert(0, new Organization { Id = 0, OrganizationName = "Select" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "Id", "OrganizationName");

            var activityam = await _context.ActivityMA.ToListAsync();
            activityam.Insert(0, new ActivityMA { Id = 0, ActivityName = "Select" });
            ViewData["activityam"] = new MultiSelectList(activityam, "Id", "ActivityName");

            var listprovince = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == null)
                .ToListAsync();
            ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

            var listdistrict = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == "c0a8-003e-17e712d40f3-c946ffe9-3-81bf")
                .ToListAsync();
            listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });
            ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");
            return View(operationPlans);
        }

        // GET: Admin/OperationPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationplanweek = await _context.OperationPlanWeek
                       .Select(
                        p => new
                        {
                            Week = p.OperationPlanWeekID,
                            WeekName = "Week " + p.Week.ToString() + " From " + p.FromDate.ToString("dd/MM/yyyy") + " to " + p.ToDate.ToString("dd/MM/yyyy"),
                            Year = p.Year,
                            WeekID = p.Week
                        })
                       .OrderByDescending(c => c.Year).ThenByDescending(c => c.WeekID)
                       .ToListAsync();
            ViewData["operationplanweek"] = new MultiSelectList(operationplanweek, "Week", "WeekName");

            var listOrganization = await _context.Organization
                .Where(p => p.ParentId == null)
                .ToListAsync();
            //listOrganization.Insert(0, new Organization { OrganizationCode = "0", OrganizationName = "Select" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "Id", "OrganizationName");

            var activityam = await _context.ActivityMA.ToListAsync();
            //activityam.Insert(0, new ActivityMA { ActivityCode = "0", ActivityName = "Select" });
            ViewData["activityam"] = new MultiSelectList(activityam, "Id", "ActivityName");

            var listprovince = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == null)
                .ToListAsync();
            ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

            var listdistrict = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == "c0a8-003e-17e712d40f3-c946ffe9-3-81bf")
                .ToListAsync();
            listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });
            ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");

            var operationPlans = await _context.OperationPlans.FindAsync(id);

            operationPlans.StartDate?.ToShortTimeString();
            operationPlans.EndDate?.ToShortTimeString();
            // operationPlans.StartDate = DateTime.ParseExact(operationPlans.StartDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //operationPlans.EndDate = DateTime.ParseExact(operationPlans.EndDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (operationPlans == null)
            {
                return NotFound();
            }
            return View(operationPlans);
        }

        // POST: Admin/OperationPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Week,Org,Team,Activity,ProvinceID,DistictID,CommuneID,VillageID,TaskID,Areas,StartDate,EndDate,Comment,UserCreated,DateCreated,DateUpdated")] OperationPlans operationPlans)
        {
            if (id != operationPlans.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    operationPlans.DateUpdated = DateTime.Now;
                    if (operationPlans.StartDate != null)
                    {
                        operationPlans.StartDate = DateTime.ParseExact(operationPlans.StartDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (operationPlans.EndDate != null)
                    {
                        operationPlans.EndDate = DateTime.ParseExact(operationPlans.EndDate?.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
            
                    _context.Update(operationPlans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationPlansExists(operationPlans.Id))
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
            var operationplanweek = await _context.OperationPlanWeek
                     .Select(
                      p => new
                      {
                          Week = p.OperationPlanWeekID,
                          WeekName = "Week " + p.Week.ToString() + " From " + p.FromDate.ToString("dd/MM/yyyy") + " to " + p.ToDate.ToString("dd/MM/yyyy"),
                          Year = p.Year,
                          WeekID = p.Week
                      })
                     .OrderByDescending(c => c.Year).ThenByDescending(c => c.WeekID)
                     .ToListAsync();
            ViewData["operationplanweek"] = new MultiSelectList(operationplanweek, "Week", "WeekName");

            var listOrganization = await _context.Organization
                .Where(p => p.ParentId == null)
                .ToListAsync();
            //listOrganization.Insert(0, new Organization { OrganizationCode = "0", OrganizationName = "Select" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "Id", "OrganizationName");

            var activityam = await _context.ActivityMA.ToListAsync();
            //activityam.Insert(0, new ActivityMA { ActivityCode = "0", ActivityName = "Select" });
            ViewData["activityam"] = new MultiSelectList(activityam, "Id", "ActivityName");

            var listprovince = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == null)
                .ToListAsync();
            ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

            var listdistrict = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == "c0a8-003e-17e712d40f3-c946ffe9-3-81bf")
                .ToListAsync();
            listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });
            ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");

          

            operationPlans.StartDate?.ToShortTimeString();
            operationPlans.EndDate?.ToShortTimeString();
            return View(operationPlans);
        }

        // GET: Admin/OperationPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationPlans = await _context.OperationPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationPlans == null)
            {
                return NotFound();
            }

            return View(operationPlans);
        }

        // POST: Admin/OperationPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operationPlans = await _context.OperationPlans.FindAsync(id);
            _context.OperationPlans.Remove(operationPlans);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationPlansExists(int id)
        {
            return _context.OperationPlans.Any(e => e.Id == id);
        }

        public async Task<JsonResult> GetTeam(int orgId)
        {
            var response = await _context.Organization
                  .Where(p => p.ParentId == orgId)
                  .ToListAsync();

            // ------- Inserting Select Item in List -------
            response.Insert(0, new Organization { Id = 0, OrganizationName = "Select" });

            return Json(new SelectList(response, "Id", "OrganizationName"));

        }

        public async Task<JsonResult> GetCommune(string DistictID)
        {
            var response = await _context.Gazetteer
                  .Where(p => p.Parentgazetteer_guid == DistictID)
                  .ToListAsync();

            // ------- Inserting Select Item in List -------
            response.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });

            return Json(new SelectList(response, "Gazetteer_guid", "Gazetteername"));

        }
        public async Task<JsonResult> GetVillage(string CommuneID)
        {
            var response = await _context.Gazetteer
                  .Where(p => p.Parentgazetteer_guid == CommuneID)
                  .ToListAsync();

            // ------- Inserting Select Item in List -------
            response.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });

            return Json(new SelectList(response, "Gazetteer_guid", "Gazetteername"));

        }
    }
}
