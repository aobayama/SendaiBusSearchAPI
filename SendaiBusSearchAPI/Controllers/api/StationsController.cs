using SendaiBusSearchAPI.Models;
using SendaiBusSearchAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SendaiBusSearchAPI.Controllers.api
{
    /// <summary>
    /// 駅情報に関するAPIを提供します。
    /// </summary>
    [Route("stations")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StationsControllerBase : ApiController
    {
        /// <summary>
        /// 駅詳細情報を検索します。
        /// </summary>
        /// <param name="id">一意の駅IDを指定します。</param>
        /// <param name="line_id">（オプション）一意の路線IDを指定して、時刻表データを制限します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("details")]
        public virtual StationInfoResult GetDetailsData(string id, string line_id = null)
        {
            var instance = DBModel.GetInstance();

            if (!instance.Stations.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Stations[id];

            if (line_id != null && line_id != string.Empty)
            {
                var result = new StationInfoResult()
                {
                    Buses = (from bus in temp.Buses
                             let key = instance.Buses[bus.BusId].LineId
                             where key == line_id
                             let tl = instance.Lines.GetDataFromDayType(bus.DayType)[key]
                             select new BusDeptTimeInfoWithLineName()
                             {
                                 BusId = bus.BusId,
                                 DayType = bus.DayType,
                                 DeptTime = bus.DeptTime,
                                 LineId = key,
                                 LineNumber = tl.Number,
                                 LineName = tl.Name
                             }).ToList(),
                    StationId = id,
                    Coordinates = temp.Coord,
                    StationName = temp.Name,
                    StationYomi = temp.Yomi
                };

                return result;
            }
            else
            {
                var result = new StationInfoResult()
                {
                    Buses = (from bus in temp.Buses
                             let key = instance.Buses[bus.BusId].LineId
                             let tl = instance.Lines.GetDataFromDayType(bus.DayType)[key]
                             select new BusDeptTimeInfoWithLineName()
                             {
                                 BusId = bus.BusId,
                                 DayType = bus.DayType,
                                 DeptTime = bus.DeptTime,
                                 LineId = key,
                                 LineNumber = tl.Number,
                                 LineName = tl.Name
                             }).ToList(),
                    StationId = id,
                    Coordinates = temp.Coord,
                    StationName = temp.Name,
                    StationYomi = temp.Yomi
                };

                return result;
            }

        }

        /// <summary>
        /// 駅概要情報を検索します。
        /// </summary>
        /// <param name="id">一意の駅IDを指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("summary")]
        public virtual StationSummaryInfo GetSummaryData(string id)
        {
            var instance = DBModel.GetInstance();

            if (!instance.Stations.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Stations[id];
            var result = new StationSummaryInfo()
            {
                BasicInfo = new StationBasicInfo()
                {
                    IdName = new StationIdNamePair()
                    {
                        Id = id,
                        Name = temp.Name,
                        Yomi = temp.Yomi
                    },
                    Coordinates = temp.Coord
                },
                Lines = new LinesList()
                {
                    Weekday = (from item in instance.Lines.Weekday where item.Value.Stations.Contains(id) select new LineNameInfo() { Id = item.Key, Name = item.Value.Name, Number = item.Value.Number }).ToList(),
                    Saturday = (from item in instance.Lines.Saturday where item.Value.Stations.Contains(id) select new LineNameInfo() { Id = item.Key, Name = item.Value.Name, Number = item.Value.Number }).ToList(),
                    Holiday = (from item in instance.Lines.Holiday where item.Value.Stations.Contains(id) select new LineNameInfo() { Id = item.Key, Name = item.Value.Name, Number = item.Value.Number }).ToList(),
                }
            };
            
            return result;
        }

        /// <summary>
        /// すべての駅の基本情報を取得します。
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("list")]
        public virtual List<StationBasicInfo> GetAllBasicData()
        {
            var instance = DBModel.GetInstance();

            var result = (from station in instance.Stations
                         select new StationBasicInfo()
                         {
                             IdName = new StationIdNamePair() { Id = station.Key, Name = station.Value.Name, Yomi = station.Value.Yomi },
                             Coordinates = station.Value.Coord
                         }).ToList();
            
            return result;
            
        }


        /// <summary>
        /// 名前または読みから駅IDを検索します。
        /// </summary>
        /// <param name="name">駅名を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("search")]
        public virtual List<StationIdNamePair> SearchStationId(string name)
        {
            // 純粋にcontainsで
            var instance = DBModel.GetInstance();

            var temp = (from item in instance.Stations where (item.Value.Name.Contains(name) || item.Value.Yomi.Contains(name)) select new StationIdNamePair() { Id = item.Key, Name = item.Value.Name, Yomi = item.Value.Yomi }).ToList();
            return temp;
        }

    }
}
