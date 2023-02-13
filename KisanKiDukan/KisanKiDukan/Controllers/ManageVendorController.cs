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
    public class ManageVendorController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VendorDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var data = Mapper.Map<Vendor>(model);
                ent.Vendors.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Data has saved successfully";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
            }
            return RedirectToAction("Add");
        }
    }
}