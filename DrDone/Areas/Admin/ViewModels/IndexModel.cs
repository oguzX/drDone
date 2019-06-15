using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.Areas.Admin.ViewModels
{
    public class IndexModel
    {
        public virtual int UserCount { get; set; }
        public virtual int ProductCount { get; set; }
        public virtual int CategoryCount { get; set; }
    }
}