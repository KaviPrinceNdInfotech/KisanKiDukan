using AutoMapper;
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
    public class ADCategoryController : Controller
    {
        //====Start ADCategory===
        private DbEntities ent = new DbEntities();
        public ActionResult AdCategory()
        {
            var records = ent.ADCategories.AsNoTracking().Where(a => a.ParentCategory == null).ToList();
            return View(records);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(ADCategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = Mapper.Map<ADCategory>(model);
                category.IsActive = true;
                ent.ADCategories.Add(category);
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
            var record = ent.ADCategories.Find(id);
            return View(Mapper.Map<ADCategoryModel>(record));
        }
        [HttpPost]
        public ActionResult Edit(ADCategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = Mapper.Map<ADCategory>(model);
                ent.Entry<ADCategory>(category).State = EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
                return RedirectToAction("AdCategory");
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
                ent.Database.ExecuteSqlCommand("Delete from ADCategory where Id=" + id);
            }
            catch
            {
            }
            return RedirectToAction("AdCategory");
        }
        //=====End ADCategory====
        //=====Start ADSubCategory====
        public ActionResult AdSubCategories()
        {
            var records = ent.ADSubCategories.AsNoTracking().ToList();
            return View(records);
        }
        public ActionResult AddSub()
        {
            var model = new ADCategoryModel();
            model.CategoryList = new SelectList(ent.ADCategories.Where(a => a.ParentCategory == null).ToList(), "Id", "CategoryName");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddSub(ADCategoryModel model)
        {
            try
            {
                model.CategoryList = new SelectList(ent.ADCategories.Where(a => a.ParentCategory != null).ToList(), "Id", "CategoryName");
                if (!ModelState.IsValid)
                {
                    model.CategoryList = new SelectList(ent.ADCategories.Where(a => a.ParentCategory != null).ToList(), "Id", "CategoryName");
                    return View(model);
                }
                var category = Mapper.Map<ADSubCategory>(model);
                category.MainCat_Id = model.Id;
                category.Name = model.CategoryName;
                ent.ADSubCategories.Add(category);
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
            var record = ent.ADSubCategories.Find(id);
            var model = Mapper.Map<ADCategoryModel>(record);
            model.CategoryName = record.Name;
            model.CategoryList = new SelectList(ent.ADCategories.Where(a => a.ParentCategory == null).ToList(), "Id", "CategoryName", model.ParentCategory);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSub(ADCategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = Mapper.Map<ADSubCategory>(model);
                category.MainCat_Id = model.Id;
                category.Name = model.CategoryName;
                ent.Entry<ADSubCategory>(category).State = EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
                return RedirectToAction("SubCategories");
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
                return View(model);
            }
        }
        public ActionResult DeleteSub(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("Delete from ADSubCategory where Id=" + id);
            }
            catch
            {

            }
            return RedirectToAction("AdSubCategories");
        }
    }
}