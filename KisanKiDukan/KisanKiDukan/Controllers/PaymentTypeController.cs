using KisanKiDukan.Models.Domain;
using KisanKiDukan.Repository.Implementation;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class PaymentTypeController : Controller
    {
        IPaymentTypeRepository PTRepo = new PaymentTypeRepository();
        // GET: PaymentType
        public ActionResult ManagePaymentType()
        {
            return View(PTRepo.GetAllPaymentTypes());
        }

        public ActionResult AddPaymentType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPaymentType(PaymentType PT)
        {
            PTRepo.AddPaymentType(PT);
            return View();
        }

        public ActionResult RemovePaymentType(int Id)
        {
            PTRepo.RemovePaymentType(PTRepo.GetPaymentTypeById(Id));
            return RedirectToAction("ManagePaymentType");
        }
    }
}