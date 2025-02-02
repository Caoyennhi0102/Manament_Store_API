﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Manament_Store_API.Models;

namespace Manament_Store_API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          
        }

        /*
         protected void Application_BeginRequest()
         {
             if (Session["SelectedLanguageId"] != null)
             {
                 int selectedLanguageId = (int)Session["SelectedLanguageId"];

                 HttpContext.Current.Items["SelectedLanguageId"] = selectedLanguageId;

                 var db = new SqlConnectionServer();
                 var translations = db.BanDiches.Where(t => t.IDNgonNgu == selectedLanguageId).ToList();

                 HttpContext.Current.Items["Translations"] = translations;
             }
         }

        */
       

    }
}
