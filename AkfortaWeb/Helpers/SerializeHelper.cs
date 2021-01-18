using System;  
using System.Collections.Generic;  
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace AkfortaWeb.Helpers
{
   public static class SerializeHelper
   {
      public static T Deserialize<T>(Stream stream)
      {
         using (stream)
         {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
               return serializer.Deserialize<T>(jsonTextReader);
            }
         }
      }

      public static List<T> GetXmlData<T>(string xmlFileName) where T : new()
      {
         var xmlData = HttpContext.Current.Server.MapPath("~/Static/" + xmlFileName + ".xml");
         var ds = new DataSet();
         ds.ReadXml(xmlData);
         var data = ds.Tables[0].AsEnumerable().Select(x => (T) Activator.CreateInstance(typeof(T), x)).ToList();
         var modified = File.GetLastWriteTime(xmlData);
         File.SetLastAccessTime(xmlData, modified);
         return data;
      }

      public static bool ShouldUpdate(string xmlFileName)
      {
         var file = HttpContext.Current.Server.MapPath("~/Static/" + xmlFileName + ".xml");
         var access = File.GetLastAccessTime(file);
         var modified = File.GetLastWriteTime(file);
         return modified != access;
      }
   }
}