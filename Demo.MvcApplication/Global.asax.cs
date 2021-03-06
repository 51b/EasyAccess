﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CK1.EasyFramework.Infrastructure.Util.CustomTimestamp;
using Demo.Service.Bootstrap.AutoMapper;
using Demo.Service.Bootstrap;
using EasyAccess.Infrastructure;
using EasyAccess.Service.Bootstrap.AutoMapper;
using EasyAccess.Service.Bootstrap;
using Spring.Web.Mvc;

namespace Demo.MvcApplication
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : SpringMvcApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EasyAccessServiceInitializer.DatabaseInitialize();
            DemoServiceInitializer.DatabaseInitialize();

            InfrastructureConfig.CustomTimestampUpdateOption = CustomTimestampUpdateOption.Disable;

            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<EasyAccessProfile>();
                    cfg.AddProfile<DemoProfile>();
                }
           );
        }
    }
}