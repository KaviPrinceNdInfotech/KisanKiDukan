using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.NewRepository;
using KisanKiDukan.Repository.Implementation;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class HomeController : Controller
    {
        DbEntities ent = new DbEntities();
        IKartRepository KRepo = new KartRepository();
        IKartDetailsRepository KDRepo = new KartDetailsRepository();
        IProductRepository PRepo = new ProductRepository();
        public ActionResult Index()
        {
            var model = new HomeModel();
            //new arrival products
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                PremiumAmount = p.PremiumAmount,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks=p.IsStock==true?"yes":"No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                           ).OrderByDescending(a => a.Id).ToList();
            foreach (var prod in products)
            {
                if (prod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == prod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = prod.Metric_Id ?? 0,
                        Product_Id = prod.Id,
                        Weight = prod.Quantity ?? 0,
                        Price = prod.OurPrice ?? prod.Price ?? 0,
                        IsStock = prod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == prod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    prod.Variants = variants;
                }
            }
            model.Products = products;

            // recommendation products
            var rmdProducts = (from p in ent.Products
                               join c in ent.Categories on p.Category_Id equals c.Id
                               join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                               where p.IsRecomend == true
                               select new ProductModel
                               {
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   CategoryName = c.CategoryName,
                                   Metrics = m.Metrics,
                                   Id = p.Id,
                                   Price = p.Price,
                                   OurPrice = p.OurPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks = p.IsStock == true ? "yes" : "No",
                                   Quantity = p.Quantity,
                                   IsVariant = p.IsVariant,
                                   Metric_Id = p.Metric_Id,
                                   PremiumAmount = p.PremiumAmount,
                               }).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in rmdProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }

            //Hot deals


            var HotProducts = (from p in ent.Products
                               join c in ent.Categories on p.Category_Id equals c.Id
                               join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                               where p.IsHotdeals == true
                               select new ProductModel
                               {
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   CategoryName = c.CategoryName,
                                   Metrics = m.Metrics,
                                   Id = p.Id,
                                   Price = p.Price,
                                   OurPrice = p.OurPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks = p.IsStock == true ? "yes" : "No",
                                   Quantity = p.Quantity,
                                   IsVariant = p.IsVariant,
                                   Metric_Id = p.Metric_Id,
                                   PremiumAmount = p.PremiumAmount,
                               }
                               ).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in HotProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }


            //Feature product
            var FeatureProducts = (from p in ent.Products
                                   join c in ent.Categories on p.Category_Id equals c.Id
                                   join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                                   where p.IsFeatureProduct == true
                                   select new ProductModel
                                   {
                                       ProductName = p.ProductName,
                                       ProductImage = p.ProductImage,
                                       CategoryName = c.CategoryName,
                                       Metrics = m.Metrics,
                                       Id = p.Id,
                                       Price = p.Price,
                                       OurPrice = p.OurPrice,
                                       ProductDescription = p.ProductDescription,
                                       IsStock = p.IsStock,
                                       IsStocks = p.IsStock == true ? "yes" : "No",
                                       Quantity = p.Quantity,
                                       IsVariant = p.IsVariant,
                                       Metric_Id = p.Metric_Id,
                                       PremiumAmount = p.PremiumAmount,
                                   }
                               ).ToList();
            foreach (var rprod in FeatureProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }

            //Speacial
            var SpeacialProducts = (from p in ent.Products
                                    join c in ent.Categories on p.Category_Id equals c.Id
                                    join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                                    where p.IsSpecial == true
                                    select new ProductModel
                                    {
                                        ProductName = p.ProductName,
                                        ProductImage = p.ProductImage,
                                        CategoryName = c.CategoryName,
                                        Metrics = m.Metrics,
                                        Id = p.Id,
                                        Price = p.Price,
                                        OurPrice = p.OurPrice,
                                        ProductDescription = p.ProductDescription,
                                        IsStock = p.IsStock,
                                        IsStocks = p.IsStock == true ? "yes" : "No",
                                        Quantity = p.Quantity,
                                        IsVariant = p.IsVariant,
                                        Metric_Id = p.Metric_Id,
                                        PremiumAmount = p.PremiumAmount,
                                    }
                             ).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in SpeacialProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }

            model.specialProducts = SpeacialProducts;
            var speciabanners = ent.BannerImages.Where(a => a.IsActive).ToList();
            model.Banners = speciabanners;
            //feature product
            model.FeatureProducts = FeatureProducts;
            var record = FeatureProducts.OrderByDescending(a => a.Id).ToList();
            model.FeatureProducts1 = record;
            var Featurebanners = ent.BannerImages.Where(a => a.IsActive).ToList();
            model.Banners = Featurebanners;
            //new product
            model.Products = products;
            var newrecord = products.OrderByDescending(a => a.Id).ToList();
            model.newProducts = newrecord;
            var nwbanners = ent.BannerImages.Where(a => a.IsActive).ToList();
            model.Banners = nwbanners;

            model.HotProducts = HotProducts;
            var Hotbanners = ent.BannerImages.Where(a => a.IsActive).ToList();
            model.Banners = Hotbanners;

            model.RecommendProducts = rmdProducts;
            var banners = ent.BannerImages.Where(a => a.IsActive).ToList();
            model.Banners = banners;

            var ProBanners = ent.promotionalbanners.ToList();
            model.ProBanners = ProBanners;

            return View(model);
        }

        public ActionResult NewArrivalProduct()
        {
            //New Arrivals

            var model = new HomeModel();
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode

                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                PremiumAmount = p.PremiumAmount,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks = p.IsStock == true ? "yes" : "No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                           ).OrderByDescending(a => a.Id).ToList();
            foreach (var prod in products)
            {
                if (prod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == prod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = prod.Metric_Id ?? 0,
                        Product_Id = prod.Id,
                        Weight = prod.Quantity ?? 0,
                        Price = prod.OurPrice ?? prod.Price ?? 0,
                        IsStock = prod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == prod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    prod.Variants = variants;
                }
            }
            model.newProducts = products;
            return View(model);
        }

        public ActionResult OrganicProduct()
        {
            //Feature product
            var model = new HomeModel();
            var FeatureProducts = (from p in ent.Products
                                   join c in ent.Categories on p.Category_Id equals c.Id
                                   join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                                   where p.IsFeatureProduct == true

                                   select new ProductModel
                                   {
                                       ProductName = p.ProductName,
                                       ProductImage = p.ProductImage,
                                       CategoryName = c.CategoryName,
                                       Metrics = m.Metrics,
                                       Id = p.Id,
                                       Price = p.Price,
                                       OurPrice = p.OurPrice,
                                       ProductDescription = p.ProductDescription,
                                       IsStock = p.IsStock,
                                       IsStocks = p.IsStock == true ? "yes" : "No",
                                       Quantity = p.Quantity,
                                       IsVariant = p.IsVariant,
                                       Metric_Id = p.Metric_Id,
                                       PremiumAmount = p.PremiumAmount,
                                   }
                               ).ToList();
            foreach (var rprod in FeatureProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }
            model.FeatureProducts = FeatureProducts;
            return View(model);
        }

        public ActionResult ReccomendedProduct()
        {
            // recommendation products
            var model = new HomeModel();
            var rmdProducts = (from p in ent.Products
                               join c in ent.Categories on p.Category_Id equals c.Id
                               join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                               where p.IsRecomend == true

                               select new ProductModel
                               {
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   CategoryName = c.CategoryName,
                                   Metrics = m.Metrics,
                                   Id = p.Id,
                                   Price = p.Price,
                                   OurPrice = p.OurPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks = p.IsStock == true ? "yes" : "No",
                                   Quantity = p.Quantity,
                                   IsVariant = p.IsVariant,
                                   Metric_Id = p.Metric_Id,
                                   PremiumAmount = p.PremiumAmount,
                               }
                           ).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in rmdProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }
            model.RecommendProducts = rmdProducts;
            return View(model);
        }

        public ActionResult HotDealProduct()
        {
            //Hot deals

            var model = new HomeModel();
            var HotProducts = (from p in ent.Products
                               join c in ent.Categories on p.Category_Id equals c.Id
                               join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                               where p.IsHotdeals == true

                               select new ProductModel
                               {
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   CategoryName = c.CategoryName,
                                   Metrics = m.Metrics,
                                   Id = p.Id,
                                   Price = p.Price,
                                   OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                   DiscountPrice=p.DiscountPrice,
                                   //OurPrice = p.OurPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks = p.IsStock == true ? "yes" : "No",
                                   Quantity = p.Quantity,
                                   IsVariant = p.IsVariant,
                                   Metric_Id = p.Metric_Id,
                                   PremiumAmount = p.PremiumAmount,
                               }
                               ).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in HotProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }
            model.HotProducts = HotProducts;
            return View(model);
        }

        public ActionResult SpecialProducts()
        {
            //Speacial
            var model = new HomeModel();
            var SpeacialProducts = (from p in ent.Products
                                    join c in ent.Categories on p.Category_Id equals c.Id
                                    join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                                    where p.IsSpecial == true

                                    select new ProductModel
                                    {
                                        ProductName = p.ProductName,
                                        ProductImage = p.ProductImage,
                                        CategoryName = c.CategoryName,
                                        Metrics = m.Metrics,
                                        Id = p.Id,
                                        Price = p.Price,
                                        OurPrice = p.OurPrice,
                                        ProductDescription = p.ProductDescription,
                                        IsStock = p.IsStock,
                                        IsStocks = p.IsStock == true ? "yes" : "No",
                                        Quantity = p.Quantity,
                                        IsVariant = p.IsVariant,
                                        Metric_Id = p.Metric_Id,
                                        PremiumAmount = p.PremiumAmount,
                                    }
                             ).OrderByDescending(a => a.Id).ToList();
            foreach (var rprod in SpeacialProducts)
            {
                if (rprod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == rprod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = rprod.Metric_Id ?? 0,
                        Product_Id = rprod.Id,
                        Weight = rprod.Quantity ?? 0,
                        Price = rprod.OurPrice ?? rprod.Price ?? 0,
                        IsStock = rprod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == rprod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    rprod.Variants = variants;
                }
            }

            model.specialProducts = SpeacialProducts;
            return View(model);
        }
        public JsonResult GetProduct(string term)
        {
            var data = (from N in ent.Products
                        where N.ProductName.StartsWith(term)
                        select new { N.ProductName, N.Id });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Blog()
        {
            var model = new BlogMasterDTO();
            string q = @"select * from BlogMaster order by Id desc";
            var data = ent.Database.SqlQuery<Blogs>(q).ToList();
            model.Blog = data;
            return View(model);
        }

        //public ActionResult BlogDetails(string burl)
        //{
        //    var model = new BlogMasterDTO();
        //    string q = @"select * from BlogMaster where Url='" + burl + "'";
        //    var data = ent.Database.SqlQuery<Blogs>(q).ToList();
        //    //model.BlogImage = data.FirstOrDefault().BlogImage;
        //    model.Short = data.FirstOrDefault().Short;
        //    model.Heading = data.FirstOrDefault().Heading;
        //    model.Date = data.FirstOrDefault().Date;
        //    model.Description = data.FirstOrDefault().Description;
        //    return View(model);
        //}

        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContentPage(int menuId)
        {
            var record = ent.ContentPages.FirstOrDefault(a => a.Menu_Id == menuId);
            return View(record);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult LoyalityClub()
        {
            return View();
        }
        public ActionResult PremiumMember()
        {
            return View();
        }
        public ActionResult AdvertiseYourProduct()
        {
            return View();
        }
        public ActionResult SellOnKKD()
        {
            return View();
        }
        public ActionResult ShippingAndDelivery()
        {
            return View();
        }
        public ActionResult ReturnsAndExchenge()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult Categories()
        {
            //var records = ent.Categories.AsNoTracking().ToList();
            //return View(records);
            var data1 = (from cat in ent.Categories
                         where cat.ParentCategory == null
                         select new CategoryModel
                         {
                             Id = cat.Id,
                             CategoryName = cat.CategoryName,
                             ParentCategory = cat.ParentCategory,
                         }).ToList();
            var data2 = (from subcat in ent.SubCategories
                         select new SubCategorymodal
                         {
                             Id=subcat.Id,
                             Name = subcat.Name,
                             MainCat_Id = subcat.MainCat_Id
                         }).ToList();
            AllCategoryModel model = new AllCategoryModel
            {
                CategoryLists = data1,
                SubCategoryLists = data2
            };
            return View(model);
        }

        public ActionResult CategoryWithProduct()
        {
            var rm = new CatWithProdVm();
            var cats = (from cat in ent.Categories
                        select new CatWithProdModel
                        {
                            CatId = cat.Id,
                            CategoryImage = cat.CategoryImage,
                            CategoryName = cat.CategoryName
                        }
                        ).Take(4).ToList();
            foreach (var cat in cats)
            {
                var prod = (from p in ent.Products
                            where p.Category_Id == cat.CatId
                            select new ProdModel
                            {
                                ProdId = p.Id,
                                ProdName = p.ProductName
                            }
                            ).Take(5).ToList();
                cat.Products = prod;
            }
            rm.Cateogries = cats;
            return PartialView(rm);
            return PartialView();
        }
        public ActionResult Products(int categoryId, int? page)
        {
            var model = new ProductDisplayModel();
            page = page ?? 1;
            int pageSize = 20;
            var category = ent.SubCategories.Find(categoryId);
            model.CategoryName = category.Name;
            int total = ent.Products.Where(a => a.Category_Id == categoryId).Count();
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            where p.SubId == categoryId
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks = p.IsStock == true ? "yes" : "No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                           ).ToList();
            foreach (var prod in products)
            {
                if (prod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == prod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = prod.Metric_Id ?? 0,
                        Product_Id = prod.Id,
                        Weight = prod.Quantity ?? 0,
                        Price = prod.OurPrice ?? prod.Price ?? 0,
                        IsStock = prod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == prod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    prod.Variants = variants;
                }
            }
            model.Products = products;
            model.Page = page;
            model.NumberOfPages = (int)noOfPages;
            model.Category_Id = categoryId;
            return View(model);
        }

        public ActionResult ProductDetail(int productId)
        {

            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            where p.Id == productId
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                CategoryName = c.CategoryName,
                                Category_Id = p.Category_Id,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks=p.IsStock==true?"yes":"No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                          ).FirstOrDefault();

            if (products != null && products.IsVariant)
            {
                var variants = new List<VariantModel>();
                var metric = ent.Metrics.Find(products.Metric_Id);
                var v = new VariantModel
                {
                    Metric = metric == null ? "" : metric.Metrics,
                    Metric_Id = products.Metric_Id ?? 0,
                    Product_Id = products.Id,
                    Weight = products.Quantity ?? 0,
                    Price = products.OurPrice ?? products.Price ?? 0,
                    IsStock = products.IsStock
                };
                variants.Add(v);
                var vars = (from var in ent.Product_Availability
                            join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                            from vm in var_me.DefaultIfEmpty()
                            where var.Product_Id == productId
                            select new VariantModel
                            {
                                Product_Id = var.Product_Id ?? 0,
                                Metric_Id = var.Metrics_Id ?? 0,
                                Weight = var.Quantity ?? 0,
                                Metric = vm.Metrics,
                                Price = var.OurPrice ?? var.Price,
                                Variant_Id = var.Id,
                                IsStock = var.IsAvailable
                            }
                                ).ToList();
                variants.AddRange(vars);
                products.Variants = variants;
            }

            return View(products);
        }

        public ActionResult AllProducts(int categoryId, int? page)
        {
            var model = new ProductDisplayModel();
            page = page ?? 1;
            int pageSize = 20;
            decimal noOfPages = Math.Ceiling((decimal)2 / pageSize);
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            where p.Category_Id== categoryId
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks = p.IsStock == true ? "yes" : "No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                           ).ToList();
            foreach (var prod in products)
            {
                if (prod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == prod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = prod.Metric_Id ?? 0,
                        Product_Id = prod.Id,
                        Weight = prod.Quantity ?? 0,
                        Price = prod.OurPrice ?? prod.Price ?? 0,
                        IsStock = prod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == prod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    prod.Variants = variants;
                }
            }
            model.Products = products;
            model.Page = page;
            model.NumberOfPages = (int)noOfPages;
            return View(model);
        }

        public ActionResult SubCatProducts(int subCatId, int? page)
        {
            var model = new ProductDisplayModel();
            page = page ?? 1;
            int pageSize = 20;
            decimal noOfPages = Math.Ceiling((decimal)2 / pageSize);
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            where p.SubId == subCatId
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.OurPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks = p.IsStock == true ? "yes" : "No",
                                Quantity = p.Quantity,
                                IsVariant = p.IsVariant,
                                Metric_Id = p.Metric_Id
                            }
                           ).ToList();
            foreach (var prod in products)
            {
                if (prod.IsVariant)
                {
                    var variants = new List<VariantModel>();
                    var metric = ent.Metrics.Where(a => a.MetricCode == prod.Metric_Id).FirstOrDefault();
                    var v = new VariantModel
                    {
                        Metric = metric == null ? "" : metric.Metrics,
                        Metric_Id = prod.Metric_Id ?? 0,
                        Product_Id = prod.Id,
                        Weight = prod.Quantity ?? 0,
                        Price = prod.OurPrice ?? prod.Price ?? 0,
                        IsStock = prod.IsStock
                    };
                    variants.Add(v);
                    var vars = (from var in ent.Product_Availability
                                join me in ent.Metrics on var.Metrics_Id equals me.MetricCode into var_me
                                from vm in var_me.DefaultIfEmpty()
                                where var.Product_Id == prod.Id
                                select new VariantModel
                                {
                                    Product_Id = var.Product_Id ?? 0,
                                    Metric_Id = var.Metrics_Id ?? 0,
                                    Weight = var.Quantity ?? 0,
                                    Metric = vm.Metrics,
                                    Price = var.OurPrice ?? var.Price,
                                    Variant_Id = var.Id,
                                    IsStock = var.IsAvailable
                                }
                                    ).ToList();
                    variants.AddRange(vars);
                    prod.Variants = variants;
                }
            }
            model.Products = products;
            model.Page = page;
            model.NumberOfPages = (int)noOfPages;
            return View(model);
        }
        public ActionResult GetLocation(string term)
        {
            var data = ent.DeliveryLocations.Where(a => a.LocationName.ToUpper().StartsWith(term.ToUpper())).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitEnquiry(EnquiryModel model)
        {
            try
            {
                var str = new Enquiry();
                string msg = "Name : " + model.Name + "<br/>";
                msg += "Email : " + model.Email + "<br/>";
                msg += "Mobile : " + model.Mobile + "<br/>";
                msg += "Message : " + model.Message + "<br/>";
                str.Name = model.Name;
                str.Email = model.Email;
                str.Mobile = model.Mobile;
                str.Message = model.Message;
                ent.Enquiries.Add(str);
                ent.SaveChanges();
                string recepient = model.Email;
                EmailOperations.SendEmail(recepient, "Website enquiry", msg, true);
                return Content("ok");

            }
            catch
            {
                return Content("Server error.");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPWD model)
        {
            try
            {

                var u = ent.Customers.Where(p => p.Email_Id.Equals(model.Email_Id)).FirstOrDefault();  //check emailid from table 
                if (u != null)
                {
                    string sender = ConfigurationManager.AppSettings["smtpUser"];
                    string password = ConfigurationManager.AppSettings["smtpPass"];
                    string pwd = u.Password;
                    MailMessage msz = new MailMessage();
                    msz.To.Add(model.Email_Id);       //Where mail will be sent 
                    msz.From = new MailAddress(sender);
                    msz.Subject = "Password Recovery";
                    msz.Body = "Password: " + pwd;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(sender, password);
                    smtp.EnableSsl = true;
                    smtp.Send(msz);
                    ModelState.Clear();
                    ViewBag.Message = "Password has been sent to your registerd emailid ";
                }
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ViewBag.Message = " Server error";
            }
            return View();
        }

        public ActionResult Certificate()
        {
            return View();
        }

        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(CustomerDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var customer = Mapper.Map<Customer>(model);
                ent.Customers.Add(customer);
                ent.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error";
                return View(model);
            }

        }

        public ActionResult Login(string url)
        {
            var model = new LoginDTOModel();
            model.url = url;
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginDTOModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = ent.Customers.FirstOrDefault(a => a.Email_Id == model.Email_Id);
            if (user == null)
            {
                TempData["msg"] = "User does not exist";
                return RedirectToAction("Login");
            }
            else
            {
                if (user.Password == model.Password)
                {
                    HttpCookie cookie = new HttpCookie("user", user.Email_Id);
                    cookie.Expires.AddDays(30);
                    Response.Cookies.Add(cookie);
                    if (!string.IsNullOrEmpty(model.url))
                    {
                        return Redirect(model.url);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "Invalid username or password";
                    return RedirectToAction("Login");
                }
            }
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["user"] != null)
            {
                HttpCookie cookie = new HttpCookie("user");
                Session.Abandon();
                Response.Cookies.Clear();
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
                TempData["msg"] = "User has been logged out";
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordUserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (UserCookie.IsInCookie)
            {
                try
                {
                    var user = ent.Customers.Find(UserCookie.ClientId);
                    if (user != null)
                    {
                        if (user.Password != model.OldPassword)
                            TempData["msg"] = "Wrong old password.";
                        else
                        {
                            user.Password = model.NewPassword;
                            ent.SaveChanges();
                            TempData["msg"] = "Password has been updated.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server error";
                }
            }
            else
                TempData["msg"] = "User is not logged in";
            return View();
        }

        public ActionResult OrderHistory(int? page)
        {
            var OrderDetails = new CustomerOrderModel();
            var cookie = Request.Cookies["user"];
            string cookieVal = cookie == null ? "" : cookie.Value;
            var user = ent.Customers.FirstOrDefault(a => a.Email_Id == cookieVal || a.Password == cookieVal);
            if (user != null)
            {
                int UserId = user.User_Id;
                var od = (from ord in ent.Orders
                          where ord.Client_Id == UserId
                          select new CODOrderDTO
                          {
                              Id = ord.Id,
                              Phone = ord.PhoneNumber,
                              Total_Price = ord.Total,
                              PaymentMode = ord.PaymentMode,
                              Address = ord.Address,
                              OrderDate = ord.OrderDate,
                              IsCancel = ord.IsCancel,
                              Total_Items = ent.OrderDetails.Where(a => a.Order_Id == ord.Id).Count()
                          }).ToList();
                int total = od.Count;
                page = page ?? 1;
                int pageSize = 30;
                decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
                OrderDetails.NumberOfPages = (int)noOfPages;
                OrderDetails.Page = page;
                od = od.OrderByDescending(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
                OrderDetails.Data = od;
                return View(OrderDetails);
            }
            else
            {
                return View(OrderDetails);
            }
        }
        public ActionResult OrderDetail(int Id)
        {
            ViewBag.OrderId = Id;
            var model = new CustomerOrderModel();
            var data = (from od in ent.OrderDetails
                        join pd in ent.Products on od.Product_Id equals pd.Id
                        join ord in ent.Orders on od.Order_Id equals ord.Id
                        join os in ent.OrderStatus on od.OrderStatus_Id equals os.Id
                        where od.Order_Id == Id
                        select new CODOrderDTO
                        {
                            Id = od.Id,
                            Order_Id = od.Order_Id,
                            Product_Id = od.Product_Id,
                            ProductName = od.ProductName,
                            Price = od.Price,
                            ProductImage = pd.ProductImage,
                            Quantity = od.Quantity,
                            FinalPrice = od.FinalPrice,
                            Total_Price = ord.Total,
                            IsCancel = od.IsCancel
                        }).ToList();
            model.Data = data;
            model.Total_Price = data.Where(a => a.Order_Id == Id).Select(a => a.Total_Price).FirstOrDefault();
            return View(model);
        }

        public ActionResult CancelOrder(int id)
        {
            var record = ent.OrderDetails.Find(id);
            try
            {
                record.IsCancel = true;
                ent.SaveChanges();
                // send notification to customer
                var order = ent.Orders.Find(record.Order_Id);
                var customerEmail = order.Email;
                string customerPhone = order.PhoneNumber;
                // send notification to customer
                string msg = "Your order of " + record.ProductName + " is cancelled.";
                EmailOperations.SendEmail(order.Email, "Order cancellation", msg, true);
                // send notification to admin
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("OrderDetail", new { orderId = record.Order_Id });
        }

        public ActionResult AddPaymentType()
        {
            return View();
        }

        public ActionResult ManagePaymentType()
        {
            return View();
        }

        public ActionResult CheckDeliveryCode(string term)
        {
            bool isAvailable = true;
            var data = ent.DeliveryPincodes.Where(a => a.Pincode.Equals(term)).FirstOrDefault();
            if (data == null)
            {
                isAvailable = false;
            }
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dp(int Id)
        {
            var model = new DownPymentDTO();
            if (Id != 0)
            {
                model.product_Id = Id;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Dp(DownPymentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var dnpyment = Mapper.Map<DownPyment>(model);
            ent.DownPyments.Add(dnpyment);
            ent.SaveChanges();
            ModelState.Clear();
            return View(model);
        }

        public ActionResult CategoryDetail(int categoryid)
        {
            var data = ent.Database.SqlQuery<SubCategorymodal>(@"select s.Id,S.Name,s.CatImage, count(p.SubId) as totalProd from SubCategory s
join Product p on
s.Id = p.SubId
where s.MainCat_Id=" + categoryid + " group by s.Id,S.Name,s.CatImage").ToList();

            return View(data);
        }

        public JsonResult SearchProduct(string term)
        {
            List<string> Products = ent.Products.Where(a => a.ProductName.StartsWith(term)).Select(a=>a.ProductName).ToList();
            return Json(Products, JsonRequestBehavior.AllowGet);
        }
    }
}
