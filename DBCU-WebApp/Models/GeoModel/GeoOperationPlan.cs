using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DBCU_WebApp.Models.GeoModel
{
    public class GeoOperationPlan
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }


    public class Properties
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("org")]
        public string Org { get; set; }

        [JsonPropertyName("team")]
        public string Team { get; set; }

        [JsonPropertyName("activity")]
        public string Activity { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("taskID")]
        public string TaskID { get; set; }


        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

    }

    public class Geometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<float?> Coordinates { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }


}
