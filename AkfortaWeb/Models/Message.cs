using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   [Serializable]
   public class Message
   {
      public Guid UID { get; set; }

      public string Subject { get; set; }

      public string Date { get; set; }

      public string MessageLT { get; set; }

      public Message()
      {
      }

      public Message(Guid uid)
      {
         UID = uid;
      }

      public static List<Message> Deserialize(IMainWebService client, Guid uidCarrier)
      {
         return SerializeHelper.Deserialize<List<Message>>(client.GetCarrierMessages(uidCarrier));
      }

      public static List<Message> DeleteAndDeserialize(IMainWebService client, Guid uidCarrier, Guid uidMessage)
      {
         return SerializeHelper.Deserialize<List<Message>>(client.DeleteMessageAndGetData(uidCarrier, uidMessage));
      }
   }
}