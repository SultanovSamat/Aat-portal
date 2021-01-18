using System;
using System.Linq;
using AkfortaWeb.Helpers;
using EECExchange.Common.Enum;
using EECExchange.Common.WebService;

namespace AkfortaWeb.Models
{
   public class TransportControlMessage
   {
      #region identifiers

      public long MessageID { get; set; }
      public long DocID { get; set; }
      public string MessageGuid { get; set; }
      public string RelatesTo { get; set; }
      public string EDocId { get; set; }

      #endregion

      #region messageField

      public DateTime? AcceptTime { get; set; }
      public string MessageDate { get; set; }
      public string SendSegment { get; set; }
      public string ReceiveSegment { get; set; }
      public string Status { get; set; }
      public string MessageName { get; set; }
      public Guid? UserId { get; set; }
      public string UserName { get; set; }
      public string IP { get; set; }
      public string Comment { get; set; }
      public string Control { get; set; }

      #endregion

      #region DocumentFields

      public DateTime EDocDateTime { get; set; }

      public string LanguageCode { get; set; }

      public string DocumentType { get; set; }

      #endregion

      public TransportControlMessage()
      {
      }

      public TransportControlMessage(EECMessage message, Country[] countries)
      {
         MessageID = message.ID;
         MessageGuid = message.MessageID;

         SendSegment = EnuEECSegments.AllElements.FirstOrDefault(x => x.Code == message.SendSegment)?.Name;
         ReceiveSegment = EnuEECSegments.AllElements.FirstOrDefault(x => x.Code == message.ReceiveSegment)?.Name;
         Status = EnuMessageStatus.AllElements.FirstOrDefault(x => x.Code == message.Status)?.Name;
         MessageDate = message.MessageDate.ToString("dd.MM.yyyy HH:mm:ss");
         var messageType = EnuEECMessageType.AllElements.FirstOrDefault(x => x.Code == message.MessageType);
         if (messageType != null)
         {
            MessageName = messageType.Name;
            IsRequest = messageType.MessageCategory == EnuMessageCategory.Request;
         }

         var doc = message.Documents?.FirstOrDefault();
         if (doc != null)
         {
            DocID = doc.ID;
            EDocId = doc.EDocId;

            EDocDateTime = doc.EDocDateTime;
            LanguageCode = doc.LanguageCode;
            DocumentType = EnuEECDocumentType.AllElements.FirstOrDefault(x => x.Code == doc.DocumentType)?.Name;

            var searchParameter = doc.GetNumericProperty("SearchParameter");
            if (searchParameter.HasValue)
            {
               var p = decimal.ToInt32(searchParameter.Value);
               var item = (SearchParameter) p;
               SearchParameter = item.GetDescription();
            }
            var periodStart = doc.GetDateTimeProperty("PeriodStart");
            var periodEnd = doc.GetDateTimeProperty("PeriodEnd");
            if (periodStart.HasValue && periodEnd.HasValue)
               RequestPeriod = $"{periodStart:dd.MM.yyyy} - {periodEnd:dd.MM.yyyy}";
            var vehicleType = doc.GetStringProperty("VehicleType");
            VehicleType = vehicleType;
            VehicleRegId = doc.GetStringProperty("VehicleRegId");

            var docsQty = doc.GetNumericProperty("DocsQuantity");
            var severalDocs = docsQty.HasValue && docsQty.Value > 1;
            
            var formCountry = doc.GetStringProperty("FormIdCountry");
            Country country;
            if (formCountry != null &&
                (country = countries.FirstOrDefault(x => x.Code2 == formCountry)) != null)
               formCountry = country.CodeISO;
            var formIdBorderCheckpoint = doc.GetStringProperty("FormIdBorderCheckpoint");
            var formIdEventDate = doc.GetDateTimeProperty("FormIdEventDate");
            var formNumberId = doc.GetStringProperty("FormNumberId");
            if (!string.IsNullOrEmpty(formCountry) && !string.IsNullOrEmpty(formIdBorderCheckpoint) &&
                formIdEventDate.HasValue && !string.IsNullOrEmpty(formNumberId))
            {
               FormId = $"{formCountry}/{formIdBorderCheckpoint}/{formIdEventDate.Value:ddMMyy}/{formNumberId}";
               if (severalDocs)
                  FormId += "...";
            }
            
            var permDocKindCode = doc.GetStringProperty("PermitDocKindCode");
            var permDocKindName = doc.GetStringProperty("PermitDocKindName");
            PermDocId = doc.GetStringProperty("PermitDocId");
            var createDate = doc.GetDateTimeProperty("PermitDocCreationDate");
            PermDocCreationDate = createDate.HasValue ? createDate.Value.ToString("dd.MM.yyyy") : string.Empty;

            if (!string.IsNullOrEmpty(permDocKindCode) && !string.IsNullOrEmpty(permDocKindName))
               PermitDocType = $"{permDocKindCode} - {permDocKindName}";

            FineQuantitty = doc.GetNumericProperty("FineQuantitty");
            NoticeQuantity = doc.GetNumericProperty("NoticeQuantity");
         }

         #region unused

         RelatesTo = message.RelatesTo;
         AcceptTime = message.AcceptTime;
         UserId = message.UserId;
         UserName = message.UserName;

         #endregion
      }

      public decimal? NoticeQuantity { get; set; }

      public decimal? FineQuantitty { get; set; }

      public string PermDocCreationDate { get; set; }

      public string PermDocId { get; set; }

      public string PermitDocType { get; set; }

      public string FormId { get; set; }

      public string VehicleRegId { get; set; }

      public string VehicleType { get; set; }

      public string RequestPeriod { get; set; }

      public string SearchParameter { get; set; }

      public bool IsRequest { get; set; }
   }
}