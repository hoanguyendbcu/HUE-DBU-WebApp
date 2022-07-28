using Newtonsoft.Json;
using System.Collections.Generic;

namespace DBCU_WebApp.Models.GeoClearance
{
    public class GeoClearance
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }
    public class Properties
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lr_id")]
        public string Lr_ID { get; set; }

        [JsonProperty("lr_name")]
        public string Lr_name { get; set; }

        [JsonProperty("reporting_team")]
        public string Reporting_team { get; set; }

        [JsonProperty("reporting_org_name")]
        public string reporting_org_name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("startdate")]
        public string Startdate { get; set; }

        [JsonProperty("enddate")]
        public string Enddate { get; set; }

        [JsonProperty("areasize")]
        public string Areasize { get; set; }

        [JsonProperty("village_name")]
        public string Village_name { get; set; }

        [JsonProperty("commune_name")]
        public string Commune_name { get; set; }

        [JsonProperty("district_name")]
        public string District_name { get; set; }
 
    }

    public class Geometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<List<List<List<double>>>> Coordinates { get; set; }

    }

    public class Feature
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("id")]
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
