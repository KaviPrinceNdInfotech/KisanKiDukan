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
    public class AdminOprationController : Controller
    {
        DbEntities ent = new DbEntities();
        [AllowAnonymous]
        public ActionResult GetUser(string term)
        {
            var data = ent.Customers.Where(a => a.Phone.Contains(term));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(MembershipDTO model)
        {
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    if (ent.Memberships.Any(a => a.Customer_Id == model.Customer_Id))
                    {
                        TempData["msg"] = "Already Mebership joined";
                        return View(model);
                    }
                    //save user mebership record
                    var data = Mapper.Map<Membership>(model);
                    data.DistributionMonth = model.DistributionMonth - 1;
                    data.CreateDate = DateTime.Now;
                    data.UpdateDate = data.CreateDate;
                    ent.Memberships.Add(data);
                    ent.SaveChanges();
                    //check exixting wallet amount
                    var uWallet = ent.Wallets.Where(a => a.User_Id == model.Customer_Id).FirstOrDefault();
                    if (uWallet != null)
                    {
                        uWallet.Mebr_Amount = uWallet.Mebr_Amount + model.Percentage;
                        ent.SaveChanges();
                    }
                    else
                    {
                        //add user wallet amount
                        var Wlt = new Wallet
                        {
                            User_Id = model.Customer_Id,
                            Mebr_Amount = model.Percentage
                        };
                        ent.Wallets.Add(Wlt);
                        ent.SaveChanges();
                    }
                    trans.Commit();
                    TempData["msg"] = "Data has been saved successfully";
                    return RedirectToAction("Add");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "Server error";
                }
            }
            return View(model);
        }
        
        public ActionResult All()
        {
            var data = (from mbrshp in ent.Memberships
                        join cust in ent.Customers on mbrshp.Customer_Id equals cust.User_Id

                        select new MembershipDTO
                        {
                            Customer_Id=mbrshp.Customer_Id,
                            FullName = cust.FullName,
                            Phone = cust.Phone,
                            Amount = mbrshp.Amount,
                            BenifitAmount = mbrshp.BenifitAmount,
                            Percentage = mbrshp.Percentage,
                            CreateDate = mbrshp.CreateDate,
                            DistributionMonth = mbrshp.DistributionMonth,
                            Remark = mbrshp.Remark
                        }).ToList();

            return View(data);
        }
    }
}