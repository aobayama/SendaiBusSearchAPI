using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendaiBusSearchAPI.Models
{

    public class StationInfoResult
    {

        [JsonProperty("id")]
        public string StationId { get; set; }

        [JsonProperty("name")]
        public string StationName { get; set; }

        [JsonProperty("timetable")]
        public List<BusDeptTimeInfoWithLineName> Buses { get; set; }

    }


    public class BusInfoResult
    {

        [JsonProperty("id")]
        public string BusId { get; set; }

        [JsonProperty("line_id")]
        public string LineId { get; set; }

        [JsonProperty("line_name")]
        public string LineName { get; set; }

        [JsonProperty("dept_times")]
        public List<StationsDeptInfo> DeptTimes { get; set; }

    }

    public class LineInfoResult
    {

        [JsonProperty("id")]
        public string LineId { get; set; }

        [JsonProperty("name")]
        public string LineName { get; set; }

        [JsonProperty("buses")]
        public List<BusDayTypeInfo> Buses { get; set; }


        [JsonProperty("stations")]
        public List<IdNamePair> Stations { get; set; }

    }

    public class RouteSearchResult
    {



    }

    public class Route
    {

        [JsonProperty("dept")]
        public string TotalDeptTime { get; set; }

        [JsonProperty("arr")]
        public string TotalArrTime { get; set; }

        [JsonProperty("time")]
        public string TotalTime { get; set; }

        [JsonProperty("transfer_count")]
        public string TransferCount { get; set; }

        [JsonProperty("cost")]
        public double Cost { get; set; }

        [JsonProperty("pathes")]
        public List<Path> Pathes { get; set; }

    }

    public class Path
    {

        [JsonProperty("dept")]
        public Node DeptNode { get; set; }

        [JsonProperty("arr")]
        public Node ArrNode { get; set; }

        [JsonProperty("line")]
        public IdNamePair Line { get; set; }

        [JsonProperty("is_walk")]
        public bool IsWalk { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

    }

    public class Node
    {
     
        [JsonProperty("date")]
        public string Time { get; set; }   

        [JsonProperty("station")]
        public IdNamePair Station { get; set; }

    }


    public class IdNamePair
    {

        [JsonProperty("id")]
        public string StationId { get; set; }

        [JsonProperty("name")]
        public string StationName { get; set; }

    }

    public class BusDeptTimeInfoWithLineName
    {
        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        [JsonProperty("line_name")]
        public string RouteName { get; set; }

        [JsonProperty("line_id")]
        public string RouteId { get; set; }

        [JsonProperty("dept")]
        public string DeptTime { get; set; }

        [JsonProperty("daytype")]
        public string DayType { get; set; }

    }

}
