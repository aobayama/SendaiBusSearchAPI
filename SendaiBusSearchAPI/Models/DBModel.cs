using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            var temp = ConfigurationManager.AppSettings["datapath"];
            var path = temp.StartsWith("~") ? HttpContext.Current.Server.MapPath(temp) : temp; // ~で始まっていれば絶対パスに変換

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
