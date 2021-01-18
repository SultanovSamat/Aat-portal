using System.Collections.Generic;
using AkfortaWeb.Models;

namespace AkfortaWeb.Helpers
{
    public static class XMLHelper
    {
        public static List<Country> GetCountries()
        {
            return Country.GetXmlData();
        }

        public static List<Route> GetRoutes()
        {
            return Route.GetXmlData();
        }

        public static List<CheckPoint> GetCheckPoints()
        {
            return CheckPoint.GetXmlData();
        }

        public static List<VehicleKind> GetVehicleKinds()
        {
            return VehicleKind.GetXmlData();
        }

        public static List<VehicleBrand> GetVehicleBrands()
        {
            return VehicleBrand.GetXmlData();
        }
 
        public static List<Department> GetDepartments()
        {
            return Department.GetXmlData();
        }
  
        public static List<Carrier> GetCarriers()
        {
            return Carrier.GetXmlData();
        }
 
        public static List<OrgStructure> GetOrgStructures()
        {
            return OrgStructure.GetXmlData();
        }

        public static List<Territory> GetTerritories()
        {
            return Territory.GetXmlData();
        }

        public static List<LicenseType> GetLicenseTypes()
        {
            return LicenseType.GetXmlData();
        }

        internal static object[] GetRegions()
        {
           return Region.GetHierarchicalData();
        }
    }
}