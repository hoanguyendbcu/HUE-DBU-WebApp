using DBCU_WebApp.Models.BomBing;
using DBCU_WebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBCU_WebApp.Controllers
{
    public class BomBingQBController : Controller
    {
        private readonly DBCU_WebContext _context;
        private readonly MissonEODRepository missonEODRepository;

        public BomBingQBController(DBCU_WebContext context, IConfiguration configuration)
        {
            _context = context;
            missonEODRepository = new MissonEODRepository(configuration);
        }
        public async Task<IActionResult> IndexAsync()
        {
            var lst = await missonEODRepository.GetDataBomBingQB();

            var qbBom = new QBBomBing3KM();
            qbBom.Type = "FeatureCollection";

            var features = new List<Feature>();

            foreach (var results in lst)
            {
                var feature = new Feature();
                var pro = new Properties();
                pro.Mag = 1;

                var geo = new Geometry();
                geo.Type = "Point";
                geo.Coordinates = new List<double> { results.LONGITUDE, results.LATITUDE };

                feature.Type = "Feature";
                feature.Properties = pro;
                feature.Geometry = geo;

                features.Add(feature);
            }
            qbBom.Features = features;

            ViewData["GetDataBomBingQB"] = JsonSerializer.Serialize(qbBom);

            return View();
        }
    }
}
