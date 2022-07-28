using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DBCU_WebApp.Controllers
{
    public interface IDataFiguresController
    {
        Task<IActionResult> BaDonTown();
        Task<IActionResult> BoTrachDistrict();
        Task<IActionResult> ByMineActionClearance();
        Task<IActionResult> ByMineActionSurvey();
        Task<IActionResult> DongHoiCity();
        Task<JsonResult> GetCommune(string DistictID);
        Task<JsonResult> GetVillage(string CommuneID);
        Task<IActionResult> Index();
        Task<IActionResult> LeThuyDistrict();
        Task<IActionResult> MinhHoaDistrict();
        Task<IActionResult> QuangBinhProvince();
        Task<IActionResult> QuangNinhDistrict();
        Task<IActionResult> QuangTrachDistrict();
        Task<IActionResult> TuyenHoaDistrict();
    }
}