using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AkfortaWeb.Models;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Controllers
{
   public class LicensesController : Umbraco.Web.Mvc.SurfaceController
   {
      [HttpPost]
      public JsonResult GetFilteredCarriers(string INN, string shortName, string orgForm, int pageNum = 1,
         int pageSize = 20)
      {
         var data = XMLHelper.GetCarriers();
         var filteredData = data;
         if (!string.IsNullOrEmpty(INN))
            filteredData = filteredData.Where(x => x.INN.ToLower().StartsWith(INN.ToLower())).ToList();
         if (!string.IsNullOrEmpty(shortName))
            filteredData = filteredData.Where(x => x.ShortName.ToLower().StartsWith(shortName.ToLower())).ToList();
         if (!string.IsNullOrEmpty(orgForm))
            filteredData = filteredData.Where(x => x.OrgStructure == orgForm).ToList();
         var count = filteredData.Count;
         var skipBegin = (pageNum - 1) * pageSize;
         if (skipBegin < 0) skipBegin = 0;
         if (pageSize <= 0) pageSize = 20;
         filteredData = filteredData.Skip(skipBegin).Take(pageSize).ToList();
         return new JsonResult
         {
            Data = new {Carriers = filteredData, Count = count}, MaxJsonLength = int.MaxValue,
            JsonRequestBehavior = JsonRequestBehavior.AllowGet
         };
      }

      [HttpPost]
      public JsonResult GetFilteredLicenseDocsConditions(Guid uidLicenseType, int carrierCategory)
      {
         var data = WebHelper.GetLicenseDocsConditons(uidLicenseType, carrierCategory);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      public JsonResult GetCarrierLicenses(Guid uidCarrier)
      {
         var data = WebHelper.GetCarrierLicenses(uidCarrier);
         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [Authorize]
      [HttpGet]
      public JsonResult GetCarrierLicenses()
      {
         var data = new List<LicenseModel>();
         if (UserHelper.IsAuthenticated)
         {
            var uidCarrier = UserHelper.CurrentCarrier.UID;
            data = WebHelper.GetCarrierLicenses(uidCarrier);
         }

         return Json(data, JsonRequestBehavior.AllowGet);
      }

      [Authorize]
      [HttpPost]
      public JsonResult GetCarrierLicenseForms(Guid uidLicense)
      {
         var data = new List<LicenseForm>();
         if (UserHelper.IsAuthenticated) data = WebHelper.GetCarrierLicenseForms(uidLicense);

         return Json(data, JsonRequestBehavior.AllowGet);
      }
   }
}