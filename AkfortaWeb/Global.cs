using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using BIZ.ExternalIntegration.ASP.MVC.Dependencies;
using EECExchange.Common.Enum;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace AkfortaWeb
{
   public class Global : Umbraco.Web.UmbracoApplication
   {
      public static bool CancelBackGroundTask { get; set; }

      public void Init(HttpApplication application)
      {

      }

      protected override void OnApplicationStarted(object sender, EventArgs e)
      {
         EnumInitializer.Init();
         base.OnApplicationStarted(sender, e);

         #region eLeed Environment initialization (auto genereated code)

         BIZ.ExternalIntegration.ASP.MVC.BIZApplicationInitializer.InitializeEnvironment(new AdvancedResolver(), 300);

         var login = ConfigurationManager.AppSettings["Login"];
         var passwordHash = ConfigurationManager.AppSettings["PasswordHash"];
         BIZ.ExternalIntegration.ASP.MVC.BIZApplicationInitializer.RegisterGlobalSession(login, passwordHash, null,
            true);

         #endregion
      }

      protected override void OnApplicationEnd(object sender, EventArgs e)
      {
         CancelBackGroundTask = true;

         #region Cleaning eLeed Environment (auto genereated code)

         BIZ.ExternalIntegration.ASP.MVC.BIZApplicationInitializer.RemoveGlobalSession();
         BIZ.ExternalIntegration.ASP.MVC.BIZApplicationInitializer.CleanEnvironment();

         #endregion
      }
   }

   public class AdvancedResolver : DependencyResolverConfigMVC
   {
      protected override void RegisterServices(Container container)
      {
         container.Options.ConstructorResolutionBehavior = new MostResolvableParametersConstructorResolutionBehavior(container);
         base.RegisterServices(container);
      }
   }

   public class MostResolvableParametersConstructorResolutionBehavior
    : IConstructorResolutionBehavior
   {
      private readonly Container container;

      public MostResolvableParametersConstructorResolutionBehavior(Container container)
      {
         this.container = container;
      }

      private bool IsCalledDuringRegistrationPhase => !this.container.IsLocked();

      private IEnumerable<ConstructorInfo> GetConstructors(Type implementation) =>
          from ctor in implementation.GetConstructors()
          let parameters = ctor.GetParameters()
          where this.IsCalledDuringRegistrationPhase
              || implementation.GetConstructors().Length == 1
              || ctor.GetParameters().All(this.CanBeResolved)
          orderby parameters.Length descending
          select ctor;

      private bool CanBeResolved(ParameterInfo parameter) =>
          this.GetInstanceProducerFor(new InjectionConsumerInfo(parameter)) != null;

      private InstanceProducer GetInstanceProducerFor(InjectionConsumerInfo i) =>
          this.container.Options.DependencyInjectionBehavior.GetInstanceProducer(i, false);

      private static string BuildExceptionMessage(Type type) =>
          !type.GetConstructors().Any()
              ? TypeShouldHaveAtLeastOnePublicConstructor(type)
              : TypeShouldHaveConstructorWithResolvableTypes(type);

      private static string TypeShouldHaveAtLeastOnePublicConstructor(Type type) =>
          string.Format(CultureInfo.InvariantCulture,
              "For the container to be able to create {0}, it should contain at least " +
              "one public constructor.", type.ToFriendlyName());

      private static string TypeShouldHaveConstructorWithResolvableTypes(Type type) =>
          string.Format(CultureInfo.InvariantCulture,
              "For the container to be able to create {0}, it should contain a public " +
              "constructor that only contains parameters that can be resolved.",
              type.ToFriendlyName());

      public ConstructorInfo GetConstructor(Type implementationType)
      {
         var constructor = this.GetConstructors(implementationType).FirstOrDefault();
         var error = constructor == null ? BuildExceptionMessage(implementationType) : null;
         return constructor;
      }
   }
}