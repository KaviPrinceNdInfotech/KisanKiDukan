using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        DbEntities ent = new DbEntities();
        CommonRepository cr = new CommonRepository();
        public ActionResult MultipleProductStockEntry()
        {
            var model = new ProductSaleModel();
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");
            return View(model);
        }

        [HttpPost]
        public ActionResult MultipleProductStockEntry(ProductSaleModel model)
        {
            
            int? uId = null;
            if (User.IsInRole("admin"))
            {
                  uId = cr.GetDepotId();
            }
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    var ProductId=ent.GETPRODUCTIDBYNAME(model.ProductName);
                    
                    if (model.Products.Count < 1)
                        throw new Exception("You must have products");
                    foreach (var item in model.Products)
                    {
                        var exist = new CentralStore();
                        if (uId != null)
                        {
                            exist = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId= " + item.Product_Id + " , @storeId = " + uId + " , @wheight =" + item.Wheight + " , @metricCode = " + item.Metric_Id + "").FirstOrDefault();
                        }
                        else
                        {
                            exist = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + model.Product_Id + " , @storeId=0 , @wheight =" + item.Wheight + " , @metricCode = " + item.Metric_Id + "").FirstOrDefault();

                        }
                        if (exist != null)
                        {
                            exist.Stock = item.Stock + exist.Stock;
                            ent.SaveChanges();
                        }
                        else
                        {
                            var cs = new CentralStore
                            {
                                Product_Id = item.Product_Id,
                                Stock = item.Stock,
                                Store_Id = uId,
                                Metric_Id = item.Metric_Id,
                                wheight = item.Wheight
                            };
                            ent.CentralStores.Add(cs);
                        }
                        //update product available status
                        if (uId == null)
                        {
                            var product = ent.Products.Where(a => a.Id == item.Product_Id && a.Metric_Id == item.Metric_Id && a.Quantity == item.Wheight).FirstOrDefault();
                            if (product != null)
                            {
                                product.IsStock = true;
                                ent.SaveChanges();
                            }
                            else
                            {
                                var prodAvail = ent.Product_Availability.Where(a => a.Product_Id == item.Product_Id && a.Metrics_Id == item.Metric_Id && a.Quantity == item.Wheight).FirstOrDefault();
                                if (prodAvail != null)
                                {
                                    prodAvail.IsAvailable = true;
                                    ent.SaveChanges();
                                }
                            }
                        }
                    }
                    ent.SaveChanges();
                    trans.Commit();
                    TempData["msg"] = "Record added successfully";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "server error";
                }
            }
            return RedirectToAction("MultipleProductStockEntry");
        }

        public ActionResult SingleProductStockEntry(int productId = 0)
        {
            var model = new ProductSaleModel();
            var data = ent.Products.Find(productId);
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");
            model.ProductName = data.ProductName;
            model.Product_Id = productId;
            return View(model);
        }

        [HttpPost]
        public ActionResult SingleProductStockEntry(ProductSaleModel model)
        {
            int? uId = null;
            if (User.IsInRole("admin"))
            {
                uId = null;
            }
            else
            {
                uId = cr.GetDepotId();
            }
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    if (model.Products.Count < 1)
                        throw new Exception("You must have products");

                    foreach (var item in model.Products)
                    {
                        var exist = new CentralStore();


                        if (uId != null)
                        {
                            exist = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + model.Product_Id + " , @storeId=" + uId + " , @wheight =" + item.Wheight + " , @metricCode = " + item.Metric_Id + "").FirstOrDefault();
                        }
                        else
                        {
                            exist = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + model.Product_Id + " , @storeId=0 , @wheight =" + item.Wheight + " , @metricCode = " + item.Metric_Id + "").FirstOrDefault();
                        }
                        if (exist != null)
                        {
                            exist.Stock = item.Stock + exist.Stock;
                            ent.SaveChanges();
                        }
                        else
                        {
                            var cs = new CentralStore
                            {
                                Product_Id = model.Product_Id,
                                Stock = item.Stock,
                                Store_Id = uId,
                                Metric_Id = item.Metric_Id,
                                wheight = item.Wheight
                            };
                            ent.CentralStores.Add(cs);
                        }

                        //update product available status
                        if (uId == null)
                        {
                            var product = ent.Products.Where(a => a.Id == model.Product_Id && a.Metric_Id == item.Metric_Id && a.Quantity == item.Wheight).FirstOrDefault();
                            if (product != null)
                            {
                                product.IsStock = true;
                                ent.SaveChanges();
                            }
                            else
                            {
                                var prodAvail = ent.Product_Availability.Where(a => a.Product_Id == model.Product_Id && a.Metrics_Id == item.Metric_Id && a.Quantity == item.Wheight).FirstOrDefault();
                                if (prodAvail != null)
                                {
                                    prodAvail.IsAvailable = true;
                                    ent.SaveChanges();
                                }
                            }
                        }
                    }
                    ent.SaveChanges();
                    trans.Commit();
                    TempData["msg"] = "Record added successfully";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "server error";
                }
            }
            return RedirectToAction("SingleProductStockEntry", new { productId = model.Product_Id });
        }
        public ActionResult IndividualStock(int productId = 0)
        {
            var model = new ProductSaleModel();
            var data = ent.Products.Find(productId);
            string qry = "";
            if (User.IsInRole("depot"))
            {
                int uId = cr.GetDepotId();
                qry = @"select p.ProductName,cs.*,m.Metrics as MetricName from centralstore cs
                        join Product p on cs.Product_Id=p.Id
                        join Metric m on cs.Metric_Id=m.MetricCode
                        where cs.Product_Id=" + productId + "and cs.Sstore_Id=" + uId + "";
            }
            else
            {
                qry = @"select p.ProductName,cs.*,m.Metrics as MetricName from centralstore cs
                        join Product p on cs.Product_Id=p.Id
                        join Metric m on cs.Metric_Id=m.MetricCode
                        where cs.Product_Id=" + productId + "";
            }
            var stockData = ent.Database.SqlQuery<ProductData>(qry).ToList();
            model.ProductName = data.ProductName;
            model.Product_Id = productId;
            model.Products = stockData;
            return View(model);
        }

        public ActionResult MultipleStock()
        {
            var model = new ProductSaleModel();
            string qry = "";
            if (User.IsInRole("depot"))
            {
                int uId = cr.GetDepotId();
                qry = @"select p.ProductName,cs.*,m.Metrics as MetricName from centralstore cs
                        join Product p on cs.Product_Id=p.Id
                        join Metric m on cs.Metric_Id=m.MetricCode
                        where cs.Sstore_Id=" + uId + "";
            }
            else
            {
                qry = @"select p.ProductName,cs.*,m.Metrics as MetricName from centralstore cs
                        join Product p on cs.Product_Id=p.Id
                        join Metric m on cs.Metric_Id=m.MetricCode";
            }

            var stockData = ent.Database.SqlQuery<ProductData>(qry).ToList();
            model.Products = stockData;
            return View(model);
        }
    }
}