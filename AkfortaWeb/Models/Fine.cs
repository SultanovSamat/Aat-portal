using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class Fine
   {
      public Guid UID { get; set; }

      public string Driver { get; set; }
      public string Status { get; set; }
      public string DateDue { get; set; }
      public double FineSum { get; set; }

      public Fine()
      {
      }

      public Fine(Guid uid)
      {
         UID = uid;
      }

      public static List<Fine> Deserialize(IMainWebService client, Guid uidVehicle)
      {
         return SerializeHelper.Deserialize<List<Fine>>(client.GetVehicleFines(uidVehicle));
      }
   }
}