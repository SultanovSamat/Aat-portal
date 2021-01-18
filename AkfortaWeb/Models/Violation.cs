using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class Violation
   {
      public Guid UID { get; set; }

      public string NotificationNumber { get; set; }

      public string ViolationText { get; set; }

      public string State { get; set; }

      public string Date { get; set; }

      public string Country { get; set; }

      public string CheckPoint { get; set; }

      public Violation()
      {
      }

      public Violation(Guid uid)
      {
         UID = uid;
      }

      public static List<Violation> Deserialize(IMainWebService client, Guid uidVehicle)
      {
         return SerializeHelper.Deserialize<List<Violation>>(client.GetVehicleViolations(uidVehicle));
      }

      public static List<Violation> DeserializeByNumber(IMainWebService client, string notificationNumber)
      {
         return SerializeHelper.Deserialize<List<Violation>>(client.GetViolations(notificationNumber));
      }
   }
}