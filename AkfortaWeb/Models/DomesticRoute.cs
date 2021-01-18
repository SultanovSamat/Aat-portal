using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class DomesticRoute
   {
      public Guid UID { get; set; }

      public int BackTripDuration { get; set; }

      public string Departure { get; set; }

      public string DepartureName { get; set; }

      public string Destination { get; set; }

      public string DestinationName { get; set; }

      public int Interval { get; set; }

      public decimal Length { get; set; }

      public string NameRoute { get; set; }
      public string NumberRoute { get; set; }

      public int TripDuration { get; set; }

      public decimal TripsQty { get; set; }

      public decimal VehiclesQty { get; set; }

      public List<Carrier> Carriers { get; set; }

      public DomesticRoute()
      {
      }

      public DomesticRoute(Guid uid)
      {
         UID = uid;
      }

      public static List<DomesticRoute> Deserialize(IMainWebService client, string depCode, string destCode, int pageNum,
         int pageSize)
      {
         var clientData = client.GetDomesticRoutes(depCode, destCode, pageSize, pageSize);
         return SerializeHelper.Deserialize<List<DomesticRoute>>(clientData);
      }
   }
}