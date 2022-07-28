using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Models.DataFigures
{
    public class ByProvince
    {
    }
    public class UXOCategory
    {
        public string category { get; set; }
        public int qty { get; set; }
        public string backgroundColor { get; set; }
    }
    public class MREGender
    {
        public string category { get; set; }
        public int qty { get; set; }
        public string backgroundColor { get; set; }
    }

    public class DataClearnceChart
    {
        public string Areasize { get; set; }
        public string Year { get; set; }
    }
    public class DataClearnceChart2
    {
        public string year { get; set; }
        public int areasize { get; set; }
        public string color { get; set; }
    }

}
