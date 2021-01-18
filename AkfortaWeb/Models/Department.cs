using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("Departments"), XmlType("Department")]
   public class Department
   {
      public string Name { get; set; }
      public Guid UID { get; set; }
      public string Account { get; set; }
      public string Chief { get; set; }
      public string Adress { get; set; }
      public string Bank { get; set; }
      public string Email { get; set; }

      public string[] Emails
      {
         get
         {
            if (Email == null) return new string[] { };
            return Email.Split(new[] {',', ';', ' '}, StringSplitOptions.RemoveEmptyEntries);
         }
      }

      public string Phone { get; set; }
      public string Schedule { get; set; }
      public string Geolocation { get; set; }

      public Department()
      {
      }

      public Department(Guid uid)
      {
         UID = uid;
      }

      public static List<Department> Deserialize(IMainWebService client)
      {
         return SerializeHelper.Deserialize<List<Department>>(client.GetScheduledDepartments());
      }

      public Department(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
         Account = row[2].ToString();
         Adress = row[3].ToString();
         Bank = row[4].ToString();
         Chief = row[5].ToString();
         Email = row[6].ToString();
         Phone = row[7].ToString();
         Schedule = row[8].ToString();
         Geolocation = row[9].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<Department>(file);
      }

      public static List<Department> GetXmlData()
      {
         SetXmlData("departments");
         return XmlData;
      }

      private static List<Department> XmlData { get; set; }
   }
}