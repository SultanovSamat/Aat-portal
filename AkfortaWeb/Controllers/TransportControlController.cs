using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AkfortaWeb.Models;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Controllers
{
    public class TransportControlController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public JsonResult GetFilteredCheckPoints(Guid countryUID, bool check, bool external)
        {
            List<CheckPoint> filteredCheckPoints = new List<CheckPoint>();
            if (check || external)
            {
                var checkPoints = XMLHelper.GetCheckPoints();
                var countryCheckPoints = checkPoints.Where(x => x.CountryUID == countryUID).ToList();
                if (check && external)
                {
                    filteredCheckPoints = countryCheckPoints;
                }
                else if (check)
                {
                    filteredCheckPoints = countryCheckPoints.Where(x => x.ViolationCheckpoint).ToList();
                }
                else if (external)
                {
                    filteredCheckPoints = countryCheckPoints.Where(x => x.ExternalCheckpoint).ToList();
                }
            }
            var data = filteredCheckPoints;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLicenseForm(string licFormNumber)
        {
            LicenseForm licenseForm = WebHelper.GetLicenseForm(licFormNumber);
            return base.Json(licenseForm, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetFines(Guid uidVehicle)
        {
            var data = WebHelper.GetFines(uidVehicle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetVehicleViolations(Guid uidVehicle)
        {
            var data = WebHelper.GetVehicleViolations(uidVehicle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetViolations(string notificationNumber)
        {
            var data = WebHelper.GetViolations(notificationNumber);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMainCountryCodeIso()
        {
            var data = WebHelper.GetMainCountryCodeIso();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredTCParameters(Guid routeUID, Guid vehRegCountryUID, int vehPurpose, int busTripType, int cargoPresence)
        {
            var data = WebHelper.GetFilteredTCParameters(routeUID, vehRegCountryUID, vehPurpose, busTripType, cargoPresence);
            var groupedData = data.GroupBy(x => x.Country).Select(y => new
            {
                Name = y.Key,
                Checks = y.Select(z => new { Country = z.Country, Check = z.Check, Desc = z.Desc }).ToArray()
            });
            return Json(groupedData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredScheduledInspections(int begYear, int endYear, Guid departmentUID, string Name, string INN)
        {
            object data = WebHelper.GetScheduledInspections(begYear, endYear, departmentUID, Name, INN);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}