using SendaiBusSearchAPI.Models;
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
    [RoutePrefix("api/stations")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StationsController : ApiController
    {
        /// <summary>
        /// 駅詳細情報を検索します。
        /// </summary>
        /// <param name="id">一意の駅IDを指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("details")]
        public StationInfoResult GetDetailsData(string id)
        {
            var instance = DBModel.GetInstance();

            if (!instance.Stations.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Stations[id];

            var result = new StationInfoResult()
            {
                Buses = (from bus in temp.Buses let key = instance.Buses[bus.BusId].LineId let tl = instance.Lines.GetDataFromDayType(bus.DayType)[key]
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
                StationName = temp.Name
            };

            return result;
        }

        /// <summary>
        /// 名前から駅IDを検索します。
        /// </summary>
        /// <param name="name">駅名を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search")]
        public List<StationIdNamePair> SearchStationId(string name)
        {
            // 純粋にcontainsで
            var instance = DBModel.GetInstance();

            var temp = (from item in instance.Stations where item.Value.Name.Contains(name) select new StationIdNamePair() { Id = item.Key, Name = item.Value.Name }).ToList();
            return temp;
        }

    }
}
