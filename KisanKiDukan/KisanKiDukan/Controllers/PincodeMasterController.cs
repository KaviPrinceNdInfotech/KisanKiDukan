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
    public class PincodeMasterController : Controller
    {
        DbEntities ent = new DbEntities();
        // GET: PincodeMaster
        public ActionResult Add()
        {
            var model = new DeliveryPincodeDTO();
            var pincode = ent.DeliveryPincodes.ToList();
            model.PincodeList = pincode;
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(DeliveryPincodeDTO model)
        {
            var pincode = ent.DeliveryPincodes.ToList();
            try
            {
                if (!ModelState.IsValid)
                {
                    model.PincodeList = pincode;
                    return View(model);
                }
                var data = Mapper.Map<DeliveryPincode>(model);
                ent.DeliveryPincodes.Add(data);
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
            var data = ent.DeliveryPincodes.ToList();
            return View(data);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ent.DeliveryPincodes.Remove(ent.DeliveryPincodes.Find(id));
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