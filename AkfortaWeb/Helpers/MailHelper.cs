using System;
using System.Net.Mail;

namespace AkfortaWeb.Helpers
{
   public static class MailHelper
   {
      public static bool SendMessage(string name, string email, string body, string from, string to)
      {
         try
         {
            var mailMessage = new MailMessage {From = new MailAddress(from)};
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = name + " <" + email + ">";
            mailMessage.Body = body;
            var smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
            mailMessage.Dispose();
         }
         catch (Exception)
         {
            return false;
         }

         return true;
      }
   }
}