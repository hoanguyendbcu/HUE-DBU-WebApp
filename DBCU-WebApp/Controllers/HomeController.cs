using DBCU_WebApp.Models;
using DBCU_WebApp.Models.GeoCHA;
using DBCU_WebApp.Models.GeoClearance;
using DBCU_WebApp.Models.GeoModel;
using DBCU_WebApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBUWebContext _context;
        private readonly MissonEODRepository missonEODRepository;

        public HomeController(DBUWebContext context, IConfiguration configuration)
        {
            _context = context;
            missonEODRepository = new MissonEODRepository(configuration);
        }
        public async Task<IActionResult> Index()
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            ViewData["missonEOD"] = await missonEODRepository.GetMissonEOD();
            //ViewData["missonNTS"] = await missonEODRepository.GetMissonNTS();
            //ViewData["missonTS"] = await missonEODRepository.GetMissonTS();
            //ViewData["missonCLC"] = await missonEODRepository.GetMissonCLC();
            ViewData["areaCLC"] = await missonEODRepository.GetAreaCLC();
            ViewData["areaCHA"] = await missonEODRepository.GetAreaCHA();
            ViewData["NoERW"] = await missonEODRepository.GetNoERW();

            ViewData["NoVictimACC"] = await missonEODRepository.NoVictimACC();

            var data = await missonEODRepository.GetGeoClearance();
            var lstGeoClearance = new GeoClearance();
            lstGeoClearance.Type = "FeatureCollection";
            var features = new List<Models.GeoClearance.Feature>();
            int i = 0;

            // Truy vấn lấy các post
            var posts = _context.News
                //.Include(p => p.AuthorId) // Load Author cho post  
                .Include(p => p.NewsCategories) // Load các Category của Post
                .ThenInclude(c => c.Category)
                .OrderByDescending(c => c.NewsId)
                 .Take(5);


            foreach (var results in data)
            {
                var feature = new Models.GeoClearance.Feature();
                var pro = new Models.GeoClearance.Properties();
                var geo = new Models.GeoClearance.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_team = results.Reporting_team;
                pro.reporting_org_name = results.Reporting_org_name;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Enddate = results.Enddate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoClearance.Geometry>(results.Polygon);
           
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                feature.Id = i;
                features.Add(feature);
                i = ++i;

            }

            lstGeoClearance.Features = features;
            ViewData["lstGeoClearance"] = JsonSerializer.Serialize(lstGeoClearance);

            // Geo CHA  
            var dataCha = await missonEODRepository.GetGeoCHA();
            var lstGeoCHA = new GeoCHA();
            lstGeoCHA.Type = "FeatureCollection";
            var featuresCHA = new List<Models.GeoCHA.Feature>();

            foreach (var results in dataCha)
            {
                var feature = new Models.GeoCHA.Feature();
                var pro = new Models.GeoCHA.Properties();
                var geo = new Models.GeoCHA.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_org_name = results.Reporting_org_name;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoCHA.Geometry>(results.Polygon);

                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                feature.Id = i;
                featuresCHA.Add(feature);
                i = ++i;

            }

            lstGeoCHA.Features = featuresCHA;
            ViewData["lstGeoCHA"] = JsonSerializer.Serialize(lstGeoCHA);

            // Geo CHA Open
            var dataChaOpen = await missonEODRepository.GetGeoCHAOpen();
            var lstGeoCHAOpen = new GeoCHA();
            lstGeoCHAOpen.Type = "FeatureCollection";
            var featuresCHAOpen = new List<Models.GeoCHA.Feature>();

            foreach (var results in dataChaOpen)
            {
                var feature = new Models.GeoCHA.Feature();
                var pro = new Models.GeoCHA.Properties();
                var geo = new Models.GeoCHA.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_org_name = results.Reporting_org_name;
                pro.Reporting_Org_localid = results.Reporting_Org_localid;
                pro.Survey_method = results.Survey_method;
                pro.Land_Use = results.Land_Use;
                pro.Comment_Land_Use = results.Comment_Land_Use;
                pro.Clearance_Priority = results.Clearance_Priority;
                pro.Type_of_Area = results.Type_of_Area;
                pro.Vehicle_Type = results.Vehicle_Type;
                pro.Vegetation_removed = results.Vegetation_removed;
                pro.Soiltype = results.Soiltype;
                pro.Vegetation_Type = results.Vegetation_Type;
                pro.Vegetation_density = results.Vegetation_density;
                pro.Slopee = results.Slopee;
                pro.Beneficiaries = results.Beneficiaries;
                pro.Soil_Condition = results.Soil_Condition;
                pro.Additional_Information = results.Additional_Information;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoCHA.Geometry>(results.Polygon);

                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                feature.Id = i;
                featuresCHAOpen.Add(feature);
                i = ++i;
            }

            lstGeoCHAOpen.Features = featuresCHAOpen;
            ViewData["lstGeoCHAOpen"] = JsonSerializer.Serialize(lstGeoCHAOpen);

            //GeoCHA Suspended
            var dataChaSuspended = await missonEODRepository.GetGeoCHASuspended();
            var lstGeoCHASuspended = new GeoCHA();
            lstGeoCHASuspended.Type = "FeatureCollection";
            var featuresCHASuspended = new List<Models.GeoCHA.Feature>();

            foreach (var results in dataChaSuspended)
            {
                var feature = new Models.GeoCHA.Feature();
                var pro = new Models.GeoCHA.Properties();
                var geo = new Models.GeoCHA.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_org_name = results.Reporting_org_name;
                pro.Reporting_Org_localid = results.Reporting_Org_localid;
                pro.Survey_method = results.Survey_method;
                pro.Land_Use = results.Land_Use;
                pro.Comment_Land_Use = results.Comment_Land_Use;
                pro.Clearance_Priority = results.Clearance_Priority;
                pro.Type_of_Area = results.Type_of_Area;
                pro.Vehicle_Type = results.Vehicle_Type;
                pro.Vegetation_removed = results.Vegetation_removed;
                pro.Soiltype = results.Soiltype;
                pro.Vegetation_Type = results.Vegetation_Type;
                pro.Vegetation_density = results.Vegetation_density;
                pro.Slopee = results.Slopee;
                pro.Beneficiaries = results.Beneficiaries;
                pro.Soil_Condition = results.Soil_Condition;
                pro.Additional_Information = results.Additional_Information;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHASuspended.Add(feature);
                i = ++i;

            }

            lstGeoCHASuspended.Features = featuresCHASuspended;
            ViewData["lstGeoCHASuspended"] = JsonSerializer.Serialize(lstGeoCHASuspended);

            //GeoCHA Completed
            var dataChaCompleted = await missonEODRepository.GetGeoCHACompleted();
            var lstGeoCHACompleted = new GeoCHA();
            lstGeoCHACompleted.Type = "FeatureCollection";
            var featuresCHACompleted = new List<Models.GeoCHA.Feature>();

            foreach (var results in dataChaCompleted)
            {
                var feature = new Models.GeoCHA.Feature();
                var pro = new Models.GeoCHA.Properties();
                var geo = new Models.GeoCHA.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_org_name = results.Reporting_org_name;
                pro.Reporting_Org_localid = results.Reporting_Org_localid;
                pro.Survey_method = results.Survey_method;
                pro.Land_Use = results.Land_Use;
                pro.Comment_Land_Use = results.Comment_Land_Use;
                pro.Clearance_Priority = results.Clearance_Priority;
                pro.Type_of_Area = results.Type_of_Area;
                pro.Vehicle_Type = results.Vehicle_Type;
                pro.Vegetation_removed = results.Vegetation_removed;
                pro.Soiltype = results.Soiltype;
                pro.Vegetation_Type = results.Vegetation_Type;
                pro.Vegetation_density = results.Vegetation_density;
                pro.Slopee = results.Slopee;
                pro.Beneficiaries = results.Beneficiaries;
                pro.Soil_Condition = results.Soil_Condition;
                pro.Additional_Information = results.Additional_Information;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoCHA.Geometry>(results.Polygon);

                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHACompleted.Add(feature);
                i = ++i;
            }

            lstGeoCHACompleted.Features = featuresCHACompleted;
            ViewData["lstGeoCHACompleted"] = JsonSerializer.Serialize(lstGeoCHACompleted);

            //GeoCHA Completed
            var dataChaOngoing = await missonEODRepository.GetGeoCHAOngoing();
            var lstGeoCHAOngoing = new GeoCHA();
            lstGeoCHAOngoing.Type = "FeatureCollection";
            var featuresCHAOngoing = new List<Models.GeoCHA.Feature>();

            foreach (var results in dataChaOngoing)
            {
                var feature = new Models.GeoCHA.Feature();
                var pro = new Models.GeoCHA.Properties();
                var geo = new Models.GeoCHA.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.Reporting_org_name = results.Reporting_org_name;
                pro.Reporting_Org_localid = results.Reporting_Org_localid;
                pro.Survey_method = results.Survey_method;
                pro.Land_Use = results.Land_Use;
                pro.Comment_Land_Use = results.Comment_Land_Use;
                pro.Clearance_Priority = results.Clearance_Priority;
                pro.Type_of_Area = results.Type_of_Area;
                pro.Vehicle_Type = results.Vehicle_Type;
                pro.Vegetation_removed = results.Vegetation_removed;
                pro.Soiltype = results.Soiltype;
                pro.Vegetation_Type = results.Vegetation_Type;
                pro.Vegetation_density = results.Vegetation_density;
                pro.Slopee = results.Slopee;
                pro.Beneficiaries = results.Beneficiaries;
                pro.Soil_Condition = results.Soil_Condition;
                pro.Additional_Information = results.Additional_Information;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo =  JsonSerializer.Deserialize<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHAOngoing.Add(feature);
                i = ++i;
            }

            lstGeoCHAOngoing.Features = featuresCHAOngoing;
            ViewData["lstGeoCHAOngoing"] = JsonSerializer.Serialize(lstGeoCHAOngoing);

            return View(await posts.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> OpeartionPlan(int WeekID)
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string strWeek = "", strFrom = "", strTo = "";

            if (lang == "en")
            {
                strWeek = " Week ";
                strFrom = " From ";
                strTo = " to ";
            }
            else if (lang == "vi")
            {
                strWeek = " Tuần ";
                strFrom = " Từ ";
                strTo = " Đến ";

            }

            var operationplanweek = await _context.OperationPlanWeek
            .Select(
             p => new
             {
                 Week = p.OperationPlanWeekID,
                 WeekName = strWeek + p.Week.ToString() + strFrom + p.FromDate.ToString("dd/MM/yyyy") + strTo + p.ToDate.ToString("dd/MM/yyyy"),
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
  
                  .OrderBy(p => p.Org)
                  .ThenBy(p => p.Team)
                  .ThenBy(p => p.Activity)
                  .ThenBy(p => p.ProvinceID)
                  .ThenBy(p => p.DistictID)
                  .ThenBy(p => p.CommuneID)
                  .ThenBy(p => p.VillageID)
                  .ToListAsync();

            var lstGeoOperationPlan = new GeoOperationPlan();
            lstGeoOperationPlan.Type = "FeatureCollection";

            var features = new List<Models.GeoModel.Feature>();

            foreach (var results in data)
            {
                if (results.Village != null && (results.Village.Long != null && results.Village.Long != 0))
                {
                    var feature = new Models.GeoModel.Feature();
                    var pro = new Models.GeoModel.Properties();
                    pro.Message = results.Organization.OganizationNameEN;
                    pro.Org = results.Organization.OrganizationCode;
                    if (lang == "vi")
                    {
                        pro.Team = results.Teams.OrganizationName;
                        pro.Activity = results.ActivityMA.ActivityName;
                        pro.Location = results.District.Gazetteername + " , " + results.Commune.Gazetteername + " , " + results.Village.Gazetteername;

                    }
                    else if (lang == "en")
                    {
                        pro.Team = results.Teams.OganizationNameEN;
                        pro.Activity = results.ActivityMA.ActivityNameEN;
                        pro.Location = results.District.Gazetteername_eng + " , " + results.Commune.Gazetteername_eng + " , " + results.Village.Gazetteername_eng;
                    }

                    pro.TaskID = results.TaskID;
                    pro.StartDate = (results.StartDate).ToString();
                    pro.EndDate = results.EndDate.ToString();

                    var geo = new Models.GeoModel.Geometry();
                    geo.Type = "Point";
                    geo.Coordinates = new List<float?> { results.Village.Long, results.Village.Lat };

                    feature.Type = "Feature";
                    feature.Properties = pro;
                    feature.Geometry = geo;

                    features.Add(feature);
                }
            }

            lstGeoOperationPlan.Features = features;
            ViewData["lstGeoOperationPlan"] = JsonSerializer.Serialize(lstGeoOperationPlan);

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_OperationPlans", data);

            return View(data);
        }

        public IActionResult OpeartionImage()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Video()
        {
            return View();
        }

        public IActionResult Document()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
