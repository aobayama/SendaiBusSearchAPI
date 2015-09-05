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
    [RoutePrefix("api/lines")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LinesController : ApiController
    {
        [HttpGet()]
        [Route("details")]
        public LineInfoResult GetDetailsData(string key, int daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = null;
            
            switch (daytype)
            {
                case 0:
                    tempLineCollection = instance.Lines.Weekday;
                    break;
                case 1:
                    tempLineCollection = instance.Lines.Saturday;
                    break;
                case 2:
                    tempLineCollection = instance.Lines.Holiday;
                    break;
                default:
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            if (!tempLineCollection.ContainsKey(key))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }


            var tempLine = tempLineCollection[key];

            var result = new LineInfoResult()
            {
                LineKey = key,
                LineId = tempLine.Id,
                LineName = tempLine.Name,
                Buses = tempLine.Buses,
                Stations = (from sta in tempLine.Stations select new IdNamePair() { Id = sta, Name = instance.Stations[sta].Name }).ToList()
            };

            return result;

        }

        [HttpGet()]
        [Route("search_from_id")]
        public List<IdKeyNamePair> GetLinesKeyFromId(int id, int daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = null;

            switch (daytype)
            {
                case 0:
                    tempLineCollection = instance.Lines.Weekday;
                    break;
                case 1:
                    tempLineCollection = instance.Lines.Saturday;
                    break;
                case 2:
                    tempLineCollection = instance.Lines.Holiday;
                    break;
                default:
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            var temp = (from t in tempLineCollection where t.Value.Id == id select new IdKeyNamePair { Id = t.Value.Id, Key = t.Key, Name = t.Value.Name }).ToList();
            return temp;
        }

        [HttpGet()]
        [Route("search_from_name")]
        public List<IdKeyNamePair> GetLinesKeyFromName(string name, int daytype)
        {
            var instance = DBModel.GetInstance();
            Dictionary<string, Line> tempLineCollection = null;

            switch (daytype)
            {
                case 0:
                    tempLineCollection = instance.Lines.Weekday;
                    break;
                case 1:
                    tempLineCollection = instance.Lines.Saturday;
                    break;
                case 2:
                    tempLineCollection = instance.Lines.Holiday;
                    break;
                default:
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            var temp = (from t in tempLineCollection where t.Value.Name.Contains(name) select new IdKeyNamePair() { Key = t.Key, Id = t.Value.Id, Name = t.Value.Name }).ToList();
            return temp;
        }

    }
}
