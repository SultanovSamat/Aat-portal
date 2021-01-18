using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   [Serializable]
   public class VehicleLT
   {
      public Guid UID { get; set; }

      public string Brand { get; set; }

      public string Model { get; set; }

      public string Vehicle { get; set; }

      public VehicleLT()
      {
      }

      public VehicleLT(Guid uid)
      {
         UID = uid;
      }

      public static List<VehicleLT> Deserialize(IMainWebService client, Guid uidCarrier)
      {
         return SerializeHelper.Deserialize<List<VehicleLT>>(client.GetCarrierVehicles(uidCarrier));
      }
   }
}