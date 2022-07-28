using Newtonsoft.Json;
using System.Collections.Generic;

namespace DBCU_WebApp.Models.GeoModel
{
    public class GeoOperationPlan
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }


    public class Properties
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("activity")]
        public string Activity { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("taskID")]
        public string TaskID { get; set; }


        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

    }

    public class Geometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<float?> Coordinates { get; set; }
    }

    public class Feature
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }


}
