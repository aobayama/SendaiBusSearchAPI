using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        const string DATETIMEPATTERN = @"yyyy/MM/dd HH\:mm\:ss";

        const string TIMESPANPATTERN = @"hh\:mm";

        public static DateTime ConvertToDateTime(string value)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(value, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return new DateTime();
            }
        }

        public static TimeSpan ConvertToTimeSpan(string value)
        {
            TimeSpan parsedDate;
            if (TimeSpan.TryParse(value, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public static string ConvertToString(DateTime value)
        {
            return value.ToString(DATETIMEPATTERN);
        }

        public static string ConvertToString(TimeSpan value)
        {
            return value.ToString(TIMESPANPATTERN);
        }


        public static TimeSpan DiffTimespan(TimeSpan value1, TimeSpan value2)
        {
            if (value1 < value2)
            {
                return (value2 - value1);
            }
            else
            {
                return (value1 - value2);
            }
        }


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
        
        [JsonProperty("coord")]
        public Coordinate Coord { get; set; }

        [JsonProperty("yomi")]
        public string Yomi { get; set; }

        [JsonProperty("buses")]
        public List<BusDeptTimeInfo> Buses { get; set; }
        
    }

    /// <summary>
    /// 座標を示します。
    /// </summary>
    public class Coordinate
    {

        /// <summary>
        /// 経度を示します。
        /// </summary>
        [JsonProperty("lon")]
        public double longitude { get; set; }

        /// <summary>
        /// 緯度を示します。
        /// </summary>
        [JsonProperty("lat")]
        public double latitude { get; set; }
    }



    public class Lines
    {
        [JsonProperty(Commons.WEEKDAY)]
        public Dictionary<string,Line> Weekday { get; set; }

        [JsonProperty(Commons.SATURDAY)]
        public Dictionary<string, Line> Saturday { get; set; }

        [JsonProperty(Commons.HOLIDAY)]
        public Dictionary<string,Line> Holiday { get; set; }

        public Dictionary<string, Line> GetDataFromDayType(DayType dayType)
        {
            switch (dayType)
            {
                case DayType.saturday:
                    return this.Saturday;
                case DayType.holiday:
                    return this.Holiday;
                default:
                    return this.Weekday;
            }
        }

    }

    public class Line
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("buses")]
        public List<string> Buses { get; set; }

        [JsonProperty("stations")]
        public List<string> Stations { get; set; }

    }

    public class Bus
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("daytype")]
        public DayType DayType { get; set; }

        [JsonProperty("line_id")]
        public string LineId { get; set; }

        [JsonProperty("dept_times")]
        public List<StationsDept> DeptTimes { get; set; }
        
    }

    /// <summary>
    /// 駅出発時刻情報を示します。
    /// </summary>
    public class StationsDeptInfo
    {
        /// <summary>
        /// 駅情報を示します。
        /// </summary>
        [JsonProperty("station")]
        public StationIdNamePair Station { get; set; }

        /// <summary>
        /// 発車時刻を示します。
        /// </summary>
        [JsonProperty("dept")]
        public string DeptTime { get; set; }

    }


    public class StationsDept
    {

        [JsonProperty("station_id")]
        public string StationId { get; set; }

        [JsonProperty("dept")]
        public string DeptTime { get; set; }

    }

    public class BusDeptTimeInfo
    {

        [JsonProperty("bus_id")]
        public string BusId { get; set; }

        [JsonProperty("dept")]
        public string DeptTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("daytype")]
        public DayType DayType { get; set; }

    }

}
