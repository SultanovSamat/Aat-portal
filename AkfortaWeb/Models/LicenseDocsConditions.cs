using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
    [Serializable]
    public class LicenseDocsConditions
    {
        public Guid UID { get; set; }

        public List<LicenseDoc> Docs {get; set;}
        public List<SpecialistPosition> Positions { get; set; }

        public List<LicenseDoc> VehicleDocs { get; set; }

        public string Conditions {get; set;}

        public static LicenseDocsConditions Deserialize(IMainWebService client, Guid uidLicenseType, int carrierCategory)
        {
            return SerializeHelper.Deserialize<LicenseDocsConditions>(client.GetLicenseDocsConditions(uidLicenseType, carrierCategory));
        }
    }

    [Serializable]
    public class LicenseDoc
    {
        public Guid UID { get; set; }
        public string Name {get; set;}
        public string Comment
        {
            get;
            set;
        }

        public bool ShowComment => Comment.Trim() != string.Empty;
    }

    [Serializable]
    public class SpecialistPosition
    {
        public Guid UID { get; set; }

        public string Position { get; set; }

        public List<LicenseDoc> Documents { get; set; }
    }
}