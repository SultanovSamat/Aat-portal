using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("Routes"), XmlType("Route")]
   public class Route
   {
      public Guid UID { get; set; }

      public string Name { get; set; }

      public Route()
      {
      }

      public Route(Guid uid)
      {
         UID = uid;
      }

      public Route(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<Route>(file);
      }

      public static List<Route> GetXmlData()
      {
         SetXmlData("routes");
         return XmlData;
      }

      private static List<Route> XmlData { get; set; }
   }
}