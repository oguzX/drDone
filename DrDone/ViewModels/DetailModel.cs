using DrDone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.ViewModels
{
    public class DetailModel
    {
        public virtual Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}