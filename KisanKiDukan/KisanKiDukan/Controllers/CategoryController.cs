using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class CategoryController : Controller
    {
        private DbEntities ent = new DbEntities();
        public ActionResult Categories()
        {
            var records = ent.Categories.AsNoTracking().Where(a => a.ParentCategory == null).ToList();
            return View(records);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = Mapper.Map<Category>(model);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View();
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View();
                    }
                    category.CategoryImage = uploadResult;
                }
                ent.Categories.Add(category);
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("Add");
        }
        public ActionResult Edit(int id)
        {
            var record = ent.Categories.Find(id);
            return View(Mapper.Map<CategoryModel>(record));
        }

        [HttpPost]
        public ActionResult Edit(CategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = Mapper.Map<Category>(model);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View();
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View();
                    }
                    category.CategoryImage = uploadResult;
                }
                ent.Entry<Category>(category).State = EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
                return RedirectToAction("Categories");
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("Delete from category where Id=" + id);
            }
            catch{ }
            return RedirectToAction("Categories");
        }
        public ActionResult SubCategories()
        {
            var records = ent.SubCategories.AsNoTracking().ToList();
            return View(records);
        }
        public ActionResult AddSub()
        {
            var model = new CategoryModel();
            model.CategoryList = new SelectList(ent.Categories.Where(a => a.ParentCategory == null).ToList(), "Id", "CategoryName");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSub(CategoryModel model)
        {
            try
            {
                model.CategoryList = new SelectList(ent.Categories.Where(a => a.ParentCategory != null).ToList(), "Id", "CategoryName");
                if (!ModelState.IsValid)
                {
                    model.CategoryList = new SelectList(ent.Categories.Where(a => a.ParentCategory != null).ToList(), "Id", "CategoryName");
                    return View(model);
                }
                var category = Mapper.Map<SubCategory>(model);
                category.MainCat_Id = model.Id;
                category.UpToText = model.UpToText;
                category.Name = model.CategoryName;
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View();
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View();
                    }
                    category.CatImage = uploadResult;
                }
                ent.SubCategories.Add(category);
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("AddSub");
        }

        public ActionResult EditSub(int id)
        {
            TempData["msg"] = "";
            var record = ent.SubCategories.Find(id);
            var model = Mapper.Map<CategoryModel>(record);
            model.CategoryName = record.Name;
            model.CategoryImage = record.CatImage;
            model.MainCat_Id = record.MainCat_Id;
            model.CategoryList = new SelectList(ent.Categories.Where(a => a.ParentCategory == null).ToList(), "Id", "CategoryName", model.ParentCategory);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSub(CategoryModel model)
        {
            model.CategoryList = new SelectList(ent.Categories.Where(a => a.ParentCategory != null).ToList(), "Id", "CategoryName");
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.SubCategories.Find(model.Id);
                    var category = Mapper.Map<SubCategory>(model);
                    data.CatImage = model.CategoryImage;
                    data.Name = model.CategoryName;
                    data.MainCat_Id = model.MainCat_Id;
                    if (model.ImageFile != null)
                    {
                        if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                        {
                            TempData["msg"] = "Image should not succeed 2 mb";
                            return View();
                        }
                        var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                        if (uploadResult == "not allowed")
                        {
                            TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                            return View();
                        }
                        data.CatImage = uploadResult;
                    }
                    ent.SaveChanges();
                    tran.Commit();
                    TempData["msg"] = "Records has added successfully.";

                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error has been occured.";
                    tran.Rollback();
                    return View(model);
                }
                return RedirectToAction("SubCategories");
            }
        }

        public ActionResult DeleteSub(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("Delete from subcategory where Id=" + id);
            }
            catch
            {

            }
            return RedirectToAction("SubCategories");
        }
    }
}