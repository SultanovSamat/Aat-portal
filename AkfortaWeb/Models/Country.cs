using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("Countries"), XmlType("Country")]
   public class Country
   {
      public Guid UID { get; set; }

      public string Code2 { get; set; }

      public string Code3 { get; set; }

      public string Email { get; set; }

      public string Name { get; set; }

      public bool ProvidesAuthForms { get; set; }

      public string CodeISO { get; set; }

      public string Currency { get; set; }

      public bool InEAC { get; set; }

      public Country()
      {
      }

      public Country(Guid uid)
      {
         UID = uid;
      }

      internal static Country DeserializeCodeIso(IMainWebService client)
      {
         var clientData = client.GetMainCountryCodeIso();
         return SerializeHelper.Deserialize<Country>(clientData);
      }

      public Country(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Code2 = row[1].ToString();
         Code3 = row[2].ToString();
         CodeISO = row[3].ToString();
         Currency = row[4].ToString();
         InEAC = Convert.ToBoolean(row[5].ToString());
         Name = row[6].ToString();
         ProvidesAuthForms = Convert.ToBoolean(row[7].ToString());
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
         {
            var data = SerializeHelper.GetXmlData<Country>(file);
            if (data != null) XmlData = data.OrderBy(x => x.Name).ToList();
         }
      }

      public static List<Country> GetXmlData()
      {
         SetXmlData("countries");
         return XmlData ?? new List<Country>();
      }

      private static List<Country> XmlData { get; set; }
   }
}