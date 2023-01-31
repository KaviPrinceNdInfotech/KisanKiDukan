using KisanKiDukan.Models.APIModels.RequstModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.NewRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class KartController : Controller
    {
        KartOperation kartOp = new KartOperation();
        DbEntities ent = new DbEntities();

        public ActionResult AddToCart(int productId, int metricId, int weight)
        {
            // if user is logged in
            var CR = new CartReturnDTO { NOI = 0, Total = 0 };
            if (UserCookie.IsInCookie)
            {
                var vendorid = ent.Products.Where(a =>  a.Id == productId).Select(a=>a.VendorId).FirstOrDefault();
                int clientId = UserCookie.ClientId;
                var kart = ent.Karts.FirstOrDefault(a => a.Client_Id == clientId);
                if (kart == null)
                {
                    kart = new Kart
                    {
                        Client_Id = clientId
                    };
                    ent.Karts.Add(kart);
                    ent.SaveChanges();

                    var newItem = new KartDetail
                    {
                        Kart_Id = kart.Id,
                        Metric_Id = metricId,
                        Quantity = 1,
                        Volume = weight,
                        Product_Id = productId,
                        VendorId = vendorid
                    };
                    ent.KartDetails.Add(newItem);
                }
                else
                {
                    if (ent.KartDetails.Any(a => a.Kart_Id == kart.Id))
                    {
                        var item = ent.KartDetails.FirstOrDefault(a => a.Kart_Id == kart.Id && a.Product_Id == productId && a.Volume == weight && a.Metric_Id == metricId);
                        if (item != null)
                        {
                            item.Quantity++;
                        }
                        else
                        {

                            var newItem = new KartDetail
                            {
                                Kart_Id = kart.Id,
                                Metric_Id = metricId,
                                Quantity = 1,
                                Volume = weight,
                                Product_Id = productId,
                                VendorId = vendorid
                            };
                            ent.KartDetails.Add(newItem);
                        }
                    }
                    else
                    {
                        var newItem = new KartDetail
                        {
                            Kart_Id = kart.Id,
                            Metric_Id = metricId,
                            Quantity = 1,
                            Volume = weight,
                            Product_Id = productId,
                            VendorId = vendorid
                        };
                        ent.KartDetails.Add(newItem);
                    }
                }
                //check product stock available or not
                var csData = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + productId + " , @storeId=0 , @wheight =" + weight + " , @metricCode = " + metricId + "").FirstOrDefault();
                if (csData != null && csData.Stock > 0)
                {
                    //double? lesQnt = csData.Stock - item.Quantity;
                }
                ent.SaveChanges();
                CR = kartOp.GetCartCounts(clientId);
            }
            return Json(CR, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadCart()
        {
            var rm = new CartReturnDTO { NOI = 0, Total = 0 };
            // registered user
            if (UserCookie.IsInCookie)
                rm = kartOp.GetCartCounts(UserCookie.ClientId);
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayKart()
        {
            var kartDetail = new AllKartDetailModel();
            if (Request.Cookies["user"] != null)
            {
                var userName = Request.Cookies["user"].Value;
                var user = ent.Customers.FirstOrDefault(a => a.Email_Id == userName || a.Password == userName);
                int ClientId = 0;
                if (user != null)
                    ClientId = user.User_Id;
                kartDetail = kartOp.GetCart(ClientId);
            }
            return View(kartDetail);
        }

        #region Kart Functions

        public ActionResult plusOne(int kartDetailId)
        {
            if (UserCookie.IsInCookie)
            {
                // KDRepo.PlusOne(Id);
                var kartDetail = ent.KartDetails.Find(kartDetailId);
                if (kartDetail != null)
                {
                    try
                    {
                        kartDetail.Quantity++;
                        ent.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return RedirectToAction("DisplayKart");
        }

        public ActionResult MinusOne(int kartDetailId)
        {
            if (UserCookie.IsInCookie)
            {
                var kartDetail = ent.KartDetails.Find(kartDetailId);
                if (kartDetail != null)
                {
                    try
                    {
                        var kart = ent.Karts.Find(kartDetail.Id);
                        if (kartDetail.Quantity > 1)
                            kartDetail.Quantity--;
                        else
                            ent.KartDetails.Remove(kartDetail);
                        ent.SaveChanges();
                        if (!ent.KartDetails.Any(a => a.Kart_Id == kart.Id))
                        {
                            ent.Karts.Remove(kart);
                            ent.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return RedirectToAction("DisplayKart");
        }
        public ActionResult RemoveProduct(int kartDetailId)
        {
            if (UserCookie.IsInCookie)
            {
                try
                {
                    var kartDetail = ent.KartDetails.Find(kartDetailId);
                    ent.KartDetails.Remove(kartDetail);
                    ent.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("DisplayKart");
        }
        #endregion
    }
}