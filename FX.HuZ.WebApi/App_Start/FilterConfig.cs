﻿using FX.HuZ.WebApi.Filters;
using System.Web;
using System.Web.Mvc;

namespace FX.HuZ.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
