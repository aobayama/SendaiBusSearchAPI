//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SendaiBusSearchAPI.Models.SystemModels
//{
//    public class BaseData
//    {

//        public Dictionary<string, Stations> Stations { get; set; }
//        public Dictionary<string, Route> Routes { get; set; }
//        public Dictionary<string, Bus> Buses { get; set; }
//    }

//    public class Stations
//    {
//        public string Name { get; set; }
//        public List<BusDeptTimeInfo> Buses { get; set; }
//    }

//    public class Route
//    {
//        public List<RouteBusInfo> Buses { get; set; }
//        public List<Stations> Stations { get; set; }
//    }

//    public class Bus
//    {

//        public Route Route { get; set; }

//        public List<StationDeptTimeInfo> DeptTimes { get; set; }

//    }

//    public class StationDeptTimeInfo
//    {
//        public Stations StationId { get; set; }

//        public DateTime DeptTime { get; set; }

//        public DayType DayType { get; set; }

//    }


//    public class RouteBusInfo
//    {
//        public Bus BusId { get; set; }

//        public DayType DayType { get; set; }

//    }

//    public class BusDeptTimeInfo
//    {

//        public Bus BusId { get; set; }

//        public DateTime DeptTime { get; set; }

//        public DayType DayType { get; set; }
        
//    }

//    public enum DayType
//    {
//        Weekday = 0,
//        Saturday = 1,
//        Holiday = 2
//    }
//}
