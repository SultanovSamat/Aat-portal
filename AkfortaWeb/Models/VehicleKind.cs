using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
    [Serializable]
    [XmlRoot("VehicleKinds"), XmlType("VehicleKind")]
    public class VehicleKind
    {
        public Guid UID { get; set; }

        public string Name { get; set; }

        public string VehiclePurpose { get; set; }

        public VehicleKind() { }

        public VehicleKind(Guid uid)
        {
            UID = uid;
        }

        public VehicleKind(DataRow row)
        { 
            UID = Guid.Parse(row[0].ToString());
            Name = row[1].ToString();
            VehiclePurpose = row[2].ToString();
        }

        public static void SetXmlData(string file)
        {
            if (XmlData == null || SerializeHelper.ShouldUpdate(file))
                XmlData = SerializeHelper.GetXmlData<VehicleKind>(file);
        }

        public static List<VehicleKind> GetXmlData()
        {
            SetXmlData("vehicleKinds");
            return XmlData;
        }

        private static List<VehicleKind> XmlData { get; set; }
    }
}
