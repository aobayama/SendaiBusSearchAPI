﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendaiBusSearchAPI.Models
{

    public static class Commons
    {
        public const string WEEKDAY = "weekday";

        public const string SATURDAY = "saturday";

        public const string HOLIDAY = "holiday";

    }

    public class JsonData
    {

        [JsonProperty("stations")]
        public Dictionary<string, Station> Stations { get; set; }

        [JsonProperty("lines")]
        public Lines Lines { get; set; }

        [JsonProperty("buses")]
        public Dictionary<string, Bus> Buses { get; set; }

    }

    public class Station
    {

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("buses")]
        public List<BusDeptTimeInfo> Buses { get; set; }

    }


    public class Lines
    {
        [JsonProperty(Commons.WEEKDAY)]
        public Dictionary<string,Line> Weekday { get; set; }

        [JsonProperty(Commons.SATURDAY)]
        public Dictionary<string, Line> Saturday { get; set; }

        [JsonProperty(Commons.HOLIDAY)]
        public Dictionary<string,Line> Holiday { get; set; }

    }

    public class Line
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("buses")]
        public List<BusDayTypeInfo> Buses { get; set; }

        [JsonProperty("stations")]
        public List<string> Stations { get; set; }

    }

    public class Bus
    {

        [JsonProperty("daytype")]
        public string DayType { get; set; }

        [JsonProperty("line_key")]
        public string LineKey { get; set; }

        [JsonProperty("dept_times")]
        public List<StationsDeptInfo> DeptTimes { get; set; }

    }

    
    public class StationsDeptInfo
    {
        
        [JsonProperty("station_id")]
        public string StationId { get; set; }
        
        [JsonProperty("dept")]
        public string DeptTime { get; set; }

    }


    public class BusDayTypeInfo
    {
        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        [JsonProperty("daytype")]
        public string DayType { get; set; }

    }

    public class BusDeptTimeInfo
    {

        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        [JsonProperty("dept")]
        public string DeptTime { get; set; }

        [JsonProperty("daytype")]
        public string DayType { get; set; }

    }

}
