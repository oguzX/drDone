using DrDone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.ViewModels
{
    public class IndexModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public AuthLogin auth { get; set; }
        public IList<Product> StaredProduct { get; set; }
    }

}