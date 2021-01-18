using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("CheckPoints"), XmlType("CheckPoint")]
   public class CheckPoint
   {
      public Guid UID { get; set; }

      public string Geolocation { get; set; }

      public string Location { get; set; }

      public string Number { get; set; }

      public string Phone { get; set; }

      public string Country { get; set; }

      public Guid CountryUID { get; set; }

      public string Email { get; set; }

      public string[] Emails => Email.Split(new[] {',', ';', ' '}, StringSplitOptions.RemoveEmptyEntries);

      public string Name { get; set; }

      public bool ExternalCheckpoint { get; set; }

      public bool ViolationCheckpoint { get; set; }

      public string Fax { get; set; }

      public CheckPoint()
      {
      }

      public CheckPoint(Guid uid)
      {
         UID = uid;
      }

      public CheckPoint(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Country = row[1].ToString();
         CountryUID = Guid.Parse(row[2].ToString());
         Email = row[3].ToString();
         ExternalCheckpoint = Convert.ToBoolean(row[4].ToString());
         Fax = row[5].ToString();
         Geolocation = row[6].ToString();
         Location = row[7].ToString();
         Name = row[8].ToString();
         Number = row[9].ToString();
         Phone = row[10].ToString();
         ViolationCheckpoint = Convert.ToBoolean(row[11].ToString());
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<CheckPoint>(file);
      }

      public static List<CheckPoint> GetXmlData()
      {
         SetXmlData("checkPoints");
         return XmlData;
      }

      private static List<CheckPoint> XmlData { get; set; }
   }
}