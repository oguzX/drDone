using DrDone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.ViewModels
{
    public class StoreModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products{ get; set; }
        public string Name { get; set; }
    }
}