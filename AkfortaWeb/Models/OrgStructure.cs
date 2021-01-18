using System;
using System.Collections.Generic;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   public class OrgStructure
   {
      public string Name { get; set; }
      public Guid UID { get; set; }
      public bool IsIndividual { get; set; }
      public bool Valid { get; set; }

      public OrgStructure()
      {
      }

      public OrgStructure(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
         IsIndividual = Convert.ToBoolean(row[2].ToString());
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<OrgStructure>(file);
      }

      public static List<OrgStructure> GetXmlData()
      {
         SetXmlData("orgStructures");
         return XmlData;
      }

      private static List<OrgStructure> XmlData { get; set; }
   }
}