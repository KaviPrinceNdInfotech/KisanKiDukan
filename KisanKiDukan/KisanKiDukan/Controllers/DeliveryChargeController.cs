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
    public class DeliveryChargeController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult Manage()
        {
            var data = ent.DeliveryChargeMasters.FirstOrDefault();
            var model = Mapper.Map<DeliveryChargeMasterDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult Manage(DeliveryChargeMasterDTO model)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var data = Mapper.Map<DeliveryChargeMaster>(model);
                if (model.Id == 0)
                {
                    ent.DeliveryChargeMasters.Add(data);
                }
                else
                {
                    ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
                }
                ent.SaveChanges();
                TempData["msg"] = "Records has saved.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
            }
            return RedirectToAction("Manage");
        }
    }
}