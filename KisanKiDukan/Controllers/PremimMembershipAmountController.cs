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
    public class PremimMembershipAmountController : Controller
    {
        DbEntities ent = new DbEntities();
        //GET: PremimMembershipAmount
        public ActionResult Amount()
        {
            var data = ent.premiumMembershipAmounts.FirstOrDefault();
            var model = Mapper.Map<PremiumAmount>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult Amount(PremiumAmount model)
        {
            try
            {
                if (model.Id < 1)
                    ModelState.Remove("Id");
                if (!ModelState.IsValid)
                    return View(model);
                var data = Mapper.Map<premiumMembershipAmount>(model);
                if (data.Id > 0)
                    ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
                else
                    ent.premiumMembershipAmounts.Add(data);
                ent.SaveChanges();
                ViewBag.msg = "Saved Successfully";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                var messgae = ex.Message;
                ViewBag.msg = "Server error";
            }
            return View(model);
        }
    }
}