using System;
using System.Collections.Generic;
using AkfortaWeb.Helpers;
using ScriptLibrary;

namespace AkfortaWeb.Models
{
    [Serializable]
    public class ControlParameter
    {
        public string Country { get; set;}

        public string Desc { get; set; }

        public string Check { get; set; }

        public static List<ControlParameter> Deserialize(IMainWebService client, Guid routeUID, Guid vehRegCountryUID, int vehPurpose, int busTripType, int cargoPresence)
        {
            return SerializeHelper.Deserialize<List<ControlParameter>>(client.GetTCParameters(routeUID, vehRegCountryUID, vehPurpose, busTripType, cargoPresence));
        }
    }
}
