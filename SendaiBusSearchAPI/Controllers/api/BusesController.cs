using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SendaiBusSearchAPI.Models;
using System.Web.Http.Cors;

namespace SendaiBusSearchAPI.Controllers.api
{
    /// <summary>
    /// バス情報に関するAPIを提供します。
    /// </summary>
    [Route("buses")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BusesControllerBase : ApiController
    {

        /// <summary>
        /// バス詳細情報を検索します。
        /// </summary>
        /// <param name="id">一意のバスIDを指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("details")]
        public BusInfoResult GetDetailsData(string id)
        {
            var instance = DBModel.GetInstance();
            if (!instance.Buses.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var temp = instance.Buses[id];
            var tempLine = instance.Lines.GetDataFromDayType(temp.DayType)[temp.LineId];
            
            var result = new BusInfoResult()
            {
                BusId = id,
                LineId = temp.LineId,
                DeptTimes = (from t in temp.DeptTimes let st = instance.Stations[t.StationId] select new StationsDeptInfo()
                {
                    Station = new StationIdNamePair() { Id = t.StationId, Name = st.Name, Yomi = st.Yomi },
                    DeptTime = t.DeptTime
                }).ToList(),
                LineNumber = tempLine.Number,
                LineName = tempLine.Name
            };

            return result;

        }



    }
}
        