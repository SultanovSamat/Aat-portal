using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
   public class QueueLic
   {
      public string Number { get; set; }
      public string INN { get; set; }

      public DateTime Date { get; set; }

      public string DateStr
      {
         get
         {
            if (Date.Year < 1900) return string.Empty;
            return Date.ToString("dd.MM.yyyy");
         }
      }

      public string Carrier { get; set; }

      public QueueLic()
      {
      }

      public QueueLic(string inn, string carrier, string number, DateTime date)
      {
         INN = inn;
         Carrier = carrier;
         Number = number;
         Date = date;
      }

      public static QueueLicData DeserializeQueueData(IMainWebService client)
      {
         return SerializeHelper.Deserialize<QueueLicData>(client.GetQueueData());
      }

      public static QueueLicMsg DeserializeTakeQueue(IMainWebService client, string inn, DateTime date)
      {
         return SerializeHelper.Deserialize<QueueLicMsg>(client.TakeQueue(inn, date));
      }

      public static QueueLicMsg DeserializeGetMineQueues(IMainWebService client, string inn)
      {
         return SerializeHelper.Deserialize<QueueLicMsg>(client.GetMineQueues(inn));
      }

      public static List<QueueLic> DeserializeGetQueues(IMainWebService client, DateTime date)
      {
         return SerializeHelper.Deserialize<List<QueueLic>>(client.GetListQueue(date));
      }
   }

   public class QueueLicData
   {
      public int Count { get; set; }
      public DateTime NextDate { get; set; }

      public string NextDateStr
      {
         get
         {
            if (NextDate.Year < 1900) return string.Empty;
            return NextDate.ToString("yyyy-MM-dd");
         }
      }
   }

   public class QueueLicMsg
   {
      public string Msg { get; set; }
      public string MineCarrier { get; set; }
      public bool IsError { get; set; }
      public List<QueueLic> MineQueues { get; set; }
   }
}