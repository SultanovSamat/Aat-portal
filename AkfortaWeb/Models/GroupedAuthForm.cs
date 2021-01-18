using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class GroupedAuthForm
   {
      public string Country { get; set; }
      public string Type { get; set; }
      public int FreeCount { get; set; }
        public string ExpiryDate { get; set; }

        public static List<GroupedAuthForm> Deserialize(IMainWebService client, Guid departmentId, int year)
      {
         return SerializeHelper.Deserialize<List<GroupedAuthForm>>(client.GetGroupedFreeAuthForms(departmentId, year));
      }

      public static List<GroupedAuthForm> DeserializeCarrierForms(IMainWebService client, Guid carrierUid,
         Guid uidCountry)
      {
         return SerializeHelper.Deserialize<List<GroupedAuthForm>>(client.GetCarrierForms(carrierUid, uidCountry));
      }
   }
}