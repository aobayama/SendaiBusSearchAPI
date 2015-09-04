using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SendaiBusSearchAPI.Models
{
    public static class DBModel
    {

        private static JsonData dbData = null;


        public static void Initialize()
        {

        }


        static DBModel()
        {
            // データを読み込む
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/bus_all.json");

            using (var fs = new FileStream(path, FileMode.Open))
                using(var reader = new StreamReader(fs))
            {
                var body = reader.ReadToEnd();
                dbData = JsonConvert.DeserializeObject<JsonData>(body);
            }            

        }


        public static JsonData GetInstance()
        {
            return dbData;
        }



    }
}
