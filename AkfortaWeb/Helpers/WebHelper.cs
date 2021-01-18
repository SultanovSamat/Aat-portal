using System;
using System.Collections.Generic;
using AkfortaWeb.Models;
using System.IO;
using BIZ.ExternalIntegration.ASP.MVC;
using ScriptLibrary;

namespace AkfortaWeb.Helpers
{
    public static class WebHelper
    {
        public static Carrier GetCarrier(string name, string password)
        {
            return Carrier.Deserialize(MainClient, name, password);
        }

        internal static bool ChangePassword(Guid uidCarrier, string curPassword, string newPassword, string newPasswordRepeat)
        {
            return Carrier.ChangePassword(MainClient, uidCarrier, curPassword, newPassword, newPasswordRepeat);
        }

        internal static bool ChangeLogin(Guid uidCarrier, string curPassword, string newLogin, string newLoginRepeat)
        {
            return Carrier.ChangeLogin(MainClient, uidCarrier, curPassword, newLogin, newLoginRepeat);
        }

        internal static bool ChangeEmail(Guid uidCarrier, string curPassword, string newEmail, string newEmailRepeat)
        {
            return Carrier.ChangeEmail(MainClient, uidCarrier, curPassword, newEmail, newEmailRepeat);
        }

        internal static List<Message> GetMessages()
        {
            return Message.Deserialize(MainClient, UserHelper.CurrentCarrier.UID);
        }

        internal static List<Message> DeleteMessageAndGetData(Guid messageUID)
        {
            return Message.DeleteAndDeserialize(MainClient, UserHelper.CurrentCarrier.UID, messageUID);
        }

        public static List<LicenseModel> GetCarrierLicenses(Guid uidCarrier)
        {
            return LicenseModel.Deserialize(MainClient, uidCarrier);
        }

        public static List<LicenseForm> GetCarrierLicenseForms(Guid uidLicense)
        {
            return LicenseForm.Deserialize(MainClient, uidLicense);
        }

        public static LicenseDocsConditions GetLicenseDocsConditons(Guid uidLicenseType, int carrierCategory)
        {
            return LicenseDocsConditions.Deserialize(MainClient, uidLicenseType, carrierCategory);
        }

        internal static List<AuthForm> GetFreeAuthForms()
        {
            return AuthForm.Deserialize(MainClient);
        }

        public static GroupedCarrier GetFormCarriers(string inn, string name, string orgForm, Guid countryUID, int pageNum, int pageSize)
        {
            return AuthForm.DeserializeFormCarriers(MainClient, inn, name, orgForm, countryUID, pageNum, pageSize);
        }

        public static List<GroupedAuthForm> GetCarrierForms(Guid uidCarrier, Guid uidCountry)
        {
            return GroupedAuthForm.DeserializeCarrierForms(MainClient, uidCarrier, uidCountry);
        }

        public static QueueLicData GetQueueData()
        {
            return QueueLic.DeserializeQueueData(MainClient);
        }

        public static QueueLicMsg TakeQueue(string inn, DateTime date)
        {
            return QueueLic.DeserializeTakeQueue(MainClient, inn, date);
        }

        public static List<QueueLic> GetQueues(DateTime date)
        {
            return QueueLic.DeserializeGetQueues(MainClient, date);
        }

        public static QueueLicMsg GetMineQueues(string inn)
        {
            return QueueLic.DeserializeGetMineQueues(MainClient, inn);
        }

        internal static List<GroupedAuthForm> GetGroupedFreeAuthForms(Guid departmentId, int year)
        {
            return GroupedAuthForm.Deserialize(MainClient, departmentId, year);
        }


        internal static List<ControlParameter> GetFilteredTCParameters(Guid routeUID, Guid vehRegCountryUID, int vehPurpose, int busTripType, int cargoPresence)
        {
            return ControlParameter.Deserialize(MainClient, routeUID, vehRegCountryUID, vehPurpose, busTripType, cargoPresence);
        }

        internal static List<VehicleLT> GetVehicles()
        {
            return VehicleLT.Deserialize(MainClient, UserHelper.CurrentCarrier.UID);
        }

        internal static List<SpecialistLT> GetSpecialists()
        {
            return SpecialistLT.Deserialize(MainClient, UserHelper.CurrentCarrier.UID);
        }

        internal static List<Fine> GetFines(Guid uidVehicle)
        {
            return Fine.Deserialize(MainClient, uidVehicle);
        }
        internal static LicenseForm GetLicenseForm(string licFormNumber)
        {
            return LicenseForm.DeserializeByLicForm(MainClient, licFormNumber);
        }

        internal static List<Violation> GetVehicleViolations(Guid uidVehicle)
        {
            return Violation.Deserialize(MainClient, uidVehicle);
        }

        internal static List<Violation> GetViolations(string notificationNumber)
        {
            return Violation.DeserializeByNumber(MainClient, notificationNumber);
        }

        internal static List<DomesticRoute> GetDomesticRoutes(string depCode, string destCode, int pageNum, int pageSize)
        {
            return DomesticRoute.Deserialize(MainClient, depCode, destCode, pageNum, pageSize);
        }

        internal static List<ScheduledInspection> GetScheduledInspections(int begYear, int endYear, Guid departmentUID, string name, string inn)
        {
            return ScheduledInspection.Deserialize(MainClient, begYear, endYear, departmentUID, name, inn);
        }

        internal static List<Department> GetSheduledDepartments()
        {
            return Department.Deserialize(MainClient);
        }

        internal static Country GetMainCountryCodeIso()
        {
            return Country.DeserializeCodeIso(MainClient);
        }

