using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class DCMasterController : Controller
    {
        DbEntities ent = new DbEntities();
        CommonRepository cr = new CommonRepository();
        public ActionResult Add()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Add(DCMasterDTO model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //    try
        //    {
        //        var data = Mapper.Map<DCMasterEntry>(model);
        //        ent.DCMasterEntries.Add(data);
        //        ent.SaveChanges();
        //        TempData["msg"] = "Record added successfully";
        //        return RedirectToAction("Add");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg"] = "Some error";
        //        return View(model);
        //    }

        //}

        //public ActionResult All()
        //{
        //    var data = ent.DCMasterEntries.ToList();
        //    return View(data);
        //}

        //public ActionResult Edit(int id)
        //{
        //    var data = Mapper.Map<DCMasterDTO>(ent.DCMasterEntries.Find(id));
        //    return View(data);
        //}

        //[HttpPost]
        //public ActionResult Edit(DCMasterDTO model)
        //{

        //    try
        //    {
        //        var data = Mapper.Map<DCMasterEntry>(model);
        //        ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
        //        ent.SaveChanges();
        //        TempData["msg"] = "Record updated successfully";
        //        return RedirectToAction("Edit", new { id = model.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg"] = "Some error";
        //    }
        //    return View(model);
        //}

        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        ent.Database.ExecuteSqlCommand(@"delete from DCMasterEntry where Id=" + id);
        //        return Content("ok");
        //    }
        //    catch
        //    {
        //        return Content("Server error");
        //    }

        //}
    }
}