using DrDone.Areas.Admin.ViewModels;
using DrDone.Infrastructure;
using DrDone.Models;
using DrDone.ViewModels;
using NHibernate.Linq;
using SBlogA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DrDone.Controllers
{
    public class ProductController : Controller
    {
        private const int PostsPerPage = 5;

        // GET: Product
        public ActionResult Index()
        {
            return RedirectToRoute(new { controller = "Product", action = "List" });
        }
        public ActionResult Detail(int id = -1)
        {
            var product = Database.Session.Load<Product>(id);
            return View("Detail", new DetailModel
            {
                Categories = Database.Session.Query<Category>().ToList(),
                Product = product
            });
        }
        [Authorize(Roles  = "admin, moderator, user")]
        public ActionResult Add()
        {
            return View("Form", new Areas.Admin.ViewModels.ProductForm()
            {
                IsNew = true,
                Categories = Database.Session.Query<Category>().Select(category => new CategoryCheckBox
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsChecked = false
                }).ToList()
            });
        }
        [Authorize(Roles = "admin, moderator, user")]
        [HttpPost, ValidateInput(false)]
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
                    User = Auth.User
                };
                if (User.IsInRole("moderator") || User.IsInRole("admin"))
                    product.Status = 1;
                else
                    product.Status = 0;
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
        [Authorize(Roles="user,admin,moderator")]
        public ActionResult List(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Product>().Count();
            var currentPostPage = Database.Session.Query<Product>()
                .Where(p => p.User.Id == Auth.User.Id)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

            return View(new ProductIndex
            {
                IsCheck = false,
                Product = new PagedData<Product>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Check(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Product>().Where(p => p.Status == 0).Count();
            var currentPostPage = Database.Session.Query<Product>()
                .Where(p => p.Status == 0)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

            return View("List",new ProductIndex
            {
                IsCheck = true,
                Product = new PagedData<Product>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Publish(int id = 1)
        {
            var product = Database.Session.Load<Product>(id);
            product.Status = 1;
            Database.Session.Update(product);
            Database.Session.Flush();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int Id)
        {
            var product = Database.Session.Load<Product>(Id);

            if (product == null)
                return HttpNotFound();

            return View("Form", new ProductForm()
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
        private string SaveImage(HttpPostedFileBase fimage, string image)
        {
            string FileName = Path.GetFileNameWithoutExtension(fimage.FileName);
            string Extension = Path.GetExtension(fimage.FileName);
            string folderPath = "~/ProductImages/";
            FileName = FileName.Replace("-", "").Substring(0, 5) + DateTime.Now.ToString("yymmssfff") + Extension;
            image = folderPath + FileName;
            FileName = Path.Combine(Server.MapPath(folderPath), FileName);

            var ResizedImage = ResizeImage(fimage,520,450);
            ResizedImage.Save(FileName);
            return image;
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
        public static WebImage ResizeImage(HttpPostedFileBase image, int width, int height)
        {
            WebImage img = new WebImage(image.InputStream);
            img.Resize(Math.Min(width, img.Width),Math.Min(height,img.Height));
            return img;
        }
        [HttpPost]
        [Authorize(Roles="admin")]
        public ActionResult addCategory(string _categoryname)
        {
            _categoryname = _categoryname.Trim();
            var categoryQuery = Database.Session.Query<Category>().FirstOrDefault(c => c.Name == _categoryname);
            var result = 1;
            var id = -1;
            string message = "<b>"+_categoryname+" </b>Başarılı bir şekilde eklendi";
            if(categoryQuery != null)
            {
                result = 0;
                message = "Bu kategori zaten var.";
            }
            else if(_categoryname == "")
            {
                result = 0;
                message = "Lütfen bir isim girin.";
            }
            else
            {
                Category category = new Category{ Name = _categoryname };
                Database.Session.Save(category);
                Database.Session.Flush();
                id = category.Id;
            }
            return Json(new { result = result, message = message, id=id });
        }
    }
}