using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Models.DataFigures
{
    public class GetDataCHA
    {
        public int? AreaSize { get; set; }
        public string Status { get; set; }

    }
    public class  LinechartCHAByYear
    {
        public int Year { get; set; }
        public int Areasize { get; set; }
    }
        public class LinechartCHAByDistrict
    {
        public string District { get; set; }
        public string Color { get; set; }
        public int Areasize { get; set; }
    }
    public class LinechartCHAByStatus
    {
        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("closed")]
        public int Closed { get; set; }

        [JsonProperty("open")]
        public int Open { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("color2")]
        public string Color2 { get; set; }
    }

}
