using System.ComponentModel;

namespace AkfortaWeb.Models
{
   public enum SearchParameter
   {
      [Description("Транспортное средство")]
      Vehicle = 1,
      [Description("Разрешительный документ на поездку")]
      TravelPermit = 2,
      [Description("Номер учетного талона")]
      AccountNumber = 3,
      [Description("Номер уведомления о несоответствии")]
      ViolationNotificationNumber = 4
   }
}