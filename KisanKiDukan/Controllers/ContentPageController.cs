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
    public class ContentPageController : Controller
    {
        private DbEntities ent = new DbEntities();

        public ActionResult Add(int menuId)
        {
            var record = ent.ContentPages.FirstOrDefault(a => a.Menu_Id == menuId);
            if (record == null)
            {
                var model = new ContentPageModel();
                model.Menu_Id = menuId;
                return View(model);
            }
            return RedirectToAction("Edit", new { id = record.Id });
        }

        [HttpPost]
        public ActionResult Add(ContentPageModel model)
        {
            try
            {
                var data = Mapper.Map<ContentPage>(model);
                ent.ContentPages.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Record has been saved.";
            }
            catch
            {
                TempData["msg"] = "Record could not been saved.";
            }
            return RedirectToAction("Add", new { menuId = model.Menu_Id });
        }

        public ActionResult Edit(int id)
        {
            var record = ent.ContentPages.Find(id);
            var data = Mapper.Map<ContentPageModel>(record);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(ContentPageModel model)
        {
            try
            {
                var data = Mapper.Map<ContentPage>(model);

                ent.Entry<ContentPage>(data).State = EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Record has updated";
            }
            catch
            {
                TempData["msg"] = "Error has occured";
            }
            return RedirectToAction("Edit", new { id = model.Id });
        }
    }
}