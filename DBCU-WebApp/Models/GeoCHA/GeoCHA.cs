using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace DBCU_WebApp.Models.GeoCHA
{
    public class GeoCHA
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
        public string Reporting_org_name { get; set; }
        [JsonPropertyName("reporting_Org_localid")]
        public string Reporting_Org_localid { get; set; }
        [JsonPropertyName("survey_method")]
        public string Survey_method { get; set; }
        [JsonPropertyName("Land_Use")]
        public string Land_Use { get; set; }
        [JsonPropertyName("comment_Land_Use")]
        public string Comment_Land_Use { get; set; }
        [JsonPropertyName("clearance_Priority")]
        public string Clearance_Priority { get; set; }
        [JsonPropertyName("type_of_Area")]
        public string Type_of_Area { get; set; }
        [JsonPropertyName("vehicle_Type")]
        public string Vehicle_Type { get; set; }
        [JsonPropertyName("vegetation_removed")]
        public string Vegetation_removed { get; set; }
        [JsonPropertyName("soiltype")]
        public string Soiltype { get; set; }
        [JsonPropertyName("vegetation_Type")]
        public string Vegetation_Type { get; set; }
        [JsonPropertyName("vegetation_density")]
        public string Vegetation_density { get; set; }
        [JsonPropertyName("slopee")]
        public string Slopee { get; set; }
        [JsonPropertyName("beneficiaries")]
        public string Beneficiaries { get; set; }
        [JsonPropertyName("soil_Condition")]
        public string Soil_Condition { get; set; }
        [JsonPropertyName("additional_Information")]
        public string Additional_Information { get; set; }


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