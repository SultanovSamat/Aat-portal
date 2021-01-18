using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class AuthForm
   {
      public Guid UID { get; set; }

      public string Series { get; set; }

      public string Number { get; set; }

      public AuthForm()
      {
      }

      public AuthForm(Guid uid)
      {
         UID = uid;
      }

      public AuthForm(Guid uid, string serie, string number)
      {
         UID = uid;
         Series = serie;
         Number = number;
      }

      public static List<AuthForm> Deserialize(IMainWebService client)
      {
         return SerializeHelper.Deserialize<List<AuthForm>>(client.GetFreeAuthForms());
      }

      public static GroupedCarrier DeserializeFormCarriers(IMainWebService client, string inn, string name,
         string orgForm, Guid countryUID, int pageNum, int pageSize)
      {
         var clientData = client.GetFormCarriers(inn, name, orgForm, countryUID, pageNum, pageSize);
         return SerializeHelper.Deserialize<GroupedCarrier>(clientData);
      }

      public bool IsFree { get; set; }
   }
}