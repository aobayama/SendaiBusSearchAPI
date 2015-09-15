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
    /// 経路検索に関するAPIを提供します。
    /// </summary>
    [RoutePrefix("api/route")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RouteController : ApiController
    {

        /// <summary>
        /// 指定された駅間のルートを検索します。
        /// </summary>
        /// <param name="from">出発駅IDを指定します。</param>
        /// <param name="to">目的駅IDを指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <param name="method">（オプション）経路検索の検索方法を指定します。省略された場合は発時刻検索が指定されます。queryTimeを指定しなかった場合、methodは強制的に発時刻検索が指定されます。</param>
        /// <param name="queryTime">（オプション）時刻検索に用いる時間を指定します。(hh:mm)省略された場合は現在時刻が指定されます。</param>
        /// <param name="count">（オプション）検索結果の最大数を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search")]
        public RouteSearchResult SearchRoute(string from, string to, DayType daytype,RouteSeachMethod method = RouteSeachMethod.DepartureBase, string queryTime = null, int count = 5)
        {
            var instance = DBModel.GetInstance();
            
            // deptがnullなら現在時刻
            TimeSpan baseTime = DateTime.Now.TimeOfDay;
            if (queryTime != null && queryTime != String.Empty)
            {
                baseTime = Commons.ConvertToTimeSpan(queryTime);
                if (baseTime == TimeSpan.Zero)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
            }
            else
            {
                method = RouteSeachMethod.DepartureBase;
            }
            
            var fromStation = instance.Stations.Where(c => c.Key == from).Select(c => c.Value).SingleOrDefault();
            var toStation = instance.Stations.Where(c => c.Key == to).Select(c => c.Value).SingleOrDefault();

            if (fromStation == null || toStation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var result = new RouteSearchResult()
            {
                FromStation = new StationIdNamePair() { Id = from, Name = fromStation.Name, Yomi = fromStation.Yomi },
                ToStation = new StationIdNamePair() { Id = to, Name = toStation.Name, Yomi = toStation.Yomi }
            };
            
            IEnumerable<Tuple<BusDeptTimeInfo, BusDeptTimeInfo>> tempSource = null;
            if (method == RouteSeachMethod.DepartureBase)
            {
                // 発検索 -> fromの発車時刻がクエリとして与えられた時刻よりも新しい
                tempSource = fromStation.Buses.Where(c => c.DayType == daytype && Commons.ConvertToTimeSpan(c.DeptTime) >= baseTime)
                                    .Join(toStation.Buses, f => f.BusId, t => t.BusId, (f, t) => new Tuple<BusDeptTimeInfo, BusDeptTimeInfo>(f, t));
            }
            else
            {
                // 着検索 -> toの発車時刻がクエリとして与えられた時刻よりも古い
                tempSource = toStation.Buses.Where(c => c.DayType == daytype && Commons.ConvertToTimeSpan(c.DeptTime) <= baseTime)
                    .Join(fromStation.Buses, t => t.BusId, f => f.BusId, (t, f) => new Tuple<BusDeptTimeInfo, BusDeptTimeInfo>(f, t));
            }

            // 発と着の順番を考慮
            var tempRoute = (from busSet in tempSource
                              let bus = instance.Buses[busSet.Item1.BusId]
                              let line = instance.Lines.GetDataFromDayType(daytype)[bus.LineId]
                              where line.Stations.IndexOf(@from) < line.Stations.IndexOf(to) select busSet);

            // 提案コスト検索
            IEnumerable<Route> tempResult = null;
            if (method == RouteSeachMethod.DepartureBase)
            {
                tempResult = (from route in tempRoute
                                   let deptTime = Commons.ConvertToTimeSpan(route.Item1.DeptTime) let arrTime = Commons.ConvertToTimeSpan(route.Item2.DeptTime)
                                   let costTime = Commons.DiffTimespan(deptTime, arrTime)
                                   orderby costTime descending // 2番目のクエリ：所要時間 (3番目のクエリ：TransferCount)
                                   orderby deptTime ascending  // 1番目のクエリ：到着時間が近い順
                                   let costTimeStr = Commons.ConvertToString(costTime)
                                   select new Route()
                                   {
                                       Pathes = new List<Path>()
                                       {
                                           new Path()
                                           {
                                               DeptNode = new Node() { Station = result.FromStation, Time = route.Item1.DeptTime },
                                               ArrNode = new Node() { Station = result.ToStation, Time = route.Item2.DeptTime },
                                               Method = TransferMethod.Bus,
                                               LineId = instance.Buses[route.Item1.BusId].LineId,
                                               BusId = route.Item1.BusId,
                                               Time = costTimeStr
                                           }
                                       },
                                       Rank = 1.0,
                                       TotalDeptTime = route.Item1.DeptTime,
                                       TotalArrTime = route.Item2.DeptTime,
                                       TransferCount = 0,
                                       TotalTime = costTimeStr
                                   }).Select((n, ind) =>
                                   {
                                       n.Rank = ind;
                                       return n;
                                   });                
            }
            else
            {
                tempResult = (from route in tempRoute
                              let deptTime = Commons.ConvertToTimeSpan(route.Item1.DeptTime)
                              let arrTime = Commons.ConvertToTimeSpan(route.Item2.DeptTime)
                              let costTime = Commons.DiffTimespan(deptTime, arrTime)
                              orderby costTime descending // 2番目のクエリ：所要時間 (3番目のクエリ：TransferCount)
                              orderby deptTime descending  // 1番目のクエリ：到着時間が近い順
                              let costTimeStr = Commons.ConvertToString(costTime)
                              select new Route()
                              {
                                  Pathes = new List<Path>()
                                       {
                                           new Path()
                                           {
                                               DeptNode = new Node() { Station = result.FromStation, Time = route.Item1.DeptTime },
                                               ArrNode = new Node() { Station = result.ToStation, Time = route.Item2.DeptTime },
                                               Method = TransferMethod.Bus,
                                               LineId = instance.Buses[route.Item1.BusId].LineId,
                                               BusId = route.Item1.BusId,
                                               Time = costTimeStr
                                           }
                                       },
                                  Rank = 1.0,
                                  TotalDeptTime = route.Item1.DeptTime,
                                  TotalArrTime = route.Item2.DeptTime,
                                  TransferCount = 0,
                                  TotalTime = costTimeStr
                              }).Select((n, ind) =>
                              {
                                  n.Rank = ind;
                                  return n;
                              });
            }

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

        /// <summary>
        /// 指定された駅間の全ルートを検索します。
        /// </summary>
        /// <param name="from">出発駅IDを指定します。</param>
        /// <param name="to">目的駅IDを指定します。</param>
        /// <param name="daytype">運行日を指定します。</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search_all")]
        public RouteSearchResult SearchRoute(string from, string to, DayType daytype)
        {
            var instance = DBModel.GetInstance();
            var fromStation = instance.Stations.Where(c => c.Key == from).Select(c => c.Value).SingleOrDefault();
            var toStation = instance.Stations.Where(c => c.Key == to).Select(c => c.Value).SingleOrDefault();

            if (fromStation == null || toStation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var result = new RouteSearchResult()
            {
                FromStation = new StationIdNamePair() { Id = from, Name = fromStation.Name, Yomi = fromStation.Yomi },
                ToStation = new StationIdNamePair() { Id = to, Name = toStation.Name, Yomi = toStation.Yomi }
            };


            result.Routes = (from busSet in 
                                 fromStation.Buses.Where(c => c.DayType == daytype)
                                .Join(toStation.Buses, f => f.BusId, t => t.BusId, (f, t) => new { From = f, To = t })
                             let bus = instance.Buses[busSet.From.BusId]
                             let line = instance.Lines.GetDataFromDayType(daytype)[bus.LineId]
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
                                        Method = TransferMethod.Bus,
                                        LineId = bus.LineId,
                                        BusId = busSet.From.BusId,
                                        Time = time
                                    }
                                },
                                 Rank = 1.0,
                                 TotalDeptTime = busSet.From.DeptTime,
                                 TotalArrTime = busSet.To.DeptTime,
                                 TransferCount = 0,
                                 TotalTime = time
                             }).ToList();
            
            return result;
        }
        
    }
}
