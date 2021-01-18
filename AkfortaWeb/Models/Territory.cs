using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("Territories"), XmlType("Territory")]
   public class Territory
   {
      public Guid UID { get; set; }

      public string Name { get; set; }

      public Territory()
      {
      }

      public Territory(Guid uid)
      {
         UID = uid;
      }

      public Territory(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<Territory>(file);
      }

      public static List<Territory> GetXmlData()
      {
         SetXmlData("territories");
         return XmlData;
      }

      private static List<Territory> XmlData { get; set; }
   }
}