using System;

namespace AkfortaWeb.Models
{
   public class TransportControlRequestModel
   {
      public SearchParameter SearchParam { get; set; }


      public DateTime StartDateTime { get; set; }

      public DateTime EndDateTime { get; set; }

      public int VehicleType { get; set; }

      public string VehicleRegId { get; set; }


      public string Country { get; set; }

      public string Post { get; set; }

      public string FormDate { get; set; }

      public string FormNum { get; set; }


      public string PermitDocType { get; set; }

      public string PermDocNum { get; set; }
      
      public DateTime PermDocDate { get; set; }
         
      
      public string[] QueryCountry { get; set; }
   }
}