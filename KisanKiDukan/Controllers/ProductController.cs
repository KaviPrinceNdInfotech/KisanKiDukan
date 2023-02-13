using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Repository.Implementation;
using KisanKiDukan.Repository.Services;
using KisanKiDukan.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Product = KisanKiDukan.Models.Domain.Product;

namespace KisanKiDukan.Controllers
{
    public class ProductController : Controller
    {
        DbEntities ent = new DbEntities();
        #region Product Master entry
        IProductRepository repos = new ProductRepository();
        ProductRepository repo = new ProductRepository();
        public ActionResult Products(int? page, string term)
        {
            var model = new ProductDisplayModelAdmin();
            int vendorID = Convert.ToInt32(Session["AddBy"]);
            if (vendorID > 1)
            {
                var pro = ent.Database.SqlQuery<ProductModel>(@"select prd.*,IsNull(mtr.Metrics,'')Metrics,c.CategoryName,sc.Name as SubCategory,ISNULL(VD.VendorName,'') AS VendorName  from Product prd
        join Category c on prd.Category_Id = c.Id
        left join SubCategory sc on prd.SubId = sc.Id
left outer join Vendor VD ON VD.LoginMaster_id=prd.VendorID
        left join Metric mtr on prd.Metric_Id=mtr.MetricCode where VendorId= " + vendorID + " order by prd.Id desc").ToList();

                if (!string.IsNullOrEmpty(term))
                {
                    term = term.ToLower().Trim();
                    pro = pro.Where(a => a.ProductName.ToLower().StartsWith(term)).ToList();
                }
                int total = pro.Count;

                page = page ?? 1;
                int pageSize = 20;
                decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
                model.NumberOfPages = (int)noOfPages;
                model.Page = page;
                pro = pro.OrderByDescending(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
                model.Products = pro;
            }
            else
            {
                var pro = ent.Database.SqlQuery<ProductModel>(@"select prd.*,IsNull(mtr.Metrics,'')Metrics,c.CategoryName,sc.Name as SubCategory,ISNULL(VD.VendorName,'') AS VendorName  from Product prd
        join Category c on prd.Category_Id = c.Id
        left join SubCategory sc on prd.SubId = sc.Id
left outer join Vendor VD ON VD.LoginMaster_id=prd.VendorID
        left join Metric mtr on prd.Metric_Id=mtr.MetricCode order by prd.Id desc").ToList();

                if (!string.IsNullOrEmpty(term))
                {
                    term = term.ToLower().Trim();
                    pro = pro.Where(a => a.ProductName.ToLower().StartsWith(term)).ToList();
                }
                int total = pro.Count;

                page = page ?? 1;
                int pageSize = 20;
                decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
                model.NumberOfPages = (int)noOfPages;
                model.Page = page;
                pro = pro.OrderByDescending(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
                model.Products = pro;
            }
            return View(model);
        }

        public ActionResult PStatus(int id)
        {
            ent.Database.ExecuteSqlCommand(@"update Product set IsStock=case when IsStock=1 then 0 else 1 end where Id= " + id);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public ActionResult GetSubCategory(int subId)
        {
            var data = ent.SubCategories.Where(a => a.MainCat_Id == subId).ToList();
            var c = Mapper.Map<IEnumerable<CategoryModel>>(data);
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            var model = new ProductModel();
            model.PAvail = new List<PAvailDTO> { new PAvailDTO { Id = 0, Product_Id = 0, Quantity = 0, Metrics_Id = 0, Price = 0, OurPrice = 0 } };
            model.CategoryList = new SelectList(ent.Categories.ToList(), "Id", "CategoryName");
            model.SubCategory = new SelectList(ent.SubCategories.ToList(), "Id", "Name");
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(ProductModel model, IEnumerable<HttpPostedFileBase> images)
        {
            Product product = new Product();
            model.CategoryList = new SelectList(ent.Categories.ToList(), "Id", "CategoryName");
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");
            model.SubCategory = new SelectList(ent.SubCategories.ToList(), "Id", "Name");
            var VID = Convert.ToInt32(Session["AddBy"]);
            var vdData = ent.Vendors.Find(VID);
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    if (model.ImageFile != null)
                    {
                        if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                        {
                            TempData["msg"] = "Image should not succeed 2 mb";
                            return View(model);
                        }
                        var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                        if (uploadResult == "not allowed")
                        {
                            TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                            return View(model);
                        }
                        model.ProductImage = uploadResult;
                    }
                    product.ProductName = model.ProductName;
                    product.Category_Id = model.Category_Id;
                    product.ProductImage = model.ProductImage;
                    product.Price = model.Price;
                    product.OurPrice = model.Price - ((model.Price*model.DiscountPrice)/100);
                    product.DiscountPrice = model.DiscountPrice;
                    product.ProductDescription = model.ProductDescription;
                    product.Metric_Id = model.Metric_Id;
                    product.IsStock = model.IsStock;
                    model.IsStocks = product.IsStock == true ?   "In-Stock" : "Out Of-Stock";
                    product.IsReplacement = model.IsReplacement;
                    product.Quantity = model.Quantity;
                    product.IsFeatured = model.IsFeatured;
                    product.IsFreeShipping = model.IsFreeShipping;
                    product.ShippingCharge = model.ShippingCharge;
                    product.IsReviewsAllow = model.IsReviewsAllow;
                    product.IsActive = model.IsActive;
                    product.IsVariant = model.IsVariant;
                    product.IsRecomend = model.IsRecomend;
                    product.SubId = model.subId;
                    product.IsHotdeals = model.IsHotdeals;
                    product.IsFeatureProduct = model.IsFeatureProduct;
                    product.IsSpecial = model.IsSpecial;
                    product.VendorId = VID;
                    if (product.Category_Id != null && product.Price != null && product.SubId != null && (product.VendorId != null || product.VendorId != 0))
                    {
                        ent.Products.Add(product);
                        ent.SaveChanges();
                        foreach (var val in model.PAvail)
                        {
                            if (val.Quantity > 0 && val.Price > 0)
                            {
                                var pAvail = new Product_Availability
                                {
                                    Product_Id = product.Id,
                                    Quantity = val.Quantity,
                                    Price = val.Price,
                                    Metrics_Id = val.Metrics_Id,
                                    IsAvailable = true,
                                    OurPrice = val.OurPrice
                                };
                                ent.Product_Availability.Add(pAvail);
                                ent.SaveChanges();
                            }
                        }
                        HttpPostedFileBase fileBase =Request.Files["images"];
                        if (fileBase.ContentLength != 0)
                        {
                            List<Product_Image> productImages = new List<Product_Image>();
                            if (images != null && images.Count() <= 5)
                            {
                                foreach (var image in images)
                                {
                                    if (image.ContentLength > 0)
                                    {
                                        var path = FileOperation.UploadImage(image, "Images");
                                        var imgList = new Product_Image()
                                        {
                                            Product_Id = product.Id,
                                            ImageName = path
                                        };
                                        productImages.Add(imgList);
                                    }
                                }
                                product.ProductMultiImages = productImages;
                                ent.Products.Add(product);
                            }
                            else
                            {
                                TempData["msg"] = "Please select five images.";
                                return RedirectToAction("Add");
                            }
                        }
                        trans.Commit();
                        TempData["msg"] = "Records has added successfully.";
                    }
                    else
                    {
                        TempData["msg"] = "Server error Please login again.";
                    }
                    return RedirectToAction("Add");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "Server error";
                    return View(model);
                }
            }
        }

        public ActionResult Edit(int id)
        {
            var record = ent.Products.Find(id);
            var model = Mapper.Map<ProductModel>(record);
            model.CategoryList = new SelectList(ent.Categories.ToList(), "Id", "CategoryName", model.Category_Id);
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics", model.Metric_Id);
            model.SubCategory = new SelectList(ent.SubCategories.ToList(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductModel model)
        {
            try
            {
                model.CategoryList = new SelectList(ent.Categories.ToList(), "Id", "CategoryName", model.Category_Id);
                model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics", model.Metric_Id);
                model.SubCategory = new SelectList(ent.SubCategories.ToList(), "Id", "Name");
                var product = Mapper.Map<Product>(model);
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }
                    product.ProductImage = uploadResult;
                }
                product.VendorId = Convert.ToInt32(Session["AddBy"]);

                if (product.Category_Id != null && product.Price != null && product.SubId != null && product.VendorId != null)
                {
                    ent.Entry<Product>(product).State = EntityState.Modified;
                    ent.SaveChanges();
                    TempData["msg"] = "Records has updated successfully.";
                }
                else
                {
                    TempData["msg"] = "Server error Please login again.";
                }
                return RedirectToAction("Edit", new { id = model.Id });
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var data = ent.Products.Find(id);
                ent.Products.Remove(data);
                ent.SaveChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("Server error");
            }
        }

        public ActionResult Variant(int id)
        {
            var model = new VarientVM();
            model.ProductName = ent.Products.Find(id).ProductName;
            model.Product_Id = id;
            model.VarientList = ent.Database.SqlQuery<PAvailDTO>(@"select prd.*,mtr.Metrics from Product_Availability prd
             join Product p on prd.Product_Id=p.Id
             join Metric mtr on prd.Metrics_Id=mtr.MetricCode 
             where prd.Product_Id=" + id + "").ToList();
            return View(model);
        }

        public ActionResult EditVarient(int id)
        {
            var record = ent.Product_Availability.Find(id);
            var model = Mapper.Map<PAvailDTO>(record);
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics", model.Metrics_Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVarient(PAvailDTO model)
        {
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics", model.Metrics_Id);
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = Mapper.Map<Product_Availability>(model);
                ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Record has been updated.";
                return RedirectToAction("EditVarient", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
                return View(model);
            }
        }

        public ActionResult DeleteVarient(int id)
        {
            try
            {
                var data = ent.Product_Availability.Find(id);
                var product = ent.Products.Find(data.Product_Id);
                ent.Product_Availability.Remove(data);
                ent.SaveChanges();
                if (!ent.Product_Availability.Any(a => a.Product_Id == product.Id))
                {
                    product.IsVariant = false;
                    ent.SaveChanges();
                }
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("Server error");
            }
        }
        public ActionResult AddMoreVarint(int productId)
        {
            var model = new ProductModel();
            model.Id = productId;
            model.PAvail = new List<PAvailDTO> { new PAvailDTO { Id = 0, Product_Id = productId, Quantity = 0, Metrics_Id = 0, Price = 0, OurPrice = 0 } };
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");

            return View(model);
        }

        [HttpPost]
        public ActionResult AddMoreVarint(ProductModel model)
        {
            model.MetricList = new SelectList(ent.Metrics.ToList(), "MetricCode", "Metrics");
            try
            {
                //save product multiple varients
                foreach (var val in model.PAvail)
                {
                    if (val.Quantity > 0 && val.Price > 0)
                    {
                        var pAvail = new Product_Availability
                        {
                            Product_Id = model.Id,
                            Quantity = val.Quantity,
                            Price = val.Price,
                            Metrics_Id = val.Metrics_Id,
                            IsAvailable = val.IsAvailable,
                            OurPrice = val.OurPrice
                        };
                        ent.Product_Availability.Add(pAvail);
                        ent.SaveChanges();
                    }
                }
                var product = ent.Products.Find(model.Id);
                if (product.IsVariant == false)
                {
                    product.IsVariant = true;
                    ent.SaveChanges();
                }
                TempData["msg"] = "Records has added successfully.";
                return RedirectToAction("AddMoreVarint", new { productId = model.Id });
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
                return View(model);
            }
        }

#endregion

        #region Metric master entry

        public ActionResult Metrics(int? page)
        {
            var model = new MetricsShowAdminModel();
            var data = ent.Metrics.ToList();
            int total = data.Count;
            page = page ?? 1;
            int pageSize = 20;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            data = data.OrderBy(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Metric = data;
            return View(model);
        }

        public ActionResult AddMetric()
        {
            var model = new AddMetricsModel();
            var metricCode = ent.Metrics.OrderByDescending(a => a.Id).Where(a => a.MetricCode != null).FirstOrDefault().MetricCode ?? 1;
            model.MetricCode = metricCode + 1;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMetric(AddMetricsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var data = Mapper.Map<Metric>(model);
                ent.Metrics.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Metrics has added successfully.";

            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("AddMetric");
        }

        public ActionResult DelMetric(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("delete from Metric where Id=" + id);
            }
            catch
            {

            }
            return RedirectToAction("Metrics");
        }
        #endregion

    }
}