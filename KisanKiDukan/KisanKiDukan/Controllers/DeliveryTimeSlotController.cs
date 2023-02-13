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
    public class DeliveryTimeSlotController : Controller
    {
        DbEntities ent = new DbEntities();
        // GET: PincodeMaster
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DeliveryTimeSlotDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (ent.DeliveryTimeSlots.Any(a => a.SlotCode == model.SlotCode))
                {
                    TempData["msg"] = "SlotCode must be unque.";
                    return View(model);
                }
                var data = Mapper.Map<DeliveryTimeSlot>(model);

                ent.DeliveryTimeSlots.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Added successfully";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server Error";
            }
            return RedirectToAction("Add");
        }

        public ActionResult All()
        {
            string query = @"
select Id,SlotCode,
Convert(nvarchar(15),StartTime,100) +' - '+ Convert(nvarchar(15),EndTime,100) as TimeSlot
from DeliveryTimeSlot";
            var data = ent.Database.SqlQuery<DeliveryTimeSlotModel>(query).ToList();
            return View(data);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ent.DeliveryTimeSlots.Remove(ent.DeliveryTimeSlots.Find(id));
                ent.SaveChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }
    }
}