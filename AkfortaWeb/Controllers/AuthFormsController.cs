using System;
using System.Web.Mvc;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Controllers
{
   public class AuthFormsController : Umbraco.Web.Mvc.SurfaceController
   {
      [HttpPost]
      public JsonResult GetFormCarriers(string INN, string shortName, string orgForm, Guid? countryUID, int pageNum = 1,
         int pageSize = 20)
      {
         var countryId = countryUID ?? Guid.Empty;
         var grData = WebHelper.GetFormCarriers(INN, shortName, orgForm, countryId, pageNum, pageSize);
         var filteredData = grData?.Data;
         var count = grData?.Count;
         return new JsonResult
         {
            Data = new {Carriers = filteredData, Count = count}, MaxJsonLength = int.MaxValue,
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
         };
      }

      [HttpPost]
      public JsonResult GetCarrierForms(Guid uidCarrier, Guid? uidCountry)
      {
         var countryId = uidCountry ?? Guid.Empty;
         var data = WebHelper.GetCarrierForms(uidCarrier, countryId);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpGet]
      public JsonResult GetQueueData()
      {
         var data = WebHelper.GetQueueData();
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult TakeQueue(string inn, DateTime date)
      {
         var data = WebHelper.TakeQueue(inn, date);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetQueues(DateTime date)
      {
         var data = WebHelper.GetQueues(date);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetMineQueues(string inn)
      {
         var data = WebHelper.GetMineQueues(inn);
         return Json(data, JsonRequestBehavior.AllowGet);
      }
   }
}