using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendaiBusSearchAPI.Models
{
    /// <summary>
    /// 駅情報を示します。
    /// </summary>
    public class StationInfoResult
    {
        /// <summary>
        /// 駅IDを示します。このIDはすべての駅に一意に割り当てられます。
        /// </summary>
        [JsonProperty("id")]
        public string StationId { get; set; }

        /// <summary>
        /// 駅名を示します。
        /// </summary>
        [JsonProperty("name")]
        public string StationName { get; set; }

        /// <summary>
        /// 駅名の読みを示します。
        /// </summary>
        [JsonProperty("yomi")]
        public string StationYomi { get; set; }

        /// <summary>
        /// この駅から発車するバス一覧を示します。
        /// </summary>
        [JsonProperty("timetable")]
        public List<BusDeptTimeInfoWithLineName> Buses { get; set; }

        /// <summary>
        /// この駅の位置情報を示します。
        /// </summary>
        [JsonProperty("coord")]
        public Coordinate Coordinates { get; set; }


    }

    /// <summary>
    /// バス情報を示します。
    /// </summary>
    public class BusInfoResult
    {

        /// <summary>
        /// バスIDを示します。このIDはすべてのバスに一意に割り当てられます。
        /// </summary>
        [JsonProperty("id")]
        public string BusId { get; set; }

        /// <summary>
        /// 路線IDを示します。
        /// </summary>
        [JsonProperty("line_id")]
        public string LineId { get; set; }
    
        /// <summary>
        /// 路線番号を示します。
        /// </summary>
        [JsonProperty("line_number")]
        public int LineNumber { get; set; }

        /// <summary>
        /// 路線名を示します。
        /// </summary>
        [JsonProperty("line_name")]
        public string LineName { get; set; }

        /// <summary>
        /// このバスの停車駅一覧とその時刻を示します。
        /// </summary>
        [JsonProperty("dept_times")]
        public List<StationsDeptInfo> DeptTimes { get; set; }

    }

    /// <summary>
    /// 路線情報を示します。
    /// </summary>
    public class LineInfoResult
    {

        /// <summary>
        /// 路線IDを示します。このIDはすべての路線に一意に割り当てられます。
        /// </summary>
        [JsonProperty("id")]
        public string LineId { get; set; }

        /// <summary>
        /// 路線番号を示します。
        /// </summary>
        [JsonProperty("numer")]
        public int LineNumber { get; set; }

        /// <summary>
        /// 路線名を示します。
        /// </summary>
        [JsonProperty("name")]
        public string LineName { get; set; }

        /// <summary>
        /// この路線に含まれるバスIDの一覧を示します。
        /// </summary>
        [JsonProperty("buses")]
        public List<string> Buses { get; set; }

        /// <summary>
        /// この路線に含まれる駅IDの一覧を示します。
        /// </summary>
        [JsonProperty("stations")]
        public List<StationIdNamePair> Stations { get; set; }

    }

    /// <summary>
    /// 経路検索情報を示します。
    /// </summary>
    public class RouteSearchResult
    {
        /// <summary>
        /// 出発駅を示します。
        /// </summary>
        [JsonProperty("from")]
        public StationIdNamePair FromStation { get; set; }

        /// <summary>
        /// 到着駅を示します。
        /// </summary>
        [JsonProperty("to")]
        public StationIdNamePair ToStation { get; set; }

        /// <summary>
        /// 検索条件として指定された時間を示します。
        /// </summary>
        [JsonProperty("query_time")]
        public string QueryTime { get; set; }

        /// <summary>
        /// 検索条件として指定された運行日を示します。
        /// </summary>
        [JsonProperty("query_daytype")]
        public DayType QueryDayType { get; set; }

        /// <summary>
        /// 検索条件として指定された、経路検索の検索方法を示します。
        /// </summary>
        [JsonProperty("query_method")]
        public RouteSeachMethod QueryMethod { get; set; }

        /// <summary>
        /// 経路一覧情報を示します。
        /// </summary>
        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }
        
    }


    /// <summary>
    /// 経路検索の検索方法を示します。
    /// </summary>
    public enum RouteSeachMethod
    {
        /// <summary>
        /// 出発時刻をもとに検索します。
        /// </summary>
        DepartureBase = 0,
        /// <summary>
        /// 到着時刻をもとに検索します。
        /// </summary>
        ArrivalBase = 1
    }
    
    /// <summary>
    /// 運行日を示します。
    /// </summary>
    public enum DayType
    {
        /// <summary>
        /// 平日を示します。
        /// </summary>
        weekday = 0,
        /// <summary>
        /// 土曜日を示します。
        /// </summary>
        saturday = 1,
        /// <summary>
        /// 休日を示します。
        /// </summary>
        holiday = 2
    }

    /// <summary>
    /// 経路情報を示します。
    /// </summary>
    public class Route
    {
        /// <summary>
        /// 出発時間を示します。
        /// </summary>
        [JsonProperty("dept")]
        public string TotalDeptTime { get; set; }

        /// <summary>
        /// 到着時間を示します。
        /// </summary>
        [JsonProperty("arr")]
        public string TotalArrTime { get; set; }

        /// <summary>
        /// 総移動時間を示します。
        /// </summary>
        [JsonProperty("time")]
        public string TotalTime { get; set; }

        /// <summary>
        /// 乗り換え回数を示します。
        /// </summary>
        [JsonProperty("transfer_count")]
        public int TransferCount { get; set; }

        /// <summary>
        /// 経路の提示順位を示します。
        /// </summary>
        [JsonProperty("rank")]
        public double Rank { get; set; }

        /// <summary>
        /// 経路の詳細情報（移動手順一覧）を示します。
        /// </summary>
        [JsonProperty("pathes")]
        public List<Path> Pathes { get; set; }

    }

    /// <summary>
    /// 駅の基本情報を示します。
    /// </summary>
    public class StationBasicInfo
    {

        /// <summary>
        /// 駅のIDと名前情報を示します。
        /// </summary>
        [JsonProperty("idname")]
        public StationIdNamePair IdName { get; set; }

        /// <summary>
        /// この駅の位置情報を示します。
        /// </summary>
        [JsonProperty("coord")]
        public Coordinate Coordinates { get; set; }

    }

    /// <summary>
    /// 駅の概要情報を示します。
    /// </summary>
    public class StationSummaryInfo
    {
        /// <summary>
        /// 駅の基本情報を示します。
        /// </summary>
        [JsonProperty("basic_info")]
        public StationBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// この駅を通る路線情報を示します。
        /// </summary>
        [JsonProperty("lines")]
        public LinesList Lines { get; set; }

    }

    /// <summary>
    /// 路線一覧情報を示します。
    /// </summary>
    public class LinesList
    {

        /// <summary>
        /// 平日の路線一覧情報を示します。
        /// </summary>
        [JsonProperty("weekday")]
        public List<LineNameInfo> Weekday { get; set; }

        /// <summary>
        /// 土曜の路線一覧情報を示します。
        /// </summary>
        [JsonProperty("saturday")]
        public List<LineNameInfo> Saturday { get; set; }

        /// <summary>
        /// 休日の路線一覧情報を示します。
        /// </summary>
        [JsonProperty("holiday")]
        public List<LineNameInfo> Holiday { get; set; }


    }


    /// <summary>
    /// 移動手順を示します。
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 出発点を示します。
        /// </summary>
        [JsonProperty("dept")]
        public Node DeptNode { get; set; }

        /// <summary>
        /// 到着点を示します。
        /// </summary>
        [JsonProperty("arr")]
        public Node ArrNode { get; set; }

        /// <summary>
        /// 使用する路線IDを示します。
        /// </summary>
        [JsonProperty("line")]
        public LineNameInfo Line { get; set; }

        /// <summary>
        /// 使用するバスIDを示します。
        /// </summary>
        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        /// <summary>
        /// 徒歩であるかを示します。
        /// </summary>
        [JsonProperty("method")]
        public TransferMethod Method { get; set; }
        
        /// <summary>
        /// 所要時間を示します。
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }

    }

    /// <summary>
    /// 移動手段を示します。
    /// </summary>
    public enum TransferMethod
    {
        /// <summary>
        /// 徒歩を示します。
        /// </summary>
        Walk,
        /// <summary>
        /// バスを示します。
        /// </summary>
        Bus,
        /// <summary>
        /// JR線を示します。
        /// </summary>
        JR,
        /// <summary>
        /// 地下鉄を示します。
        /// </summary>
        Subway
    }

    /// <summary>
    /// 地点情報を示します。
    /// </summary>
    public class Node
    {
     
        /// <summary>
        /// 出発/到着時間を示します。
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }   

        /// <summary>
        /// 出発/到着駅情報を示します。
        /// </summary>
        [JsonProperty("station")]
        public StationIdNamePair Station { get; set; }

    }

    /// <summary>
    /// 簡易的な路線情報を示します。
    /// </summary>
    public class LineNameInfo
    {
        /// <summary>
        /// 路線IDを示します。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 路線番号を示します。
        /// </summary>
        [JsonProperty("number")]
        public int Number { get; set; }

        /// <summary>
        /// 路線名を示します。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    /// <summary>
    /// 簡易的な駅情報を示します。
    /// </summary>
    public class StationIdNamePair
    {
        /// <summary>
        /// 駅IDを示します。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// 駅の読みを示します。
        /// </summary>
        [JsonProperty("yomi")]
        public string Yomi { get; set; }

        /// <summary>
        /// 駅名を示します。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    /// <summary>
    /// バスの出発時刻とその路線名を示します。
    /// </summary>
    public class BusDeptTimeInfoWithLineName
    {
        /// <summary>
        /// バスIDを示します
        /// </summary>
        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        /// <summary>
        /// 路線IDを示します。
        /// </summary>
        [JsonProperty("line_id")]
        public string LineId { get; set; }

        /// <summary>
        /// 路線名を示します。
        /// </summary>
        [JsonProperty("line_name")]
        public string LineName { get; set; }

        /// <summary>
        /// 路線番号を示します。
        /// </summary>
        [JsonProperty("line_number")]
        public int LineNumber { get; set; }

        /// <summary>
        /// 出発時刻を示します。
        /// </summary>
        [JsonProperty("dept")]
        public string DeptTime { get; set; }

        /// <summary>
        /// 運行日を示します。
        /// </summary>
        [JsonProperty("daytype")]
        public DayType DayType { get; set; }

    }

}
