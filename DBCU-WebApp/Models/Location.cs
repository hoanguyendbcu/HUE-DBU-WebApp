using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Models
{
    public class Location
    {
        [Display(Name = "Province")]
        [StringLength(50)]
        public string ProvinceID { get; set; }

        [Display(Name = "District")]
        [StringLength(50)]
        public string DistictID { get; set; }

        [Display(Name = "Commune")]
        [StringLength(50)]
        public string CommuneID { get; set; }

        [Display(Name = "Village")]
        [StringLength(50)]
        public string VillageID { get; set; }
        [ForeignKey("ProvinceID")]
        public Gazetteer Province { set; get; }

        [ForeignKey("DistictID")]
        public Gazetteer District { set; get; }

        [ForeignKey("CommuneID")]
        public Gazetteer Commune { set; get; }

        [ForeignKey("VillageID")]
        public Gazetteer Village { set; get; }
    }
}
