using System;
using System.Collections.Generic;
using System.Data;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class Carrier
   {
      public string Name { get; set; }
      public int MessageCount { get; set; }
      public Guid UID { get; set; }
      public bool IsArchive { get; set; }
      public string INN { get; set; }
      public string ShortName { get; set; }
      public string OrgStructure { get; set; }
      public string LegalRegionName { get; set; }

      public string LegalAddress { get; set; }

      public string Address
      {
         get
         {
            var flag = string.IsNullOrEmpty(LegalAddress);
            var result = flag ? LegalRegionName : LegalAddress + ", " + LegalRegionName;

            return result;
         }
      }

      public string Serie { get; set; }
      public string Number { get; set; }
      public string EMail { get; set; }
      public DateTime RegExpirationDate { get; set; }

      public string RegExpirationDateString =>
         RegExpirationDate.Year < 1900 ? string.Empty : RegExpirationDate.ToString("dd.MM.yyyy");

      public Carrier()
      {
      }

      public Carrier(string name, Guid uid)
      {
         Name = name;
         UID = uid;
      }

      public Carrier(DataRow row)
      {
         UID = Guid.Parse(row[0].ToString());
         Name = row[1].ToString();
         INN = row[2].ToString();
         ShortName = row[3].ToString();
         OrgStructure = row[4].ToString();
         LegalRegionName = row[5].ToString();
         LegalAddress = row[6].ToString();
         Serie = row[7].ToString();
         Number = row[8].ToString();
         RegExpirationDate = Convert.ToDateTime(row[9]);
      }

      public static Carrier Deserialize(IMainWebService client, string name, string password)
      {
         var carrierStream = client.GetCarrier(name, password);
         return carrierStream != null ? SerializeHelper.Deserialize<Carrier>(carrierStream) : null;
      }

      public static bool ChangePassword(IMainWebService client, Guid uidCarrier, string curPassword, string newPassword,
         string newPasswordRepeat)
      {
         return client.ChangePassword(uidCarrier, curPassword, newPassword, newPasswordRepeat);
      }

      public static bool ChangeLogin(IMainWebService client, Guid uidCarrier, string curPassword, string newLogin,
         string newLoginRepeat)
      {
         return client.ChangeLogin(uidCarrier, curPassword, newLogin, newLoginRepeat);
      }

      public static bool ChangeEmail(IMainWebService client, Guid uidCarrier, string curPassword, string newEmail,
         string newEmailRepeat)
      {
         return client.ChangeEmail(uidCarrier, curPassword, newEmail, newEmailRepeat);
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
            XmlData = SerializeHelper.GetXmlData<Carrier>(file);
      }

      public static List<Carrier> GetXmlData()
      {
         SetXmlData("carriers");
         return XmlData;
      }

      private static List<Carrier> XmlData { get; set; }
   }

   public class GroupedCarrier
   {
      public List<Carrier> Data { get; set; }
      public int Count { get; set; }
   }
}