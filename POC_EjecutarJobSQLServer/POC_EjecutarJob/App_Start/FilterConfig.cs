﻿using System.Web;
using System.Web.Mvc;

namespace POC_EjecutarJob
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
