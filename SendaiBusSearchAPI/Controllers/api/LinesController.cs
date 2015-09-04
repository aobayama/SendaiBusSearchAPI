using SendaiBusSearchAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SendaiBusSearchAPI.Controllers.api
{
    [RoutePrefix("api/lines")]
    public class LinesController : ApiController
    {
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

        public List<string> GetLinesKeyFromId(int id, int daytype)
        {


        }




    }
}
