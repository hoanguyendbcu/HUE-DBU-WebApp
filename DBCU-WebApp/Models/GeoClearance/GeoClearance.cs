using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DBCU_WebApp.Models.GeoClearance
{
    public class GeoClearance
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }
    public class Properties
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("lr_id")]
        public string Lr_ID { get; set; }

        [JsonPropertyName("lr_name")]
        public string Lr_name { get; set; }

        [JsonPropertyName("reporting_team")]
        public string Reporting_team { get; set; }

        [JsonPropertyName("reporting_org_name")]
        public string reporting_org_name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("startdate")]
        public string Startdate { get; set; }

        [JsonPropertyName("enddate")]
        public string Enddate { get; set; }

        [JsonPropertyName("areasize")]
        public string Areasize { get; set; }

        [JsonPropertyName("village_name")]
        public string Village_name { get; set; }

        [JsonPropertyName("commune_name")]
        public string Commune_name { get; set; }

        [JsonPropertyName("district_name")]
        public string District_name { get; set; }
 
    }

    public class Geometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<List<List<List<double>>>> Coordinates { get; set; }

    }

    public class Feature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class GeoClearanceData
    {
     
        public string Lr_ID { get; set; }
        public string Lr_name { get; set; }
        public string Reporting_team { get; set; }
        public string Reporting_org_name { get; set; }
        public string Status { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public string Areasize { get; set; }
        public string Village_name { get; set; }
        public string Commune_name { get; set; }
        public string District_name { get; set; }
        public string Polygon { get; set; }
 
    }

   
}
