using DrDone.Areas.Admin.ViewModels;
using DrDone.Infrastructure;
using DrDone.Models;
using NHibernate.Linq;
using SBlogA.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DrDone.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("product")]
    public class ProductController : Controller
    {
        private const int PostsPerPage = 5;

        public DateTime? Datatime { get; private set; }

        // GET: Admin/Product
        public ActionResult Index(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Product>().Count();
            var currentPostPage = Database.Session.Query<Product>()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

            return View("~/Views/Product/List.cshtml", new ProductIndex {
                IsCheck=true,
                Product = new PagedData<Product>(currentPostPage,totalPostCount,page,PostsPerPage)
            });
        }

        public ActionResult Add()
        {
            return View("~/Views/Product/Form.cshtml", new ProductForm() {
                IsNew = true,
                Categories = Database.Session.Query<Category>().Select(category => new CategoryCheckBox
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsChecked = false
                }).ToList()
            });
        }

        public ActionResult Edit(int Id)
        {
            var product = Database.Session.Load<Product>(Id);

            if (product == null)
                return HttpNotFound();

            return View("~/Views/Product/Form.cshtml", new ProductForm()
            {
                IsNew = false,
                PostId = Id,
                Title = product.Title,
                Content = product.Description,
                Price = product.Price,
                ImageUrl = product.Image,
                Categories = Database.Session.Query<Category>().Select(category => new CategoryCheckBox
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsChecked = product.Category.Contains(category)
                }).ToList()
            });
        }

        [HttpPost,ValidateInput(false)]
        public ActionResult Form(ProductForm form)
        {
            form.IsNew = form.PostId == null;
            if (!ModelState.IsValid && form.Image.FileName == "")
                return View(form);

            Product product;
            if (form.IsNew)
            {
                product = new Product
                {
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.User,
                    Status = 1
                };
            }
            else
            {
                product = Database.Session.Load<Product>(form.PostId);
                if (product == null)
                    return HttpNotFound();

                product.UpdatedAt = DateTime.Now;
            }

            SyncCategory(form.Categories, product.Category);
            product.Image = SaveImage(form.Image, product.Image);
            product.Title = form.Title;
            product.Description = form.Content;
            product.Price = form.Price;
            Database.Session.SaveOrUpdate(product);
            Database.Session.Flush();
            return RedirectToAction("Index");
        }
        public ActionResult Trash(int id)
        {
            var product = Database.Session.Load<Product>(id);
            if (product == null)
                return HttpNotFound();
            product.DeletedAt = DateTime.UtcNow;
            Database.Session.Update(product);
            return RedirectToAction("index");
        }
        public ActionResult Restore(int id)
        {
            var product = Database.Session.Load<Product>(id);
            if (product == null)
                return HttpNotFound();
            product.DeletedAt = null;
            Database.Session.Update(product);
            return RedirectToAction("index");
        }
        public ActionResult Delete(int id)
        {
            var product = Database.Session.Load<Product>(id);
            if (product == null)
                return HttpNotFound();

            Database.Session.Delete(product);
            Database.Session.Flush();
            return RedirectToAction("index");
        }

        public ActionResult Star(int id)
        {
            var product = Database.Session.Load<Product>(id);
            if (product == null)
                return HttpNotFound();
            if(product.Status==1)
                product.Status = 2;
            else
                product.Status = 1;
            product.UpdatedAt = DateTime.Now;
            Database.Session.Update(product);
            return RedirectToAction("index");
        }
        private void SyncCategory(IList<CategoryCheckBox> checkboxes, IList<Category> categories)
        {
            var selectedCat = new List<Category>();
            foreach (var cat in Database.Session.Query<Category>())
            {
                var checkbox = checkboxes.Single(c => c.Id == cat.Id);
                checkbox.Name = cat.Name;
                if (checkbox.IsChecked)
                    selectedCat.Add(cat);
            }
            foreach (var toAdd in selectedCat.Where(t => !categories.Contains(t)))
                categories.Add(toAdd);
            foreach (var toRomeve in categories.Where(t => !selectedCat.Contains(t)).ToList())
                categories.Remove(toRomeve);
        }
        private string SaveImage(HttpPostedFileBase fimage, string image)
        {
            string FileName = Path.GetFileNameWithoutExtension(fimage.FileName);
            string Extension = Path.GetExtension(fimage.FileName);
            string folderPath = "~/ProductImages/";
            FileName = FileName.Replace("-", "").Substring(0, 5) + DateTime.Now.ToString("yymmssfff") + Extension;
            image = folderPath + FileName;
            FileName = Path.Combine(Server.MapPath(folderPath), FileName);

            var ResizedImage = ResizeImage(fimage, 520, 450);
            ResizedImage.Save(FileName);
            return image;
        }
        public static WebImage ResizeImage(HttpPostedFileBase image, int width, int height)
        {
            WebImage img = new WebImage(image.InputStream);
            img.Resize(Math.Min(width, img.Width), Math.Min(height, img.Height));
            return img;
        }

    }
}