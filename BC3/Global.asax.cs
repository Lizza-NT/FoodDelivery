using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BC3.Services;

namespace BC3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Neo4jService Neo4jService { get; private set; }
        public static CuaHangService CuaHangService { get; private set; }
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //NinjectWebCommon.Start();
        }
        protected void Application_End()
        {
            // Đảm bảo kết nối được đóng khi ứng dụng kết thúc
            //Neo4jService?.Dispose();
            //NinjectWebCommon.Stop();
        }
    }
}
