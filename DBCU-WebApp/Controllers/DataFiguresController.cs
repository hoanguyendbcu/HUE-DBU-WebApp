﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBCU_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DBCU_WebApp.Repository;
using Microsoft.Extensions.Configuration;
using DBCU_WebApp.Models.GeoClearance;
using Newtonsoft.Json;
using DBCU_WebApp.Models.GeoCHA;

namespace DBCU_WebApp.Controllers
{
    public class DataFiguresController : Controller
    {
        private readonly DBCU_WebContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly MissonEODRepository missonEODRepository;
        private readonly DataFiguresRepository dataFiguresRepository;

        public DataFiguresController(DBCU_WebContext context, IConfiguration configuration, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            missonEODRepository = new MissonEODRepository(configuration);
            dataFiguresRepository = new DataFiguresRepository(configuration);
        }
        public async Task<IActionResult> Index()
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string strSelect = "";
            if (lang == "en")
            {
                strSelect = "Select";

            }
            else if (lang == "vi")
            {
                strSelect = "Chọn";

            }
            var listprovince = await _context.Gazetteer
               .Where(p => p.Parentgazetteer_guid == null)
               .ToListAsync();
            ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

            var listdistrict = await _context.Gazetteer
                .Where(p => p.Parentgazetteer_guid == "0a0a-017b-12e6ca6fce1-6dcce7a2-7-2aef")
                .ToListAsync();
            listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = strSelect });
            ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");
            return View();
        }

        //public async Task<IActionResult> Index(string a)
        //{
        //    var listprovince = await _context.Gazetteer
        //       .Where(p => p.Parentgazetteer_guid == null)
        //       .ToListAsync();
        //    ViewData["listprovince"] = new MultiSelectList(listprovince, "Gazetteer_guid", "Gazetteername");

        //    var listdistrict = await _context.Gazetteer
        //        .Where(p => p.Parentgazetteer_guid == "0a0a-017b-12e6ca6fce1-6dcce7a2-7-2aef")
        //        .ToListAsync();
        //    listdistrict.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = "Select" });
        //    ViewData["listdistrict"] = new MultiSelectList(listdistrict, "Gazetteer_guid", "Gazetteername");
        //    return View();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DistictID"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCommune(string DistictID)
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string strSelect = "";
            if (lang == "en")
            {
                strSelect = "Select";

            }
            else if (lang == "vi")
            {
                strSelect = "Chọn";

            }

            var response = await _context.Gazetteer
                  .Where(p => p.Parentgazetteer_guid == DistictID)
                  .ToListAsync();

            // ------- Inserting Select Item in List -------
            response.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = strSelect });

            return Json(new SelectList(response, "Gazetteer_guid", "Gazetteername"));

        }
        public async Task<JsonResult> GetVillage(string CommuneID)
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string strSelect = "";
            if (lang == "en")
            {
                strSelect = "Select";

            }
            else if (lang == "vi")
            {
                strSelect = "Chọn";

            }

            var response = await _context.Gazetteer
                  .Where(p => p.Parentgazetteer_guid == CommuneID)
                  .ToListAsync();

            // ------- Inserting Select Item in List -------
            response.Insert(0, new Gazetteer { Gazetteer_guid = "0", Gazetteername = strSelect });

            return Json(new SelectList(response, "Gazetteer_guid", "Gazetteername"));

        }

        public async Task<IActionResult> QuangBinhProvince()
        {
            ViewBag.NavClassByProvince = "active";
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "0";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);


            return View();
        }

        public async Task<IActionResult> DongHoiCity()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Dong Hoi City ";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);


            return View();
        }

        public async Task<IActionResult> BaDonTown()
        {
            ViewBag.NavClassByProvince = "active";
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Ba Don Town";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);


            return View();
        }

        public async Task<IActionResult> MinhHoaDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Minh Hoa";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);
            return View();
        }
        public async Task<IActionResult> TuyenHoaDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Tuyen Hoa";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);

            return View();
        }
        public async Task<IActionResult> QuangTrachDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Quang Trach";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);

            return View();
        }
        public async Task<IActionResult> BoTrachDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Bo Trach";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);
            return View();
        }
        public async Task<IActionResult> QuangNinhDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Quang Ninh";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);


            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);

            return View();
        }

        public async Task<IActionResult> LeThuyDistrict()
        {
            ViewBag.NavClassByProvince = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            string distict = "Le Thuy";

            ViewData["areaCLC"] = await dataFiguresRepository.GetAreaCLC(distict);
            ViewData["NoERW"] = await dataFiguresRepository.GetNoERW(distict);
            ViewData["NoMRE"] = await dataFiguresRepository.GetNoMRE(distict);

            var jsonCategory = await dataFiguresRepository.UXOCategory(distict);
            var jsonModel = await dataFiguresRepository.UXOModel(distict);


            ViewData["UXOCategory"] = JsonConvert.SerializeObject(jsonCategory);
            ViewData["UXOModel"] = JsonConvert.SerializeObject(jsonModel);

            if (lang == "en")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGender(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGender(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);

            }
            else if (lang == "vi")
            {
                var jsonMREGender = await dataFiguresRepository.GetMREGenderVN(distict);
                var jsonMREChildGender = await dataFiguresRepository.GetMREChildGenderVN(distict);
                ViewData["MREGender"] = JsonConvert.SerializeObject(jsonMREGender);
                ViewData["MREChildGender"] = JsonConvert.SerializeObject(jsonMREChildGender);
            }

            var jsonDataClearnceChart = await dataFiguresRepository.GetDataClearnceChart2(distict);
            ViewData["DataClearnceChart"] = JsonConvert.SerializeObject(jsonDataClearnceChart);
            return View();
        }

        /// <summary>
        /// ByMineAction
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ByMineActionSurvey()
        {
            ViewBag.NavClassByMineAction = "active";

            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            ViewData["areaCLC"] = await missonEODRepository.GetAreaCLC();
            ViewData["NoERW"] = await missonEODRepository.GetNoERW();
            ViewData["NoMRE"] = await missonEODRepository.GetNoMRE();

            int i = 0;

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

                geo = JsonConvert.DeserializeObject<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHA.Add(feature);
                i = ++i;
            }

            lstGeoCHA.Features = featuresCHA;
            ViewData["lstGeoCHA"] = JsonConvert.SerializeObject(lstGeoCHA);

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

                geo = JsonConvert.DeserializeObject<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHAOpen.Add(feature);
                i = ++i;

            }

            lstGeoCHAOpen.Features = featuresCHAOpen;
            ViewData["lstGeoCHAOpen"] = JsonConvert.SerializeObject(lstGeoCHAOpen);

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

                geo = JsonConvert.DeserializeObject<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHASuspended.Add(feature);
                i = ++i;

            }

            lstGeoCHASuspended.Features = featuresCHASuspended;
            ViewData["lstGeoCHASuspended"] = JsonConvert.SerializeObject(lstGeoCHASuspended);

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

                geo = JsonConvert.DeserializeObject<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHACompleted.Add(feature);
                i = ++i;

            }

            lstGeoCHACompleted.Features = featuresCHACompleted;
            ViewData["lstGeoCHACompleted"] = JsonConvert.SerializeObject(lstGeoCHACompleted);

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

                geo = JsonConvert.DeserializeObject<Models.GeoCHA.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCHAOngoing.Add(feature);
                i = ++i;
            }

            lstGeoCHAOngoing.Features = featuresCHAOngoing;
            ViewData["lstGeoCHAOngoing"] = JsonConvert.SerializeObject(lstGeoCHAOngoing);

            ///

            var dataChaAreaSize = await dataFiguresRepository.GetDataCHA();
            int getDataCHASuspended = 0, getDataCHAOngoing = 0, getDataCHACompleted = 0, getDataCHAOpen = 0, getDataCHAUnCompleted = 0, getDataCHATotal = 0;


            if (dataChaAreaSize.Where(f => f.Status == "Suspended").Any())
            {
                getDataCHASuspended = (int)dataChaAreaSize.Where(f => f.Status == "Suspended").FirstOrDefault().AreaSize;
            }
            if (dataChaAreaSize.Where(f => f.Status == "Ongoing").Any())
            {
                getDataCHAOngoing = (int)dataChaAreaSize.Where(f => f.Status == "Ongoing").FirstOrDefault().AreaSize;
            }
            if (dataChaAreaSize.Where(f => f.Status == "Completed").Any())
            {
                getDataCHACompleted = (int)dataChaAreaSize.Where(f => f.Status == "Completed").FirstOrDefault().AreaSize;
            }
            if (dataChaAreaSize.Where(f => f.Status == "Closed").Any())
            {
                getDataCHACompleted = getDataCHACompleted + (int)dataChaAreaSize.Where(f => f.Status == "Closed").FirstOrDefault().AreaSize;
            }
            if (dataChaAreaSize.Where(f => f.Status == "Open").Any())
            {
                getDataCHAOpen = (int)dataChaAreaSize.Where(f => f.Status == "Open").FirstOrDefault().AreaSize;
            }

            getDataCHATotal = getDataCHASuspended + getDataCHAOngoing + getDataCHACompleted + getDataCHAOpen;
            getDataCHAUnCompleted = getDataCHASuspended + getDataCHAOngoing + getDataCHAOpen;

            ViewData["getDataCHASuspended"] = getDataCHASuspended;
            ViewData["getDataCHAOngoing"] = getDataCHAOngoing;
            ViewData["getDataCHACompleted"] = getDataCHACompleted;
            ViewData["getDataCHAOpen"] = getDataCHAOpen;
            ViewData["getDataCHATotal"] = getDataCHATotal;
            ViewData["getDataCHAUnCompleted"] = getDataCHAUnCompleted;

            var jsonDataCHAByYear = await dataFiguresRepository.GetDataCHAByYear();
            ViewData["DataCHAByYear"] = JsonConvert.SerializeObject(jsonDataCHAByYear);

            if (lang == "vi")
            {
                var jsonDataCHAByDistrict = await dataFiguresRepository.GetDataCHAByDistrict();
                ViewData["DataCHAByDistrict"] = JsonConvert.SerializeObject(jsonDataCHAByDistrict);
            }
            else
            {
                var jsonDataCHAByDistrict = await dataFiguresRepository.GetDataCHAByDistrictEN();
                ViewData["DataCHAByDistrict"] = JsonConvert.SerializeObject(jsonDataCHAByDistrict);
            }

            var jsonModel = await dataFiguresRepository.UXOModelSurvey();
            ViewData["UXOModelSurvey"] = JsonConvert.SerializeObject(jsonModel);

            var jsonDataCHAByStatus = await dataFiguresRepository.GetDataCHAByStatus();
            ViewData["DataCHAByStatus"] = JsonConvert.SerializeObject(jsonDataCHAByStatus);


            return View();
        }

        public async Task<IActionResult> ByMineActionClearance(string Org)
        {
            ViewBag.NavClassByMineAction = "active";
            string lang = System.Globalization.CultureInfo.CurrentCulture.ToString();
            int i = 0;


            var listOrganization = await _context.Organization
               .Where(p => p.ParentId == null)
               .ToListAsync();
            listOrganization.Insert(0, new Organization { Id = 0, OrganizationName = "All" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "OrganizationCode", "OrganizationName");

            if (Org == null || Org == "") Org = "0";
            // Geo CLCCM
            var dataCLCCM = await missonEODRepository.GetGeoCLCCM();
            var lstGeoCLCCM = new GeoClearance();
            lstGeoCLCCM.Type = "FeatureCollection";
            var featuresCLCCM = new List<Models.GeoClearance.Feature>();

            foreach (var results in dataCLCCM)
            {
                var feature = new Models.GeoClearance.Feature();
                var pro = new Models.GeoClearance.Properties();
                var geo = new Models.GeoClearance.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.reporting_org_name = results.Reporting_org_name;
                pro.Reporting_team = results.Reporting_team;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Enddate = results.Enddate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo = JsonConvert.DeserializeObject<Models.GeoClearance.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCLCCM.Add(feature);
                i = ++i;
            }

            lstGeoCLCCM.Features = featuresCLCCM;
            ViewData["lstGeoCLCCM"] = JsonConvert.SerializeObject(lstGeoCLCCM);

            // Geo CLC TM
            var dataCLCTM= await missonEODRepository.GetGeoCLCTM();
            var lstGeoCLCTM = new GeoClearance();
            lstGeoCLCTM.Type = "FeatureCollection";
            var featuresCLCTM = new List<Models.GeoClearance.Feature>();

            foreach (var results in dataCLCTM)
            {
                var feature = new Models.GeoClearance.Feature();
                var pro = new Models.GeoClearance.Properties();
                var geo = new Models.GeoClearance.Geometry();

                pro.Lr_ID = results.Lr_ID;
                pro.Lr_name = results.Lr_name;
                pro.reporting_org_name = results.Reporting_org_name;
                pro.Reporting_team = results.Reporting_team;
                pro.Status = results.Status;
                pro.Startdate = results.Startdate;
                pro.Enddate = results.Enddate;
                pro.Areasize = results.Areasize;
                pro.District_name = results.District_name;
                pro.Commune_name = results.Commune_name;
                pro.Village_name = results.Village_name;

                geo = JsonConvert.DeserializeObject<Models.GeoClearance.Geometry>(results.Polygon);
                feature.Id = i;
                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;
                featuresCLCTM.Add(feature);
                i = ++i;
            }

            lstGeoCLCTM.Features = featuresCLCTM;
            ViewData["lstGeoCLCTM"] = JsonConvert.SerializeObject(lstGeoCLCTM);


            if (lang == "vi")
            {
                var jsonDataCHAByDistrict = await dataFiguresRepository.GetDataCLCByDistrict(Org);
                ViewData["DataCLCByDistrict"] = JsonConvert.SerializeObject(jsonDataCHAByDistrict);
            }
            else
            {
                var jsonDataCHAByDistrict = await dataFiguresRepository.GetDataCLCByDistrictEN(Org);
                ViewData["DataCLCByDistrict"] = JsonConvert.SerializeObject(jsonDataCHAByDistrict);
            }

            var jsonDataCLCByYear = await dataFiguresRepository.GetDataCLCByYear(Org);
            ViewData["DataCLCByYear"] = JsonConvert.SerializeObject(jsonDataCLCByYear);

            var DataUXOModel = await dataFiguresRepository.UXOModelClearance(Org);          

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_MineActionClearance", DataUXOModel);

            return View(DataUXOModel);
        }

        public async Task<IActionResult> ByMineActionMRE(string org)
        {
            ViewBag.NavClassByMineAction = "active";

            var listOrganization = await _context.Organization
                .Where(p => p.ParentId == null)
                .ToListAsync();
            listOrganization.Insert(0, new Organization { Id = 0, OrganizationName = "All" });
            ViewData["listOrganization"] = new MultiSelectList(listOrganization, "OrganizationCode", "OrganizationName");

            if (org == null || org == "") org = "0";

            var jsonDataMREByYear = await dataFiguresRepository.GetMREByYear(org);
            ViewData["DataMREByYear"] = JsonConvert.SerializeObject(jsonDataMREByYear);

            ViewData["MREGenderMale"] = await dataFiguresRepository.GetMREGenderMale(org);
            ViewData["MREGenderFemale"] = await dataFiguresRepository.GetMREGenderFemale(org);
            ViewData["MREChild"] = await dataFiguresRepository.GetMREChild(org);
            ViewData["MRETotal"] = await dataFiguresRepository.GetMRETotal(org);

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_MineActionMRE");

            return View();
        }
        public async Task<IActionResult> ByMineActionVictim()
        {
            ViewBag.NavClassByMineAction = "active";
            return View();
        }
        public async Task<IActionResult> ByMineActionVictimAssisstance()
        {
            ViewBag.NavClassByMineAction = "active";
            return View();
        }
        public async Task<IActionResult> ByMineActionPostClc()
        {
            ViewBag.NavClassByMineAction = "active";
            return View();
        }

        public async Task<IActionResult> ByMineActionOther()
        {
            ViewBag.NavClassByMineAction = "active";
            return View();
        }
    }
}