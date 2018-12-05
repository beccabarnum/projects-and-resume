using Ninject;
using Ninject.Web.Common.WebHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VendingService.Database;
using VendingService.File;
using VendingService.Helpers;
using VendingService.Interfaces;
using VendingService.Mock;

namespace VendingWeb
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // Bind Database
            kernel.Bind<IVendingService>().To<MockVendingDBService>();
            //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //kernel.Bind<IVendingService>().To<VendingDBService>().WithConstructorArgument("connectionString", connectionString);

            // Bind Log Service
            kernel.Bind<ILogService>().To<LogFileService>();

            return kernel;
        }
    }
}
