using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class ScheduledInspection
   {
      public Guid UID { get; set; }

      public string Department { get; set; }
      public Guid DepartmentUID { get; set; }
      public string Name { get; set; }
      public string Date { get; set; }
      public string INN { get; set; }

      public ScheduledInspection()
      {
      }

      public ScheduledInspection(Guid uid)
      {
         UID = uid;
      }

      public static List<ScheduledInspection> Deserialize(IMainWebService client, int begYear, int endYear,
         Guid departmentUID, string name, string INN)
      {
         return SerializeHelper.Deserialize<List<ScheduledInspection>>(
            client.GetScheduledInspections(begYear, endYear, departmentUID, name, INN));
      }
   }
}