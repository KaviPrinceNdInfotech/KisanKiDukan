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
    public class OrderStatusController : Controller
    {
        IOrderStatusRepository OSRepo = new OrderStatusRepository();
        // GET: OrderStatus
        public ActionResult ManageOrderStatus()
        {
            return View(OSRepo.GetAllOrderStatus());
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(OrderStatu OS)
        {
            OSRepo.AddStatus(OS);
            return View();
        }

        public ActionResult RemoveOS(int Id)
        {
            OSRepo.RemoveOrderStatus(OSRepo.GetOrderStatusByID(Id));
            return RedirectToAction("ManageOrderStatus");
        }
    }
}