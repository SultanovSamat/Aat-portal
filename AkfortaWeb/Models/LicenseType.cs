using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("LicenseTypes"), XmlType("LicenseType")]
   public class LicenseType
   {
      public Guid UID { get; set; }

      public string Name { get; set; }

      public LicenseType()
      {
      }

      public LicenseType(Guid uid)
      {
         UID = uid;
      }

      public LicenseType(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<LicenseType>(file);
      }

      public static List<LicenseType> GetXmlData()
      {
         SetXmlData("licenseTypes");
         return XmlData;
      }

      private static List<LicenseType> XmlData { get; set; }
   }
}