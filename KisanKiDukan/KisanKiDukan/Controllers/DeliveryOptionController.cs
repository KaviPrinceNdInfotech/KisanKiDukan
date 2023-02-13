using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class DeliveryOptionController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult All()
        {
            var model = new List<DeliveryOption>();
            model = ent.DeliveryOptions.OrderByDescending(a => a.Id).ToList();
            return View(model);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DeliveryOptionDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var state = Mapper.Map<DeliveryOption>(model);
                ent.DeliveryOptions.Add(state);
                ent.SaveChanges();
                TempData["msg"] = "Record has saved.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
            }
            return RedirectToAction("Add");
        }

        public ActionResult Edit(int id)
        {
            var data = ent.DeliveryOptions.Find(id);
            var model = Mapper.Map<DeliveryOptionDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DeliveryOptionDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var state = Mapper.Map<DeliveryOption>(model);
                ent.Entry(state).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Record has updated.";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var data = ent.DeliveryOptions.Find(id);
                ent.DeliveryOptions.Remove(data);
                ent.SaveChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("Server error");
            }
        }
    }
}