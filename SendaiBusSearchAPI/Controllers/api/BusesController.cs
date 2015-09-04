using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SendaiBusSearchAPI.Models;

namespace SendaiBusSearchAPI.Controllers.api
{
    [RoutePrefix("api/buses")]
    public class BusesController : ApiController
    {

        public BusInfoResult GetDetailsData(string id)
        {
            var instance = DBModel.GetInstance();
            if (!instance.Buses.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Buses[id];
            var tempLine = instance.Lines.GetDataFromDayType(temp.DayType)[temp.LineKey];
            

            var result = new BusInfoResult()
            {
                BusId = id,
                LineKey = temp.LineKey,
                DeptTimes = temp.DeptTimes,
                LineId = tempLine.Id,
                LineName = tempLine.Name
            };

            return result;

        }

    }
}
        