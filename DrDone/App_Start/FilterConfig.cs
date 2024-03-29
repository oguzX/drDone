﻿using DrDone.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace DrDone
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TransactionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
