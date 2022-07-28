using Newtonsoft.Json;
using System.Collections.Generic;


namespace DBCU_WebApp.Models.GeoCHA
{
    public class GeoCHA
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
        public string Reporting_org_name { get; set; }
        [JsonProperty("reporting_Org_localid")]
        public string Reporting_Org_localid { get; set; }
        [JsonProperty("survey_method")]
        public string Survey_method { get; set; }
        [JsonProperty("Land_Use")]
        public string Land_Use { get; set; }
        [JsonProperty("comment_Land_Use")]
        public string Comment_Land_Use { get; set; }
        [JsonProperty("clearance_Priority")]
        public string Clearance_Priority { get; set; }
        [JsonProperty("type_of_Area")]
        public string Type_of_Area { get; set; }
        [JsonProperty("vehicle_Type")]
        public string Vehicle_Type { get; set; }
        [JsonProperty("vegetation_removed")]
        public string Vegetation_removed { get; set; }
        [JsonProperty("soiltype")]
        public string Soiltype { get; set; }
        [JsonProperty("vegetation_Type")]
        public string Vegetation_Type { get; set; }
        [JsonProperty("vegetation_density")]
        public string Vegetation_density { get; set; }
        [JsonProperty("slopee")]
        public string Slopee { get; set; }
        [JsonProperty("beneficiaries")]
        public string Beneficiaries { get; set; }
        [JsonProperty("soil_Condition")]
        public string Soil_Condition { get; set; }
        [JsonProperty("additional_Information")]
        public string Additional_Information { get; set; }


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
       
    public class GeoCHAData
    {

        public string Lr_ID { get; set; }
        public string Lr_name { get; set; }
        public string Reporting_team { get; set; }
        public string Reporting_org_name { get; set; }
        public string Reporting_Org_localid { get; set; }
        public string Survey_method { get; set; }
        public string Land_Use { get; set; }
        public string Comment_Land_Use { get; set; }
        public string Clearance_Priority { get; set; }
        public string Type_of_Area { get; set; }
        public string Vehicle_Type { get; set; }
        public string Vegetation_removed { get; set; }
        public string Soiltype { get; set; }
        public string Vegetation_Type { get; set; }
        public string Vegetation_density { get; set; }
        public string Slopee { get; set; }
        public string Beneficiaries { get; set; }
        public string Soil_Condition { get; set; }
        public string Additional_Information { get; set; }
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