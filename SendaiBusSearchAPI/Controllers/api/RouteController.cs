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
    [RoutePrefix("api/route")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RouteController : ApiController
    {

        [HttpGet()]
        [Route("search")]
        public RouteSearchResult SearchRoute(int from, int to, int daytype,string dept = null, int count = 5)
        {
            var instance = DBModel.GetInstance();
            
            string dayTypeQuery = null;
            switch (daytype)
            {
                case 0:
                    dayTypeQuery = Commons.WEEKDAY;
                    break;
                case 1:
                    dayTypeQuery = Commons.SATURDAY;
                    break;
                case 2:
                    dayTypeQuery = Commons.HOLIDAY;
                    break;
                default:
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            // deptがnullなら現在時刻
            TimeSpan deptTime = DateTime.Now.TimeOfDay;
            if (dept != null && dept != String.Empty)
            {
                deptTime = Commons.ConvertToTimeSpan(dept);
                if (deptTime == TimeSpan.Zero)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
            }


            var fromStation = instance.Stations.Where(c => c.Key == from).Select(c => c.Value).SingleOrDefault();
            var toStation = instance.Stations.Where(c => c.Key == to).Select(c => c.Value).SingleOrDefault();

            if (fromStation == null || toStation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var result = new RouteSearchResult()
            {
                FromStation = new IdNamePair() { Id = from, Name = fromStation.Name },
                ToStation = new IdNamePair() { Id = to, Name = toStation.Name }
            };


            var tempResult = (from busSet in
                                 fromStation.Buses.Where(c => c.DayType == dayTypeQuery && Commons.ConvertToTimeSpan(c.DeptTime) >= deptTime)
                                .Join(toStation.Buses, f => f.BusId, t => t.BusId, (f, t) => new { From = f, To = t })
                             let bus = instance.Buses[busSet.From.BusId]
                             let line = instance.Lines.GetDataFromDayType(dayTypeQuery)[bus.LineKey]
                             where line.Stations.IndexOf(@from) < line.Stations.IndexOf(to)
                             let time = Commons.ConvertToString(Commons.DiffTimespan(Commons.ConvertToTimeSpan(busSet.From.DeptTime), Commons.ConvertToTimeSpan(busSet.To.DeptTime)))
                             select new Route()
                             {
                                 Pathes = new List<Path>()
                                {
                                    new Path()
                                    {
                                        DeptNode = new Node() { Station = result.FromStation, Time = busSet.From.DeptTime },
                                        ArrNode = new Node() { Station = result.ToStation, Time = busSet.To.DeptTime },
                                        IsWalk = false,
                                        LineKey = bus.LineKey,
                                        BusId = busSet.From.BusId,
                                        Time = time
                                    }
                                },
                                 Cost = 1.0,
                                 TotalDeptTime = busSet.From.DeptTime,
                                 TotalArrTime = busSet.To.DeptTime,
                                 TransferCount = 0,
                                 TotalTime = time
                             });
            if (count == -1 || count <= 0)
            {
                result.Routes = tempResult.ToList();
            }
            else
            {
                result.Routes = tempResult.Take(count).ToList();
            }

            return result;
        }
        

        [HttpGet()]
        [Route("search_all")]
        public RouteSearchResult SearchRoute(int from, int to, int daytype)
        {
            var instance = DBModel.GetInstance();

            string dayTypeQuery = null;
            switch (daytype)
            {
                case 0:
                    dayTypeQuery = Commons.WEEKDAY;
                    break;
                case 1:
                    dayTypeQuery = Commons.SATURDAY;
                    break;
                case 2:
                    dayTypeQuery = Commons.HOLIDAY;
                    break;
                default:
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            var fromStation = instance.Stations.Where(c => c.Key == from).Select(c => c.Value).SingleOrDefault();
            var toStation = instance.Stations.Where(c => c.Key == to).Select(c => c.Value).SingleOrDefault();

            if (fromStation == null || toStation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var result = new RouteSearchResult()
            {
                FromStation = new IdNamePair() { Id = from, Name = fromStation.Name },
                ToStation = new IdNamePair() { Id = to, Name = toStation.Name }
            };


            result.Routes = (from busSet in 
                                 fromStation.Buses.Where(c => c.DayType == dayTypeQuery)
                                .Join(toStation.Buses, f => f.BusId, t => t.BusId, (f, t) => new { From = f, To = t })
                             let bus = instance.Buses[busSet.From.BusId]
                             let line = instance.Lines.GetDataFromDayType(dayTypeQuery)[bus.LineKey]
                             where line.Stations.IndexOf(@from) < line.Stations.IndexOf(to)
                             let time = Commons.ConvertToString(Commons.DiffTimespan(Commons.ConvertToTimeSpan(busSet.From.DeptTime), Commons.ConvertToTimeSpan(busSet.To.DeptTime)))
                             select new Route()
                             {
                                 Pathes = new List<Path>()
                                {
                                    new Path()
                                    {
                                        DeptNode = new Node() { Station = result.FromStation, Time = busSet.From.DeptTime },
                                        ArrNode = new Node() { Station = result.ToStation, Time = busSet.To.DeptTime },
                                        IsWalk = false,
                                        LineKey = bus.LineKey,
                                        BusId = busSet.From.BusId,
                                        Time = time
                                    }
                                },
                                 Cost = 1.0,
                                 TotalDeptTime = busSet.From.DeptTime,
                                 TotalArrTime = busSet.To.DeptTime,
                                 TransferCount = 0,
                                 TotalTime = time
                             }).ToList();
            
            return result;
        }


    }
}
