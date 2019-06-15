using DrDone.Infrastructure;
using DrDone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DrDone.Areas.Admin.ViewModels
{
    public class ProductIndex
    {
        public PagedData<Product> Product { get; set; }
        public bool IsCheck { get; set; }
    }

    public class ProductForm
    {
        public bool IsNew { get; set; }
        public int? PostId { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [Required, DataType(DataType.Currency)]
        public int Price { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Content{ get; set; }

        [Required]
        public IList<CategoryCheckBox> Categories { get; set; }
        [Required]
        public HttpPostedFileBase Image { get; set; }
        public string ImageUrl { get; set; }

    }

    public class CategoryCheckBox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public String Name { get; set; }
    }
}