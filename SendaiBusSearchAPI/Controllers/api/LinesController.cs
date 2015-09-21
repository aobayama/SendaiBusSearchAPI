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
    /// 路線情報に関するAPIを提供します。
    /// </summary>
    [Route("lines")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LinesControllerBase : ApiController
    {
        /// <summary>
        /// 路線詳細情報を検索します。
        /// </summary>
        /// <param name="id">一意の路線IDを指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("details")]
        public virtual LineInfoResult GetDetailsData(string id, DayType daytype)
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
                Stations = (from sta in tempLine.Stations let staTemp = instance.Stations[sta] select new StationIdNamePair() { Id = sta, Name = staTemp.Name, Yomi = staTemp.Yomi }).ToList()
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
        [ActionName("search_from_number")]
        public virtual List<LineNameInfo> GetLineIdsFromNumber(int number, DayType daytype)
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
        [ActionName("search_from_name")]
        public virtual List<LineNameInfo> GetLinesKeyFromName(string name, DayType daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = instance.Lines.GetDataFromDayType(daytype);
            var temp = (from t in tempLineCollection where t.Value.Name.Contains(name) select new LineNameInfo() { Id = t.Key, Number = t.Value.Number, Name = t.Value.Name }).ToList();
            return temp;
        }

        /// <summary>
        /// 路線一覧情報を検索します。
        /// </summary>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [ActionName("list")]
        public virtual List<LineNameInfo> GetListLineFromName(DayType daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = instance.Lines.GetDataFromDayType(daytype);
            var temp = (from t in tempLineCollection select new LineNameInfo() { Id = t.Key, Number = t.Value.Number, Name = t.Value.Name }).ToList();
            return temp;
        }

    }
}
