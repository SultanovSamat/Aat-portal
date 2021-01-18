using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Akforta.eLeed.Expressions;
using AkfortaWeb.Models;
using AkfortaWeb.Helpers;
using AkfortaWeb.Helpers.ControlGeneration;
using AkfortaWeb.Settings;
using BIZ.Core.Common.Script;
using BIZ.ExternalIntegration.ASP.MVC;
using EECExchange.Common.EECDocuments;
using EECExchange.Common.Enum;
using EECExchange.Common.WebService;
using EECExchange.DataContract;
using EECExchange.Presentation;
using EECExchange.Tools;

namespace AkfortaWeb.Controllers
{
   public class ApiController : Umbraco.Web.Mvc.SurfaceController
   {
      public JsonResult GetCountries()
      {
         var data = XMLHelper.GetCountries();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetRoutes()
      {
         var data = XMLHelper.GetRoutes();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetEACCountries()
      {
         var countries = XMLHelper.GetCountries();
         var data = countries.Where(x => x.InEAC).ToList();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetVehicleKinds()
      {
         var data = XMLHelper.GetVehicleKinds();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetVehicleBrands()
      {
         var data = XMLHelper.GetVehicleBrands();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [Authorize]
      public JsonResult GetVehicles()
      {
         var data = WebHelper.GetVehicles();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [Authorize]
      public JsonResult GetSpecialists()
      {
         var data = WebHelper.GetSpecialists();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetDepartments()
      {
         var data = XMLHelper.GetDepartments();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetSheduledDepartments()
      {
         var data = WebHelper.GetSheduledDepartments();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetFreeAuthForms()
      {
         var data = WebHelper.GetFreeAuthForms();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetGroupedFreeAuthForms(Guid? departmentId, int year)
      {
         var departmentId2 = departmentId ?? Guid.Empty;
         var groupedFreeAuthForms = WebHelper.GetGroupedFreeAuthForms(departmentId2, year);
         return Json(groupedFreeAuthForms, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetLicenseTypes()
      {
         var data = XMLHelper.GetLicenseTypes();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetTerritories()
      {
         var data = XMLHelper.GetTerritories();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetOrgStructures()
      {
         var data = XMLHelper.GetOrgStructures();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetRegions()
      {
         var data = XMLHelper.GetRegions();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetSubRegions(string code)
      {
         var data = Region.GetSubRegions(code);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetCarriers()
      {
         var data = XMLHelper.GetCarriers();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [Authorize]
      public JsonResult GetMessages()
      {
         var data = WebHelper.GetMessages();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      [Authorize]
      public JsonResult DeleteMessage(Guid messageUID)
      {
         var data = WebHelper.DeleteMessageAndGetData(messageUID);
         UserHelper.SetMessageCount(data.Count);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetDomesticRoutes(string depCode, string destCode, int pageNum, int pageSize)
      {
         var data = WebHelper.GetDomesticRoutes(depCode, destCode, pageNum, pageSize);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult SendMessage(Contact model)
      {
         var dictionaryValue = Umbraco.GetDictionaryValue("Contact.From");
         var dictionaryValue2 = Umbraco.GetDictionaryValue("Contact.To");
         var flag = MailHelper.SendMessage(model.Name, model.Email, model.Message, dictionaryValue, dictionaryValue2);
         return Json(flag, JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetSearchParameters()
      {
         return Json(typeof(SearchParameter).GetEnumNamesDescriptions(), JsonRequestBehavior.AllowGet);
      }

      public JsonResult GetVehicleTypes()
      {
         var result = BIZApplicationInitializer.RunInCurrentSession(() =>
            ScriptLibrary.Global.DC.dcr_VehicleType.Where(x => x.STATE != ObjectState.DELETED)
               .Select(x => new VehicleType
               {
                  Code = x.Code, Name = x.Name
               }).ToList());

         return Json(result, JsonRequestBehavior.AllowGet);
      }
      
      public JsonResult GetPermissionDocTypes()
      {
         var result = BIZApplicationInitializer.RunInCurrentSession(() =>
            ScriptLibrary.Global.DC.dcr_PermissionDocType.Where(x => x.STATE != ObjectState.DELETED)
               .Select(x => new PermissionDocType
               {
                  Code = x.Code,
                  Name = x.Name
               }).ToList());

         return Json(result, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetCarrierEECMessages(DateTime dateFrom, DateTime dateTo)
      {
         var carrier = UserHelper.CurrentCarrier;
         if (carrier == null) throw new Exception("Please, Authentificate");

         var messageFilter = new EECMessageFilter
         {
            MessageDateFrom = dateFrom,
            MessageDateTo = dateTo,
            UserIds = new[] {carrier.UID},
            MessageTypes = EnuEECMessageType.AllElements.Where(x => x.CommonProcess == EnuEECCommonProcess.PRS01)
               .Select(x => x.Code).ToArray()
         };

         var serviceReference = SettingsProvider.GetEECExchangeService();

         var messagesResult = serviceReference.GetEECMessagesDocumentsByFilter(messageFilter, new EECDocumentFilter());
         if (messagesResult.HasError)
            throw messagesResult.Error;

         var countries = BIZApplicationInitializer.RunInCurrentSession(() =>
            ScriptLibrary.Global.DC.dcr_Countries.Where(x => x.STATE != ObjectState.DELETED).Select(
               x => new Country
               {
                  UID = x.UID, Name = x.NameRus, Code2 = x.Code2, CodeISO = x.CodeISO, Code3 = x.Code3, InEAC = x.InEAC,
                  Currency = x.Currency.NameRus, Email = x.Email, ProvidesAuthForms = x.ProvidesAuthForms
               }
            ).ToArray());
         var result = messagesResult.Result.OrderByDescending(x => x.MessageDate)
            .Select(x => new TransportControlMessage(x, countries));
         return Json(result, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult SendRequest(TransportControlRequestModel model)
      {
         var carrier = UserHelper.CurrentCarrier;
         if (carrier == null) throw new Exception("Please, Authentificate");

         var errors = new List<string>();

         if (Enum.GetValues(typeof(SearchParameter)).Cast<SearchParameter>().All(x => x != model.SearchParam))
            errors.Add("Не заполнено поле «Параметр поиска»");
         else if (model.SearchParam == SearchParameter.Vehicle)
         {
            if (model.StartDateTime <= ExpressionNullValues.DateTime)
               errors.Add("Не заполнено поле «Начальная дата/время»");
            if (model.EndDateTime <= ExpressionNullValues.DateTime)
               errors.Add("Не заполнено поле «Конечная дата/время»");
            if (model.StartDateTime > model.EndDateTime)
               errors.Add("Поле «Начальная дата/время» не должно быть больше чем «Конечная дата/время»");
            if (model.VehicleType <= 0)
               errors.Add("Не заполнено поле «Тип транспортного средства»");
            if (model.VehicleType > 0 && !BIZApplicationInitializer.RunInCurrentSession(() =>
                   ScriptLibrary.Global.DC.dcr_VehicleType.Any(x => x.STATE != ObjectState.DELETED && x.Code == model.VehicleType)))
               errors.Add("В поле «Тип транспортного средства» заполнен невалидный код ТС");
            if (string.IsNullOrEmpty(model.VehicleRegId))
               errors.Add("Не заполнено поле «Регистрационный номер АТС»");
            if (!string.IsNullOrEmpty(model.VehicleRegId) && !Regex.IsMatch(model.VehicleRegId, @"[A-Z\/0-9]{0,40}"))
               errors.Add("Поле «Регистрационный номер АТС» имеет неправильный формат");
         }
         else if (model.SearchParam == SearchParameter.AccountNumber ||
                  model.SearchParam == SearchParameter.ViolationNotificationNumber)
         {
            if (string.IsNullOrEmpty(model.Country) || model.Country.Length != 3 || string.IsNullOrEmpty(model.Post) ||
                model.Post.Length != 8 ||
                string.IsNullOrEmpty(model.FormDate) || model.FormDate.Length != 6 ||
                string.IsNullOrEmpty(model.FormNum) || model.FormNum.Length != 5 ||
                !BIZApplicationInitializer.RunInCurrentSession(() =>
                   ScriptLibrary.Global.DC.dcr_Countries.Any(x => x.STATE != ObjectState.DELETED && x.CodeISO == model.Country) &&
                   ScriptLibrary.Global.DC.dcr_Posts.Any(x => x.STATE != ObjectState.DELETED && x.Code == model.Post)) ||
                !DateTime.TryParseExact(model.FormDate, "ddMMyy", CultureInfo.InvariantCulture,
                   DateTimeStyles.AssumeLocal, out _)
            )
               errors.Add("«Номер учетного талона или уведомления» не заполнено, или имеет неверный формат");
         }
         else if (model.SearchParam == SearchParameter.TravelPermit)
         {
            if (string.IsNullOrEmpty(model.PermitDocType))
               errors.Add("Не заполнено поле «Код вида документа»");
            if (!string.IsNullOrEmpty(model.PermitDocType) && !BIZApplicationInitializer.RunInCurrentSession(() =>
                   ScriptLibrary.Global.DC.dcr_PermissionDocType.Any(x =>
                      x.STATE != ObjectState.DELETED && x.Code == model.PermitDocType)))
               errors.Add("В поле «Код вида документа» указан невалидный код вида документа");
            if (string.IsNullOrEmpty(model.PermDocNum))
               errors.Add("Не заполнено поле «Номер документа»");
            if (model.PermDocDate <= ExpressionNullValues.DateTime)
               errors.Add("Не заполнено поле «Дата документа»");
         }

         if (model.QueryCountry == null || model.QueryCountry.Length == 0)
            errors.Add("Для отправки сообщения необходимо выбрать хотя бы одну страну назначения запроса");

         if (!errors.Any())
         {
            var doc = new TransportInspectionResultRequestDetailsType
            {
               EDocHeader = new EDocHeaderType
               {
                  InfEnvelopeCode = EnuEECMessageType.PRS01MSG008.Code,
                  EDocCode = EnuEECDocumentType.RTTRS01_003.DocumentCode,
                  EDocDateTime = DateTime.Now
               }
            };

            if (model.SearchParam == SearchParameter.Vehicle)
            {
               doc.ReportPeriodDetails = new PeriodDetailsType {StartDateTime = model.StartDateTime, EndDateTime = model.EndDateTime};
               doc.VehicleKindCode = new UnifiedCode20Type
                  {Value = model.VehicleType.ToString(CultureInfo.InvariantCulture), codeListId = "P.RS.01.CLS.007"};
               doc.TransportMeansRegId = new TransportMeansRegIdType
                  {Value = model.VehicleRegId};
            }
            else if (model.SearchParam == SearchParameter.AccountNumber)
            {
               var countryCode = BIZApplicationInitializer.RunInCurrentSession(() =>
                  ScriptLibrary.Global.DC.dcr_Countries
                     .First(x => x.STATE != ObjectState.DELETED && x.CodeISO == model.Country).Code2);
               doc.TransportRegTicketIdDetails = new IssuedDocIdDetailsType
               {
                  UnifiedCountryCode = new UnifiedCountryCodeType {Value = countryCode, codeListId = "P.CLS.001"},
                  BorderCheckpointCode = model.Post,
                  EventDateCode = model.FormDate,
                  FormNumberId = model.FormNum
               };
            }
            else if (model.SearchParam == SearchParameter.ViolationNotificationNumber)
            {
               var countryCode = BIZApplicationInitializer.RunInCurrentSession(() =>
                  ScriptLibrary.Global.DC.dcr_Countries
                     .First(x => x.STATE != ObjectState.DELETED && x.CodeISO == model.Country).Code2);
               doc.ViolationNoticeIdDetails = new IssuedDocIdDetailsType
               {
                  UnifiedCountryCode = new UnifiedCountryCodeType { Value = countryCode, codeListId = "P.CLS.001" },
                  BorderCheckpointCode = model.Post,
                  EventDateCode = model.FormDate,
                  FormNumberId = model.FormNum
               };
            }
            else if (model.SearchParam == SearchParameter.TravelPermit)
            {
               var docKind = BIZApplicationInitializer.RunInCurrentSession(() =>
                  ScriptLibrary.Global.DC.dcr_PermissionDocType.Where(x => x.STATE != ObjectState.DELETED && x.Code == model.PermitDocType)
                     .Select(x => new {x.Code, x.Name}).First());
               doc.TransportPermitDocDetails = new TransportPermitDocDetailsType
               {
                  DocKindCode = new UnifiedCode20Type {Value = docKind.Code, codeListId = "P.RS.01.CLS.002"},
                  DocKindName = docKind.Name,
                  DocId = model.PermDocNum,
                  DocCreationDate = model.PermDocDate
               };
            }

            var messagesSender = SettingsProvider.GetEECMessagesProvider();
            
            Debug.Assert(model.QueryCountry != null, "model.QueryCountry != null");
            
            foreach (var country in model.QueryCountry)
            {
               doc.EDocHeader.EDocId = Guid.NewGuid().ToString("D");
               var result = messagesSender.SendMessage(
                  new[]
                  {
                     Tuple.Create<object, EECDocProperty[]>(doc,
                        new[]
                        {
                           new EECDocProperty
                           {
                              FieldName = "SearchParameter", FieldType = FieldType.Numeric,
                              ValueNumber = (int) model.SearchParam
                           }
                        })
                  }, 
                  EnuEECMessageType.PRS01MSG008, EnuEECProcedure.PRS01PRC006,
                  EnuEECTransaction.PRS01TRN004, country, EnuEECParticipant.PRS01ACT003, EnuEECParticipant.PRS01ACT002);
               if (!result) errors.Add($"Сообщение не было отправлено в {country}");
            }
         }

         return Json(new {Errors = errors}, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetDocDataHtml(string documentId)
      {
         var carrier = UserHelper.CurrentCarrier;
         if (carrier == null) throw new Exception("Please, Authentificate");

         string error = null;
         var html = string.Empty;
         var evalExpression = string.Empty;

         var messagesSender = SettingsProvider.GetEECMessagesProvider();
         var result = messagesSender.GetMessageInfoByDocumentID(documentId, true);
         object doc = null;

         if (result == null)
            error = "Данные по запрашиваемому документу не найдены";
         else
         {
            var docInfo = result.Documents.FirstOrDefault();
            if (docInfo == null)
               error = "Документ не найден";
            else
            {
               try
               {
                  var docType = EnuEECDocumentType.AllElements.First(x => x.Code == docInfo.DocumentType);
                  doc = Serializer.Deserialize(EncodingCompressProvider.GetStringFromCompressedData(docInfo.Data),
                     docType.ModelType);
               }
               catch
               {
                  error = "Ошибка в данных";
               }
            }
         }

         if (result != null)
         {
            if (result.UserId != carrier.UID)
               error = "Запрашиваемый документ принадлежит другому пользователю";
            else if (doc != null)
            {
               var mapper = new DocTypeMapper();
               var vmType = mapper.GetViewModelType(doc.GetType());
               if (vmType != null)
               {
                  var controls = ControlGenerationProvider.GetAllControls(vmType);
                  html = AngularJSPreviewFormGenerator.GenerateHtml(controls, 2, "previewData", "$scope.previewScope",
                     out evalExpression);
               }
            }
         }

         return Json(new {html, evalExpression, data = doc, error}, JsonRequestBehavior.AllowGet);
      }
      
      [HttpPost]
      public JsonResult GetResponseDocHtml(string documentId)
      {
         var carrier = UserHelper.CurrentCarrier;
         if (carrier == null) throw new Exception("Please, Authentificate");

         string error = null;
         var html = string.Empty;
         var evalExpression = string.Empty;

         var messagesSender = SettingsProvider.GetEECMessagesProvider();
         var message = messagesSender.GetMessageInfoByDocumentID(documentId);

         if (message == null)
            error = "Данные по запрашиваемому документу не найдены";
         else
         {
            var messageType = EnuEECMessageType.AllElements.FirstOrDefault(x => x.Code == message.MessageType);
            if (messageType == null || messageType.MessageCategory != EnuMessageCategory.Request)
               error = "Сообщение не является запросом";
         }

         var responseDocuments = messagesSender.CheckRequest(documentId, out var notFound, out var responseMessage);
         var doc = responseDocuments?.FirstOrDefault();
         if (notFound || doc == null || responseMessage == null)
            error = "Ответное сообщение не найдено";
         else
         {
            if (responseMessage.UserId != carrier.UID)
               error = "Запрашиваемый документ принадлежит другому пользователю";
            else
            {
               var mapper = new DocTypeMapper();
               var vmType = mapper.GetViewModelType(doc.GetType());
               if (vmType != null)
               {
                  var controls = ControlGenerationProvider.GetAllControls(vmType);
                  html = AngularJSPreviewFormGenerator.GenerateHtml(controls, 2, "previewData", "$scope.previewScope",
                     out evalExpression);
               }
            }
         }

         return Json(new {html, evalExpression, data = doc, error}, JsonRequestBehavior.AllowGet);
      }

      protected override void OnException(ExceptionContext filterContext)
      {
         base.OnException(filterContext);
         if (filterContext.Exception != null)
            Logging.Logger.Error(filterContext.Exception);
      }
   }
}