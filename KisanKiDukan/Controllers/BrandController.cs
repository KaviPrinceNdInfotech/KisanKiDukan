using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class BrandController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(BrandDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.Logo != null)
                {
                    var logo = FileOperation.UploadImage(model.Logo, "BrandLogo");
                    if (logo.Equals("not allowed"))
                    {
                        TempData["err"] = "Only .jpeg, .jpg, .png image will be accept";
                        return View(model);
                    }
                    model.BrandLogo = logo;
                }
                var data = Mapper.Map<ProductBrand>(model);
                data.IsActive = true;
                ent.ProductBrands.Add(data);
                ent.SaveChanges();
                TempData["scs"] = "Record has been saved.";
                return RedirectToAction("Add");
            }
            catch (Exception ex)
            {
                TempData["err"] = "Record could not been saved.";
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = Mapper.Map<BrandDTO>(ent.ProductBrands.Find(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(BrandDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (model.Logo != null)
                {
                    var logo = FileOperation.UploadImage(model.Logo, "BrandLogo");
                    if (logo.Equals("not allowed"))
                    {
                        TempData["err"] = "Only .jpeg, .jpg, .png image will be accept";
                        return View(model);
                    }
                    model.BrandLogo = logo;
                }
                var data = Mapper.Map<ProductBrand>(model);
                ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                TempData["scs"] = "Record has been updated.";
                return RedirectToAction("Edit", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["err"] = "Record could not been saved.";
                return View(model);
            }
        }
        public ActionResult ShowRecords()
        {
            var records = ent.ProductBrands.ToList();
            return View(records);
        }

        public ActionResult ChangeStatus(int id)
        {
            try
            {
                string q = @"update ProductBrand set IsActive = case when IsActive=1 then 0 else 1 end where Id=" + id;
                ent.Database.ExecuteSqlCommand(q);
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("Server error");
            }

        }

        public ActionResult ChangeFeature(int id)
        {
            try
            {
                string q = @"update ProductBrand set IsFeature = case when IsFeature=1 then 0 else 1 end where Id=" + id;
                ent.Database.ExecuteSqlCommand(q);
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("Server error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("delete from ProductBrand where id=" + id);
                return Content("ok");

            }
            catch (Exception ex)
            {
                return Content("Server error");

            }

        }
    }
}