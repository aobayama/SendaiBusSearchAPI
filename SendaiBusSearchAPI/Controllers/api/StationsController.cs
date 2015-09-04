using SendaiBusSearchAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SendaiBusSearchAPI.Controllers.api
{
    [RoutePrefix("api/stations")]
    public class StationsController : ApiController
    {
        [Route("details")]
        public StationInfoResult GetDetailsData(int id)
        {
            var instance = DBModel.GetInstance();

            if (!instance.Stations.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Stations[id];

            var result = new StationInfoResult()
            {
                Buses = (from bus in temp.Buses let key = instance.Buses[bus.BusId].LineKey let tl = instance.Lines.GetDataFromDayType(bus.DayType)[key]
                         select new BusDeptTimeInfoWithLineName()
                {
                    BusId = bus.BusId,
                    DayType = bus.DayType,
                    DeptTime = bus.DeptTime,
                    LineKey = key,
                    LineId = tl.Id,
                    LineName = tl.Name
                }).ToList(),
                StationId = id,
                StationName = temp.Name
            };

            return result;
        }

        [Route("search")]
        public List<IdNamePair> SearchStationId(string name)
        {
            // 純粋にcontainsで
            var instance = DBModel.GetInstance();

            var temp = (from item in instance.Stations where item.Value.Name.Contains(name) select new IdNamePair() { Id = item.Key, Name = item.Value.Name }).ToList();
            return temp;
        }

    }
}
