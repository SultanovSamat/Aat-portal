using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   [Serializable]
   public class LicenseModel
   {
      public Guid UID { get; set; }
      public string Number { get; set; }
      public DateTime IssuedDate { get; set; }
      public string IssuedDateString => IssuedDate.Year < 1900 ? string.Empty : IssuedDate.ToString("dd.MM.yyyy");
      public DateTime SuspendDate { get; set; }
      public string SuspendDateString => SuspendDate.Year < 1900 ? string.Empty : SuspendDate.ToString("dd.MM.yyyy");

      public DateTime LicValidityStart { get; set; }

      public string LicValidityStartString =>
         LicValidityStart.Year < 1900 ? string.Empty : LicValidityStart.ToString("dd.MM.yyyy");

      public DateTime LicValidityEnd { get; set; }

      public string LicValidityEndString =>
         LicValidityEnd.Year < 1900 ? string.Empty : LicValidityEnd.ToString("dd.MM.yyyy");

      public string LicenseType { get; set; }
      public string Territory { get; set; }
      public string Status { get; set; }
      public int LicFormsCount { get; set; }

      public static List<LicenseModel> Deserialize(IMainWebService client, Guid uidCarrier)
      {
         return SerializeHelper.Deserialize<List<LicenseModel>>(client.GetCarrierLicenses(uidCarrier));
      }
   }
}