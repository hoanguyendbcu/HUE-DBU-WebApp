using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DBCU_WebApp.Models.BomBing
{
    public class QBBomBing3KMPoint
    {
        public double LATITUDE;
        public double LONGITUDE;
        public string AIRCRAFT;
        public string ORDNANCE;
        public string ORD_CLASS;
        public string CATEGORY;
    }
    public class Properties
    {
        [JsonPropertyName("mag")]
        public int Mag { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
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

    public class QBBomBing3KM
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }
}
