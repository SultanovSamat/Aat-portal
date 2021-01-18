using System.Web.Mvc;
using System.Web.Security;
using AkfortaWeb.Models;
using AkfortaWeb.Helpers;


namespace AkfortaWeb.Controllers
{
   public class AccountController : Umbraco.Web.Mvc.SurfaceController
   {
      [HttpGet]
      public ActionResult Logout()
      {
         UserHelper.LogOut();
         return Redirect("/");
      }

      [HttpPost]
      public ActionResult Login(MemberLoginModel model, string returnUrl)
      {
         if (ModelState.IsValid)
         {
            if (UserHelper.IsLoginnedWith(model.Name, model.Password))
            {
               FormsAuthentication.SetAuthCookie(model.Name, true);
            }
            else if (UserHelper.IsArchive)
            {
               TempData["StatusLogin"] = "Данный перевозчик переведен в архив!";
            }
            else
            {
               TempData["StatusLogin"] = "Нет доступа к системе или такой пользователь не зарегистрирован!";
            }
         }
         else
         {
            TempData["StatusLogin"] = "Неверно указано имя пользователя или пароль!";
         }

         return RedirectToCurrentUmbracoPage();
      }

      [Authorize]
      [HttpPost]
      public ActionResult ChangePassword(string curPassword, string newPassword, string newPasswordRepeat)
      {
         if (string.IsNullOrEmpty(curPassword))
         {
            TempData["StatusPassword"] = "Введите пароль!";
         }
         else if (newPassword != newPasswordRepeat)
         {
            TempData["StatusPassword"] = "Пароли не совпадают!";
         }
         else if (UserHelper.ChangePassword(curPassword, newPassword, newPasswordRepeat))
         {
            TempData["StatusPassword"] = "Пароль успешно изменен!";
         }
         else
         {
            TempData["StatusPassword"] =
               "Пароль не был изменен! Повторите операцию позже или обратитесь в отдел лицензирования!";
         }

         return RedirectToCurrentUmbracoPage();
      }

      [Authorize]
      [HttpPost]
      public ActionResult ChangeLogin(string curPassword, string newLogin, string newLoginRepeat)
      {
         if (string.IsNullOrEmpty(curPassword))
         {
            TempData["Status"] = "Введите пароль!";
         }
         else if (newLogin != newLoginRepeat)
         {
            TempData["Status"] = "Логины не совпадают!";
         }
         else if (UserHelper.ChangeLogin(curPassword, newLogin, newLoginRepeat))
         {
            TempData["Status"] = "Логин успешно изменен!";
         }
         else
         {
            TempData["Status"] =
               "Логин не был изменен! Повторите операцию позже или обратитесь в отдел лицензирования!";
         }

         return RedirectToCurrentUmbracoPage();
      }

      [Authorize]
      [HttpPost]
      public ActionResult ChangeEmail(string curPassword, string newEmail, string newEmailRepeat)
      {
         if (string.IsNullOrEmpty(curPassword))
         {
            TempData["StatusEmail"] = "Введите e-Mail!";
         }
         else if (newEmail != newEmailRepeat)
         {
            TempData["StatusEmail"] = "Электронные адреса не совпадают!";
         }
         else if (UserHelper.ChangeEmail(curPassword, newEmail, newEmailRepeat))
         {
            TempData["StatusEmail"] = "e-Mail успешно изменен!";
         }
         else
         {
            TempData["StatusEmail"] =
               "e-Mail не был изменен! Повторите операцию позже или обратитесь в отдел лицензирования!";
         }

         return RedirectToCurrentUmbracoPage();
      }
   }
}