        internal static void Ping()
        {
            try
            {
                MainClient.PingService();
            }
            catch (Exception)
            {
            }
        }

        private static IMainWebService _mainService;

        private static IMainWebService MainClient =>
           _mainService ?? (_mainService = new MainWebServiceWrap(new MainWebService()));
    }

    public class MainWebServiceWrap : IMainWebService
    {
       private readonly MainWebService _mainWebService;

       public MainWebServiceWrap(MainWebService mainWebService)
       {
          _mainWebService = mainWebService;
       }

       public Stream PingService()
       {
          return runInSession(x => x.PingService());
       }

       public Stream GetCarrier(string name, string password)
       {
         return runInSession(x => x.GetCarrier(name, password));
      }

       public Stream GetCarrierMessages(Guid uidCarrier)
       {
         return runInSession(x => x.GetCarrierMessages(uidCarrier));
      }

       public Stream GetCarrierLicenses(Guid uidCarrier)
       {
         return runInSession(x => x.GetCarrierLicenses(uidCarrier));
      }

       public Stream GetCarrierLicenseForms(Guid uidLicense)
       {
         return runInSession(x => x.GetCarrierLicenseForms(uidLicense));
      }

       public Stream GetCarrierForms(Guid uidCarrier, Guid uidCountry)
       {
         return runInSession(x => x.GetCarrierForms(uidCarrier, uidCountry));
      }

       public Stream GetFormCarriers(string inn, string name, string orgForm, Guid countryUID, int pageNum, int pageSize)
       {
         return runInSession(x => x.GetFormCarriers(inn, name, orgForm, countryUID, pageNum, pageSize));
      }

       public Stream GetLicenseForm(string licFormNumber)
       {
         return runInSession(x => x.GetLicenseForm(licFormNumber));
      }

       public Stream GetLicenseDocsConditions(Guid uidLicenseType, int carrierCategory)
       {
         return runInSession(x => x.GetLicenseDocsConditions(uidLicenseType, carrierCategory));
      }

       public Stream GetCarrierVehicles(Guid uidCarrier)
       {
         return runInSession(x => x.GetCarrierVehicles(uidCarrier));
      }

       public Stream GetVehicleFines(Guid uidVehicle)
       {
         return runInSession(x => x.GetVehicleFines(uidVehicle));
      }

       public Stream GetVehicleViolations(Guid uidVehicle)
       {
         return runInSession(x => x.GetVehicleViolations(uidVehicle));
      }

       public Stream GetViolations(string notificationNumber)
       {
         return runInSession(x => x.GetViolations(notificationNumber));
      }

       public Stream GetMainCountryCodeIso()
       {
         return runInSession(x => x.GetMainCountryCodeIso());
      }

       public Stream GetCarrierSpecialists(Guid uidCarrier)
       {
         return runInSession(x => x.GetCarrierSpecialists(uidCarrier));
      }

       public Stream DeleteMessageAndGetData(Guid uidCarrier, Guid messageUID)
       {
         return runInSession(x => x.DeleteMessageAndGetData(uidCarrier, messageUID));
      }

       public bool ChangePassword(Guid uidCarrier, string curPassword, string newPassword, string newPasswordRepeat)
       {
         return runInSession(x => x.ChangePassword(uidCarrier, curPassword, newPassword, newPasswordRepeat));
      }

       public bool ChangeLogin(Guid uidCarrier, string password, string newLogin, string newLoginRepeat)
       {
         return runInSession(x => x.ChangeLogin(uidCarrier, password, newLogin, newLoginRepeat));
      }

       public bool ChangeEmail(Guid uidCarrier, string password, string newEmail, string newEmailRepeat)
       {
         return runInSession(x => x.ChangeEmail(uidCarrier, password, newEmail, newEmailRepeat));
      }

       public Stream GetFreeAuthForms()
       {
         return runInSession(x => x.GetFreeAuthForms());
      }

       public Stream GetGroupedFreeAuthForms(Guid departmentId, int year)
       {
         return runInSession(x => x.GetGroupedFreeAuthForms(departmentId, year));
      }

       public Stream GetTCParameters(Guid routeUID, Guid vehRegCountryUID, int vehPurpose, int busTripType, int cargoPresence)
       {
         return runInSession(x => x.GetTCParameters(routeUID, vehRegCountryUID, vehPurpose, busTripType, cargoPresence));
      }

       public Stream GetDomesticRoutes(string depCode, string destCode, int pageNum, int pageSize)
       {
         return runInSession(x => x.GetDomesticRoutes(depCode, destCode, pageNum, pageSize));
      }

       public Stream GetScheduledDepartments()
       {
         return runInSession(x => x.GetScheduledDepartments());
      }

       public Stream GetScheduledInspections(int begYear, int endYear, Guid departmentUID, string Name, string INN)
       {
         return runInSession(x => x.GetScheduledInspections(begYear, endYear, departmentUID, Name, INN));
      }

       public Stream GetQueueData()
       {
         return runInSession(x => x.GetQueueData());
      }

       public Stream TakeQueue(string inn, DateTime date)
       {
         return runInSession(x => x.TakeQueue(inn, date));
      }

       public Stream GetListQueue(DateTime date)
       {
         return runInSession(x => x.GetListQueue(date));
      }

       public Stream GetMineQueues(string inn)
       {
         return runInSession(x => x.GetMineQueues(inn));
       }

       private T runInSession<T>(Func<MainWebService, T> action)
       {
          return BIZApplicationInitializer.RunInCurrentSession(() => action(_mainWebService));
       }
       
       private void runInSession(Action<MainWebService> action)
       {
          BIZApplicationInitializer.RunInCurrentSession(() => action(_mainWebService));
       }
    }
}