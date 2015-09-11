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
    /// 路線情報に関するAPIを提供します。
    /// </summary>
    [RoutePrefix("api/lines")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LinesController : ApiController
    {
        /// <summary>
        /// 路線詳細情報を検索します。
        /// </summary>
        /// <param name="id">一意の路線IDを指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("details")]
        public LineInfoResult GetDetailsData(string id, DayType daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = instance.Lines.GetDataFromDayType(daytype);
            
            if (!tempLineCollection.ContainsKey(id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }


            var tempLine = tempLineCollection[id];

            var result = new LineInfoResult()
            {
                LineId = id,
                LineNumber = tempLine.Number,
                LineName = tempLine.Name,
                Buses = tempLine.Buses,
                Stations = (from sta in tempLine.Stations select new StationIdNamePair() { Id = sta, Name = instance.Stations[sta].Name }).ToList()
            };

            return result;

        }

        /// <summary>
        /// 路線番号から路線IDを検索します。
        /// </summary>
        /// <param name="number">路線番号を指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search_from_number")]
        public List<LineNameInfo> GetLineIdsFromNumber(int number, DayType daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = instance.Lines.GetDataFromDayType(daytype);
            var temp = (from t in tempLineCollection where t.Value.Number == number select new LineNameInfo { Number = t.Value.Number, Id = t.Key, Name = t.Value.Name }).ToList();
            return temp;
        }

        /// <summary>
        /// 路線名から路線IDを検索します。
        /// </summary>
        /// <param name="name">路線名を指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search_from_name")]
        public List<LineNameInfo> GetLinesKeyFromName(string name, DayType daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = instance.Lines.GetDataFromDayType(daytype);
            var temp = (from t in tempLineCollection where t.Value.Name.Contains(name) select new LineNameInfo() { Id = t.Key, Number = t.Value.Number, Name = t.Value.Name }).ToList();
            return temp;
        }

    }
}
