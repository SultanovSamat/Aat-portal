using System.Web;
using System.Web.Security;
using AkfortaWeb.Models;

namespace AkfortaWeb.Helpers
{
    public static class UserHelper
    {
        public const string SessionKeyCurrentCarrier = "CurrentCarrier";

        public static bool IsAuthenticated => CurrentCarrier != null && !CurrentCarrier.IsArchive;

        public static bool IsArchive => CurrentCarrier != null && CurrentCarrier.IsArchive;

        public static string GetCarrierName()
        {
            if (!IsAuthenticated) return string.Empty;
            return CurrentCarrier.Name;
        }

        public static int GetMessageCount()
        {
            if (!IsAuthenticated) return 0;
            return CurrentCarrier.MessageCount;
        }

        public static string GetEMail()
        {
           return !IsAuthenticated ? string.Empty : CurrentCarrier.EMail;
        }

        public static Carrier CurrentCarrier
        {
            get
            {
                if (HttpContext.Current == null) return null;

                var carrier = HttpContext.Current.Session[SessionKeyCurrentCarrier] as Carrier;
                return carrier;
            }
            set
            {
                if (HttpContext.Current == null) return;
                HttpContext.Current.Session[SessionKeyCurrentCarrier] = value;
            }
        }

        public static bool IsLoginnedWith(string name, string password)
        {
            CurrentCarrier = WebHelper.GetCarrier(name, password);
            return IsAuthenticated;
        }

        public static bool ChangePassword(string curPassword, string newPassword, string newPasswordRepeat)
        {
            return WebHelper.ChangePassword(CurrentCarrier.UID, curPassword, newPassword, newPasswordRepeat);
        }

        public static bool ChangeLogin(string curPassword, string newLogin, string newLoginRepeat)
        {
            return WebHelper.ChangeLogin(CurrentCarrier.UID, curPassword, newLogin, newLoginRepeat);
        }

        internal static bool ChangeEmail(string curPassword, string newEmail, string newEmailRepeat)
        {
            return WebHelper.ChangeEmail(CurrentCarrier.UID, curPassword, newEmail, newEmailRepeat);
        }

        public static void LogOut()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }

        internal static void SetMessageCount(int messageCount)
        {
            if (!IsAuthenticated) return;
            CurrentCarrier.MessageCount = messageCount;
        }
    }
}
