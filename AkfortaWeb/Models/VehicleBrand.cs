using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("VehicleBrands"), XmlType("VehicleBrand")]
   public class VehicleBrand
   {
      public Guid UID { get; set; }

      public string Name { get; set; }

      public string Code { get; set; }

      public VehicleBrand()
      {
      }

      public VehicleBrand(Guid uid)
      {
         UID = uid;
      }

      public VehicleBrand(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
         Code = row[2].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<VehicleBrand>(file);
      }

      public static List<VehicleBrand> GetXmlData()
      {
         SetXmlData("vehicleBrands");
         return XmlData;
      }

      private static List<VehicleBrand> XmlData { get; set; }
   }
}