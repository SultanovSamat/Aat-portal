using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class SpecialistLT
   {
      public Guid UID { get; set; }

      public string DrivingExperience { get; set; }

      public string Position { get; set; }

      public string Specialist { get; set; }

      public SpecialistLT()
      {
      }

      public SpecialistLT(Guid uid)
      {
         UID = uid;
      }

      public static List<SpecialistLT> Deserialize(IMainWebService client, Guid uidCarrier)
      {

         return SerializeHelper.Deserialize<List<SpecialistLT>>(client.GetCarrierSpecialists(uidCarrier));
      }
   }
}