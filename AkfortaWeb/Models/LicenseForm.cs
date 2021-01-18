using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
    [Serializable]
    public class LicenseForm
    {
        public Guid UID { get; set; }
        public string Department { get; set; }
        public string LicFormNumber { get; set; }
        public string Receipt { get; set; }
        public string IssuedDate { get; set; }
        public DateTime ValidityStart { get; set; }
        public DateTime ValidityEnd { get; set; }
        public string SumToPay { get; set; }
        public string Vehicle { get; set; }
        public string Trailer { get; set; }
        public string Validity => $"{ValidityStart:dd.MM.yyyy} - {ValidityEnd:dd.MM.yyyy}";

        public DateTime LicFormIssuedDate { get; set; }
        public string LicFormIssuedDateStr
        {
            get
            {
                if (LicFormIssuedDate.Year < 1900) return string.Empty;
                return LicFormIssuedDate.ToString("dd.MM.yyyy");
            }
        }
        public string INN { get; set; }
        public string LicenseNum { get; set; }
        public string LicenseType { get; set; }
        public string CoveredTer { get; set; }
        public DateTime LicValidityStart { get; set; }
        public DateTime LicValidityEnd { get; set; }
        public string LicValidity => $"{LicValidityStart:dd.MM.yyyy} - {LicValidityEnd:dd.MM.yyyy}";
        public string LicStatus { get; set; }
        public bool IsCancelled { get; set; }

        public static List<LicenseForm> Deserialize(IMainWebService client, Guid uidLicense)
        {
           return SerializeHelper.Deserialize<List<LicenseForm>>(client.GetCarrierLicenseForms(uidLicense));
        }

        public static LicenseForm DeserializeByLicForm(IMainWebService client, string licFormNumber)
        {
           return SerializeHelper.Deserialize<LicenseForm>(client.GetLicenseForm(licFormNumber));
        }
    }
}