using KisanKiDukan.Utilities;
using KisanKiDukan.Models.APIModels;
using KisanKiDukan.Models.APIModels.ResponseModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.IO;
using AutoMapper;



namespace KisanKiDukan.Controllers
{
    public class ProductAPIController : ApiController
    {
        DbEntities ent = new DbEntities();
        //====SearchProducts api====
        [HttpGet, Route("api/ProductAPI/SearchProducts")]
        public IHttpActionResult SearchProducts(string term)
        {
            try
            {
                var data = (from N in ent.Products
                            where N.ProductName.StartsWith(term)
                            select new { N.ProductName, N.Id }).ToList();
                return Ok(data);
                //if(data.Count > 0)
                //{
                //    return Ok(data);
                //}
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //====GetProductByCategories api====

        [HttpGet, Route("api/ProductAPI/GetProductByCategories")]
        public IHttpActionResult GetProductByCategories(int id, int userId)
        {
            var model = new ProductShowListModel();
            var userdata = ent.Customers.Find(userId);
            if (userdata.IsPremiumMember == true)
            {
                string query = @"select pr.Id as Product_Id,pr.ShippingCharge,sctr.Name as SubCategoryName, sctr.UpToText,pr.VendorId,vr.VendorName,pr.ProductName,pr.ProductImage,pr.Category_Id,ctr.CategoryName,ctr.CategoryImage,pr.Price,pr.Metric_Id as Metrics_Id,mtr.Metrics,pr.ProductDescription,pr.IsStock as StockAvailability ,isnull(pr.Quantity,'')Weight,pr.PremiumAmount,pr.IsVariant,dbo.GetQtyInKart('" + userId + "',pr.Id,pr.Metric_Id,pr.quantity) as Qty from  Product pr join  Category ctr on pr.Category_Id = ctr.Id join SubCategory sctr on ctr.Id = sctr.MainCat_Id join Vendor vr on pr.VendorId=vr.Id left join   Metric mtr on pr.Metric_Id = mtr.MetricCode where pr.SubId =" + id + "";
                var data = ent.Database.SqlQuery<ProductShowByCategoriesModel>(query).ToList();
                if (data.Count > 0)
                {
                    model.Products = data;
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);
                }
                model.Message = "No Product found";
                model.Status = "0";
                return Ok(model);
            }
            else
            {
                string query = @"select pr.Id as Product_Id,pr.ShippingCharge,pr.ProductName,pr.ProductImage,pr.VendorId,vr.VendorName,pr.Category_Id,ctr.CategoryName,ctr.CategoryImage, scb.Name as SubCategoryName,scb.UpToText, scb.CatImage as SubCategoryImage, pr.Price,pr.Metric_Id as Metrics_Id,mtr.Metrics,pr.ProductDescription,pr.IsStock as StockAvailability ,isnull(pr.Quantity,'')Weight,pr.OurPrice,pr.IsVariant,dbo.GetQtyInKart('" + userId + "',pr.Id,pr.Metric_Id,pr.quantity) as Qty from  Product pr join  Category ctr on pr.Category_Id = ctr.Id join SubCategory scb on ctr.Id = scb.MainCat_Id join Vendor vr on pr.VendorId=vr.Id left join   Metric mtr on pr.Metric_Id = mtr.MetricCode where pr.SubId =" + id + "";
                var data = ent.Database.SqlQuery<ProductShowByCategoriesModel>(query).ToList();
                if (data.Count > 0)
                {
                    model.Products = data;
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);
                }
                model.Message = "No Product found";
                model.Status = "0";
                return Ok(model);
            }
        }

        //====GetAllProducts api====

        [HttpGet] 
        public IHttpActionResult GetAllProducts(int userId)
        {
            var model = new ProductShowListModel();
            string query = @"select pr.Id as Product_Id,pr.ShippingCharge,pr.ProductName,pr.ProductImage,pr.Category_Id,ctr.CategoryName,ctr.CategoryImage,pr.Price,pr.Metric_Id as Metrics_Id,mtr.Metrics,pr.ProductDescription,pr.IsStock as StockAvailability ,isnull(pr.Quantity,'')Weight,pr.OurPrice,pr.IsVariant,dbo.GetQtyInKart('" + userId + "',pr.Id,pr.Metric_Id,pr.quantity) as Qty from  Product pr join  Category ctr on pr.Category_Id = ctr.Id left join   Metric mtr on pr.Metric_Id = mtr.MetricCode";
            var data = ent.Database.SqlQuery<ProductShowByCategoriesModel>(query).ToList();
            if (data.Count > 0)
            {
                model.Products = data;
                model.Message = "success";
                model.Status = "1";
                return Ok(model);
            }

            model.Message = "No Product found";
            model.Status = "0";
            return Ok(model);

        }

        //====GetCategory api====

        [HttpGet, Route("api/ProductAPI/GetCategory")]
        public IHttpActionResult GetCategory()
        {
            var model = new CategoriesApiReturnModel();
            dynamic obj = new ExpandoObject();
            string q = @"select * from Category";
            var cateogries = ent.Database.SqlQuery<CategoryApiModel>(q).ToList();
            foreach (var s in cateogries)
            {
                string subcat = @"select *, SubCategory.CatImage as SubCategroyImage , SubCategory.MainCat_Id from SubCategory join Category on Category.Id = SubCategory.MainCat_Id where SubCategory.MainCat_Id=" + s.Id;
                var sub = ent.Database.SqlQuery<SubCategories>(subcat).ToList();
                s.SubCategories = sub;
            }
            model.Category = cateogries;
            model.Message = "Success";
            model.Status = "1";
            return Ok(model);
        }

        //====GetProductbycategoryId api====

        [HttpGet]
        public IHttpActionResult GetProductbycategoryId(int id)
        {
          var result = ent.Products.Where(x => x.Category_Id == id).ToList();
            return Ok(new {products=result});
        }

        //====ProductOrder api====
        [HttpPost, Route("api/ProductAPI/ProductOrder")]
        public IHttpActionResult ProductOrder(OrderModel model)
        {
            using (var trans = ent.Database.BeginTransaction())
            {
                var order = new Order();
                var orderDetail = new List<OrderDetail>();
                try
                {
                    if (model.OrderData.Count > 0)
                    {
                        // Generate Order start
                        order.Id = 0;
                        order.OrderStatus_Id = 1; // 1 for pending
                        order.PaymentMode = model.OrderData.Select(a => a.PaymentMode).FirstOrDefault();
                        var data = ent.Wallets.Where(a => a.User_Id == model.User_Id).FirstOrDefault();
                        //check payment mode on json data then store PaymentType_Id
                        switch (order.PaymentMode)
                        {
                            case "COD":
                                order.PaymentType_Id = 1;
                                break;
                            case "ONLINE":
                                order.PaymentType_Id = 2;
                                order.PaymentStatus = true;
                                break;
                            case "WALLET":
                                order.PaymentType_Id = 3;
                                order.PaymentStatus = true;
                                data.Mebr_Amount = model.Mebr_Amount;//update wallet amount
                                break;
                            default:
                                break;
                        }
                        order.OrderDate = DateTime.Now;
                        order.Total = model.Total_Price;
                        order.Client_Id = model.User_Id;
                        order.Address = model.Shipping_Address;
                        order.PhoneNumber = model.Alternate_No;
                        order.DeliveryCharges = model.DeliveryCharges;
                        order.ScheduleDate = model.SchedultedDate;
                        order.TimeSlot = model.SchedultedTimeSlot;
                        order.SlotCode = model.SlotCode;
                        order.VendorId = model.VendorId;
                        ent.Orders.Add(order);
                        ent.SaveChanges();
                        // Generate Order end
                        // saving order detail
                        if (order.Id > 0)
                        {
                            foreach (var item in model.OrderData)
                            {
                                var metric = ent.Metrics.Where(a => a.Metrics.Equals(item.Metrics)).FirstOrDefault();

                                var od = new OrderDetail
                                {
                                    Order_Id = order.Id,
                                    ProductName = item.ProductName,
                                    Product_Id = item.Product_Id,
                                    Category_Id = item.CategoryId,
                                    User_Id = order.Client_Id,
                                    Price = item.Product_Price,
                                    FinalPrice = item.FinalPrice,
                                    Quantity = item.Quantity,
                                    Metrics = item.Metrics,
                                    OrderStatus_Id = 1,
                                    VendorId = item.VendorId,
                                    Volume = item.Weight
                                };
                                ent.OrderDetails.Add(od);
                                orderDetail.Add(od);
                                //updating central store product quantity
                                var csData = ent.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + item.Product_Id + " , @storeId=0  , @wheight =" + item.Weight + " , @metricCode =  " + metric.MetricCode + "").FirstOrDefault();

                                if (csData != null && csData.Stock > 0)
                                {
                                    csData.Stock = csData.Stock - item.Quantity;
                                    ent.SaveChanges();
                                }
                                else
                                {
                                    //updating  product available status
                                    var prodData = ent.Products.Where(a => a.Id == item.Product_Id && a.Quantity == item.Weight && a.Metric_Id == metric.MetricCode).FirstOrDefault();
                                    if (prodData != null)
                                    {
                                        prodData.IsStock = false;
                                        ent.SaveChanges();
                                    }
                                    else
                                    {
                                        //check product varient if main product not found and change available status
                                        var prodVariant = ent.Product_Availability.Where(a => a.Id == item.Product_Id && a.Quantity == item.Weight && a.Metrics_Id == metric.MetricCode).FirstOrDefault();
                                        if (prodVariant != null)
                                            prodVariant.IsAvailable = false;
                                        ent.SaveChanges();
                                    }
                                }
                            }
                            ent.SaveChanges();
                            // Updating the total of order
                        }
                        //empty the kart
                        var kart = ent.Karts.FirstOrDefault(a => a.Client_Id == model.User_Id);
                        if (kart != null)
                        {
                            var kartDetail = ent.KartDetails.Where(a => a.Kart_Id == kart.Id).ToList();
                            if (kartDetail.Count > 0)
                            {
                                ent.KartDetails.RemoveRange(kartDetail);
                                ent.Karts.Remove(kart);
                                ent.SaveChanges();
                            }
                        }
                        model.Message = "Order success";
                        model.Status = "1";
                        trans.Commit();
                        //send sms user to track order detail
                        string msg = "Hi " + order.Name + "\n";
                        msg += "We have recieved your order of. \n";
                        foreach (var od in orderDetail)
                        {
                            msg += od.Quantity + " " + od.ProductName + " " + od.Volume + " " + od.Metrics + "\n";
                        }
                        SmsOperation.SendSms(order.PhoneNumber, msg);
                        DateTime? CurrentDate = DateTime.Now.Date;
                        var orderData = ent.Orders.Find(order.Id);
                        var userData = ent.Customers.Find(model.User_Id);
                        var orderRecord = (from od in ent.OrderDetails
                                           join prod in ent.Products on od.Product_Id equals prod.Id
                                           where od.Order_Id == orderData.Id
                                           select new OrderDetailModel
                                           {
                                               ProductName = prod.ProductName,
                                               Quantity = od.Quantity,
                                               Price = (double)prod.OurPrice,
                                               FinalPrice = od.Quantity * (double)prod.OurPrice
                                           }
                                           ).ToList();
                        EmailOperations.SendOrderEmail(userData.FullName, userData.Email_Id, userData.Phone, orderData.Address, model.Total_Price, order.OrderDate, orderRecord);
                        return Ok(model);
                    }
                    model.Message = "Not success";
                    model.Status = "0";
                    trans.Rollback();
                    return Ok(model);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return InternalServerError(ex);
                }
            }
        }

        //====CategoryOrders api====
        [HttpGet, Route("api/ProductAPI/CategoryOrders")]
        public IHttpActionResult CategoryOrders(int id)
        {
            var model = new AllOrderListAPI();
            var data = (from odr in ent.Orders
                        join os in ent.OrderStatus on odr.OrderStatus_Id equals os.StatusCode
                        where odr.Client_Id == id && odr.IsCancel == false
                        select new AllOrderApi
                        {
                            Order_Id = odr.Id,
                            OrderDate = odr.OrderDate,
                            PaymentMode = odr.PaymentMode,
                            Shipping_Address = odr.Address,
                            AlternateNumber = odr.PhoneNumber,
                            Grand_Total = odr.Total,
                            IsCancel = odr.IsCancel,
                            Total_Items = ent.OrderDetails.Where(od => od.Order_Id == odr.Id).Count(),
                            DeliveryCharges = odr.DeliveryCharges,
                            ScheduledDeliveryDate = odr.ScheduleDate,
                            TimeSlot = odr.TimeSlot,
                            OrderStatus = os.StatusName
                        }).ToList();

            if (data.Count > 0)
            {
                model.Orders = data;
                model.Message = "Success";
                model.Status = "1";
                return Ok(model);
            }
            model.Message = "No Order Found";
            model.Status = "0";
            return Ok(model);
        }

        //====GetProductOrderUser api====

        [HttpGet, Route("api/ProductAPI/GetProductOrderUser")]
        public IHttpActionResult GetProductOrderUser(int Order_Id, int User_Id)
        {
            var model = new UserOrderDetailModel();
            var order = ent.Database.SqlQuery<OrderData>("@select o.*,os.StatusName from [Order] o join OrderStatus os on o.OrderStatus_Id = os.StatusCode").FirstOrDefault();
            if (order != null)
            {
                model.DeliveryCharges = order.DeliveryCharges;
            }
            var ord = (from a in ent.Orders
                       join b in ent.OrderDetails on a.Id equals b.Order_Id
                       join c in ent.Products on b.Product_Id equals c.Id
                       join d in ent.Customers on a.Client_Id equals d.User_Id
                       join os in ent.OrderStatus on a.OrderStatus_Id equals os.StatusCode
                       where a.Client_Id == User_Id && a.Id == Order_Id

                       select new ProductOrderModel
                       {
                           Order_Id = a.Id,
                           Product_Id = b.Product_Id,
                           ProductName = b.ProductName,
                           Quantity = b.Quantity,
                           Metrics = b.Metrics,
                           Product_Price = b.Price,
                           ProductImage = c.ProductImage,
                           Description = c.ProductDescription,
                           OrderDate = a.OrderDate,
                           Address = d.Address,
                           PaymentMode = a.PaymentMode,
                           Phone = d.Phone,
                           FinalPrice = b.FinalPrice,
                           OrderStatus = os.StatusName
                       }).ToList();

            if (ord.Count > 0)
            {
                model.OrderDetail = ord;
                model.Message = "success";
                model.Status = "1";
                return Ok(model);
            }
            else
            {
                model.Message = "No Order found";
                model.Status = "0";
                return Ok(model);
            }
        }

        //====CancelledCategoryOrder api====

        [HttpGet, Route("api/ProductAPI/CancelledCategoryOrder")]
        public IHttpActionResult CancelledCategoryOrder(int id)
        {
            var model = new AllOrderListAPI();
            var data = (from odr in ent.Orders
                        where odr.Client_Id == id && odr.OrderStatus_Id == 4
                        select new AllOrderApi
                        {
                            Order_Id = odr.Id,
                            OrderDate = odr.OrderDate,
                            PaymentMode = odr.PaymentMode,
                            Shipping_Address = odr.Address,
                            AlternateNumber = odr.PhoneNumber,
                            Grand_Total = odr.Total,
                            Total_Items = ent.OrderDetails.Where(od => od.Order_Id == odr.Id).Count(),
                            IsCancel = odr.IsCancel,
                            DeliveryCharges = odr.DeliveryCharges,
                            CancellationDate = odr.StatusUpdateDate,
                            ScheduledDeliveryDate = odr.ScheduleDate,
                            TimeSlot = odr.TimeSlot
                        }).ToList();
            if (data.Count > 0)
            {
                model.Orders = data;
                model.Message = "Success";
                model.Status = "1";
                return Ok(model);
            }
            model.Message = "No Order Found";
            model.Status = "0";
            return Ok(model);
        }

        //====CancelledSubCategoryOrder api====

        [HttpGet, Route("api/ProductAPI/CancelledSubCategoryOrder")]
        public IHttpActionResult CancelledSubCategoryOrder(int Order_Id, int User_Id)
        {
            var model = new UserOrderDetailModel();

            var ord = (from a in ent.Orders
                       join b in ent.OrderDetails on a.Id equals b.Order_Id
                       join c in ent.Products on b.Product_Id equals c.Id
                       join d in ent.Customers on a.Client_Id equals d.User_Id
                       join os in ent.OrderStatus on a.OrderStatus_Id equals os.StatusCode
                       where a.Client_Id == User_Id && a.Id == Order_Id && a.OrderStatus_Id == 4
                       select new ProductOrderModel
                       {
                           Order_Id = a.Id,
                           Product_Id = b.Product_Id,
                           ProductName = b.ProductName,
                           Quantity = b.Quantity,
                           Metrics = b.Metrics,
                           Product_Price = b.Price,
                           ProductImage = c.ProductImage,
                           Description = c.ProductDescription,
                           OrderDate = a.OrderDate,
                           Address = d.Address,
                           PaymentMode = a.PaymentMode,
                           Phone = d.Phone,
                           FinalPrice = b.FinalPrice,
                           OrderStatus = os.StatusName
                       }).ToList();

            if (ord.Count > 0)
            {
                model.OrderDetail = ord;
                model.Message = "success";
                model.Status = "1";
                return Ok(model);
            }
            else
            {
                model.Message = "No Order found";
                model.Status = "0";
                return Ok(model);

            }
        }

        //====CancelOrderBy api====

        [HttpPost, Route("api/ProductAPI/CancelOrderBy")]
        public IHttpActionResult CancelOrderBy(CancelOrderReturnModel cod)
        {
            var model = new CancelOrderReturnModel();
            try
            {
                var record = ent.Orders.Find(cod.Order_Id);
                record.IsCancel = true;
                record.OrderStatus_Id = 4; // 4 for cancel
                record.StatusUpdateDate = DateTime.Now;
                ent.SaveChanges();
                if (record.Id > 0)
                {
                    var orderDetail = ent.OrderDetails.Where(a => a.Order_Id == record.Id).ToList();
                    if (orderDetail.Count > 0)
                    {
                        foreach (var item in orderDetail)
                        {
                            var od = new OrderDetail();
                            od.IsCancel = item.IsCancel = true;
                            od.OrderStatus_Id = 4;
                            od.StatusUpdateDate = DateTime.Now;
                            od.CancellationDate = DateTime.Now;
                        }
                    }
                    var pmode = record.PaymentMode;
                    var payStatus = record.PaymentStatus;
                    double amt = record.Total;
                    // refund start of user wallet
                    var user = ent.Wallets.Where(a => a.User_Id == cod.User_Id).FirstOrDefault();
                    if (user != null)
                    {
                        double walletAmt = user.Mebr_Amount;
                        switch (record.PaymentMode)
                        {
                            case "ONLINE":
                                user.Mebr_Amount = amt + walletAmt;

                                break;
                            case "WALLET":
                                user.Mebr_Amount = amt + walletAmt;
                                break;
                            default:
                                break;
                        }
                        if (record.PaymentStatus == true && record.PaymentMode == "COD")
                        {
                            user.Mebr_Amount = amt + walletAmt;
                        }
                    }
                    // refund end of user wallet
                    ent.SaveChanges();
                    model.Message = "success";
                    model.Status = "1";
                    model.Order_Id = cod.Order_Id;
                    model.Mebr_Amount = amt;
                    model.User_Id = cod.User_Id;
                    return Ok(model);
                }
                model.Message = "Not success";
                model.Status = "0";
                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        //====Banners api====

        [HttpGet, Route("api/ProductAPI/Banners")]
        public IHttpActionResult Banners()
        {
            dynamic model = new ExpandoObject();
            var banner = (from bi in ent.BannerImages

                          select new BannersModel
                          {

                              Id = bi.Id,
                              BannerPath = bi.BannerPath
                          }
                           ).ToList();
            if (banner.Count > 0)
            {
                model.Banner = banner;
                model.Message = "Success";
                model.Status = 1;
                return Ok(model);
            }

            model.Message = "No data found";
            model.Status = 0;
            return Ok(model);

        }

        //====PBanners api====

        [HttpGet, Route("api/ProductAPI/PBanners")]
        public IHttpActionResult PBanners()
        {
            dynamic model = new ExpandoObject();
            var banner = (from bi in ent.promotionalbanners

                          select new PBannersModel
                          {

                              Id = bi.Id,
                              promotionalbannerpath = bi.promotionalbannerpath
                          }
                           ).ToList();
            if (banner.Count > 0)
            {
                model.Banner = banner;
                model.Message = "Success";
                model.Status = 1;
                return Ok(model);
            }

            model.Message = "No data found";
            model.Status = 0;
            return Ok(model);

        }

        //====VariantByProduct api====

        [HttpGet, Route("api/ProductAPI/VariantByProduct")]
        public IHttpActionResult VariantByProduct(int id, int userId)
        {
            var model = new VarientVM();
            model.Product_Id = id;
            string query = @"select prd.Id,prd.IsAvailable as StockAvailability,prd.Product_Id,prd.Price,isnull(prd.Quantity,'')Weight,mtr.Metrics,prd.Metrics_Id,prd.OurPrice,dbo.GetQtyInKart('" + userId + "',prd.Product_Id,prd.Metrics_Id,prd.quantity) as Qty from Product_Availability prd join Product p on prd.Product_Id = p.Id join Metric mtr on prd.Metrics_Id = mtr.MetricCode where prd.Product_Id = " + id + "";
            var data = ent.Database.SqlQuery<ProductVarientReturn>(query).ToList();
            model.VList = data;

            if (model.VList.Count() > 0)
            {
                model.Message = "Success";
                model.Status = 1;
                return Ok(model);
            }

            model.Message = "No data found";
            model.Status = 0;
            return Ok(model);
        }

        //====GetOrderDetail api====

        [HttpGet]
        public IHttpActionResult GetOrderDetail(int orderId)
        {
            var rm = new OrderDetailRm();
            string query = @"select od.Id as OrderDetailId,od.Price,od.ProductName,od.FinalPrice,od.Quantity,od.Metrics,od.Volume as [Weight],od.IsCancel,Convert(nvarchar(100),CancellationDate,105) as CancellationDate, os.StatusName from orderdetail od 
join [Order] o on od.Order_Id = o.Id
join OrderStatus os on o.OrderStatus_Id = os.Id
where od.Order_Id=" + orderId;
            var data = ent.Database.SqlQuery<OrderDetailModel>(query).ToList();
            rm.OrderDetail = data;
            return Ok(rm);
        }

        //====CancelSingleItem api====
        [HttpGet]
        public IHttpActionResult CancelSingleItem(int orderDetailId)
        {
            var rm = new MessageModel();
            try
            {
                var data = ent.OrderDetails.Find(orderDetailId);
                data.IsCancel = true;
                data.CancellationDate = DateTime.Now;
                data.StatusUpdateDate = DateTime.Now;
                data.OrderStatus_Id = 4; // 4 for cancellation
                ent.SaveChanges();
                var order = ent.Orders.Find(data.Order_Id);
                if (order != null)
                {
                    if (!ent.OrderDetails.Any(a => a.Order_Id == order.Id && a.OrderStatus_Id == 4))
                    {
                        order.IsCancel = true;
                        order.OrderStatus_Id = 4;
                        order.StatusUpdateDate = DateTime.Now;
                        ent.SaveChanges();
                    }
                }
                rm.Status = 1;
                rm.Message = "This item has cancelled successfully";
            }
            catch (Exception ex)
            {
                rm.Status = 0;
                rm.Message = "server error";
            }
            return Ok(rm);
        }

        //====GetDeliveryCharge api====
        [HttpGet]
        public IHttpActionResult GetDeliveryCharge()
        {
            var data = ent.DeliveryChargeMasters.FirstOrDefault();
            dynamic obj = new ExpandoObject();
            if (data != null)
            {
                obj.DeliveryCharge = data.DeliveryCharge;
                obj.MinAmountForFreeDelivery = data.MinAmt;

            }
            obj.DeliveryCharge = 0;
            obj.MinAmountForFreeDelivery = 0;

            return Ok(obj);
        }

        //====CheckPincodeAvailability api====
        [HttpGet]
        public IHttpActionResult CheckPincodeAvailability(string pincode)
        {
            var isAvailable = ent.DeliveryPincodes.Any(a => a.Pincode == pincode);
            dynamic dta = new ExpandoObject();
            dta.IsAvailable = isAvailable;
            return Ok(dta);
        }

        //====GetDeliveryTimeSlots api====

        [HttpGet]
        public IHttpActionResult GetDeliveryTimeSlots()
        {
            string query = @"select SlotCode,
Convert(nvarchar(15),StartTime,100) +' - '+ Convert(nvarchar(15),EndTime,100) as TimeSlot
from DeliveryTimeSlot";
            var data = ent.Database.SqlQuery<TimeSlotResponse>(query).ToList();
            dynamic rm = new ExpandoObject();
            rm.TimeSlots = data;
            return Ok(rm);
        }

        //====GetproductbyIdanduserid api====

        [HttpGet, Route("api/ProductAPI/GetproductbyIdanduserid")]
        public IHttpActionResult GetproductbyIdanduserid(int id, int userId)
        {
            var model = new ProductShowListModel();
            var userdata = ent.Customers.Find(userId);
            if (userdata.IsPremiumMember == true)
            {
                var dadaa = new VariantModels();
                string query = @"select pr.Id as Product_Id,sctr.UpToText,pr.SubId,pr.ShippingCharge,pr.ProductName,pr.ProductImage,pr.VendorId,vr.VendorName,pr.Category_Id, pr.Price,pr.Metric_Id,mtr.Metrics,pr.ProductDescription,pr.IsStock ,isnull(pr.Quantity,'')Weight,pr.OurPrice,pr.IsVariant,dbo.GetQtyInKartDetail('" + userId + "',pr.Id) as Quantity from  Product pr join  Category ctr on pr.Category_Id = ctr.Id join Vendor vr on pr.VendorId=vr.Id left join Metric mtr on pr.Metric_Id = mtr.MetricCode join SubCategory sctr on pr.SubId = sctr.Id where pr.Id =" + id + "";
                var data = ent.Database.SqlQuery<ProductShowByIdAndUserIDModel>(query).ToList();
                var pr = ent.Products.Where(a => a.Id == id).FirstOrDefault();
                var deta = data.FirstOrDefault();
                dadaa.Metric_Id = pr.Metric_Id;
                dadaa.Product_Id = pr.Id;
                dadaa.Price = pr.Price;
                dadaa.OurPrice = pr.OurPrice;
                dadaa.IsStock = pr.IsStock;
                dadaa.Qty = deta.Qty;
                dadaa.Metric = deta.Metrics;
                dadaa.Weight = deta.Weight;
                dadaa.VendorId = deta.VendorId;
                string query1 = @"select prd.Id as Variant_Id,prd.IsAvailable as IsStock,prd.Product_Id,pr.VendorId,vr.VendorName,prd.OurPrice,prd.Price,isnull(prd.Quantity,'')Weight,mtr.Metrics as Metric,prd.Metrics_Id as Metric_Id ,prd.OurPrice,dbo.GetQtyInKart('" + userId + "',prd.Product_Id,prd.Metrics_Id,prd.quantity) as Qty from Product_Availability prd join Product p on prd.Product_Id = p.Id join Vendor vr on pr.VendorId=vr.Id join Metric mtr on prd.Metrics_Id = mtr.MetricCode where prd.Product_Id =" + id + "";
                var data1 = ent.Database.SqlQuery<VariantModels>(query1).ToList();
                data1.Add(dadaa);
                model.GetProducts = data;
                var gt = model.GetProducts.FirstOrDefault();
                gt.Variant = data1;
                if (data.Count > 0)
                {
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);
                }
                model.Message = "No Product found";
                model.Status = "0";
                return Ok(model);
            }
            else
            {
                var dadaa = new VariantModels();
                string query = @"select pr.Id as Product_Id,sctr.UpToText,pr.Metric_Id,pr.SubId,pr.ShippingCharge,pr.ProductName,pr.VendorId,pr.ProductImage,pr.Category_Id, pr.Price,pr.Metric_Id,mtr.Metrics,pr.ProductDescription,pr.IsStock ,isnull(pr.Quantity,'')Weight,pr.OurPrice,pr.IsVariant,dbo.GetQtyInKartDetail('" + userId + "',pr.Id) as Quantity from  Product pr join  Category ctr on pr.Category_Id = ctr.Id  left join   Metric mtr on pr.Metric_Id = mtr.MetricCode join SubCategory sctr on pr.SubId = sctr.Id where pr.Id =" + id + "";
                var data = ent.Database.SqlQuery<ProductShowByIdAndUserIDModel>(query).ToList();
                var pr = ent.Products.Where(a => a.Id == id).FirstOrDefault();
                var deta = data.FirstOrDefault();
                dadaa.Metric_Id = pr.Metric_Id;
                dadaa.Product_Id = pr.Id;
                dadaa.Price = pr.Price;
                dadaa.OurPrice = pr.OurPrice;
                dadaa.IsStock = pr.IsStock;
                dadaa.Qty = deta.Qty;
                dadaa.Metric = deta.Metrics;
                dadaa.Weight = deta.Weight;
                dadaa.VendorId = deta.VendorId;
                string query1 = @"select prd.Id as Variant_Id,prd.IsAvailable as IsStock,prd.Product_Id,prd.OurPrice,p.VendorId,prd.Price,isnull(prd.Quantity,'')Weight,mtr.Metrics as Metric ,prd.Metrics_Id as Metric_Id,prd.OurPrice,dbo.GetQtyInKart('" + userId + "',prd.Product_Id,prd.Metrics_Id,prd.quantity) as Qty from Product_Availability prd join Product p on prd.Product_Id = p.Id join Metric mtr on prd.Metrics_Id = mtr.MetricCode where prd.Product_Id =" + id + "";
                var data1 = ent.Database.SqlQuery<VariantModels>(query1).ToList();
                data1.Add(dadaa);
                model.GetProducts = data;
                var gt = model.GetProducts.FirstOrDefault();
                gt.Variant = data1;
                if (data.Count > 0)
                {
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);
                }
                model.Message = "No Product found";
                model.Status = "0";
                return Ok(model);
            }
        }

        //==== start Giftboxes api====

        [HttpGet, Route("api/ProductAPI/HotDetails")]
        public IHttpActionResult HotDetails()
        {
            var model = new HomeModelAPI();
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
                                   OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                   DiscountPrice = p.DiscountPrice,
                                   //OurPrice = p.OurPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks=p.IsStock==true? "In-Stock" : "Out Of-Stock",
                                   Quantity = p.Quantity,
                                   IsVariant = p.IsVariant,
                                   Metric_Id = p.Metric_Id,
                                   PremiumAmount = p.PremiumAmount,
                               }
                               ).OrderByDescending(a => a.Id).Take(8).ToList();
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

            model.HotDeals = HotProducts;
            return Ok(model);
        }

        [HttpGet, Route("api/ProductAPI/NewArrivalProduct")]
        public IHttpActionResult NewArrivalProduct()
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
                                OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                DiscountPrice = p.DiscountPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                 IsStocks=p.IsStock==true? "In-Stock" : "Out Of-Stock",
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
            return Ok(model);
        }
        [HttpGet, Route("api/ProductAPI/OrganicProduct")]
        public IHttpActionResult OrganicProduct()
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
                                       OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                       DiscountPrice = p.DiscountPrice,
                                       ProductDescription = p.ProductDescription,
                                       IsStock = p.IsStock,
                                       IsStocks=p.IsStock==true? "In-Stock" : "Out Of-Stock",
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
            return Ok(model);
        }
        [HttpGet, Route("api/ProductAPI/ReccomendedProduct")]
        public IHttpActionResult ReccomendedProduct()
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
                                   OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                   DiscountPrice = p.DiscountPrice,
                                   ProductDescription = p.ProductDescription,
                                   IsStock = p.IsStock,
                                   IsStocks=p.IsStock==true? "In-Stock" : "Out Of-Stock",
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
            return Ok(model);
        }
        [HttpGet, Route("api/ProductAPI/SpecialProducts")]
        public IHttpActionResult SpecialProducts()
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
                                        OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                        DiscountPrice = p.DiscountPrice,
                                        ProductDescription = p.ProductDescription,
                                        IsStock = p.IsStock,
                                        IsStocks=p.IsStock==true? "In-Stock" : "Out Of-Stock",
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
            return Ok(model);
        }

        //==== GetProductDetail api====


        [HttpGet, Route("api/ProductAPI/GetProductDetail")]
        public IHttpActionResult GetProductDetail(int productId)
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
                                IsStocks = p.IsStock == true ? "In-Stock" : "Out Of-Stock",
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

            return Ok(products);
        }

        //==== GetProduct api====

        [HttpGet, Route("api/ProductAPI/GetProduct")]
        public IHttpActionResult GetProduct()
        {
            try
            {
                var result = from p in ent.Products
                             join c in ent.Categories on p.Category_Id equals c.Id
                             select new Product1()
                             {
                                 Id = p.Id,
                                 ProductName = p.ProductName,
                                 ProductImage = p.ProductImage,
                                 ProductDescription = p.ProductDescription,
                                 Price = p.Price,
                                 IsReviewsAllow = p.IsReviewsAllow,
                                 Quantity = p.Quantity,
                                 OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                 DiscountPrice = p.DiscountPrice,
                                 CategoryName = c.CategoryName,
                                 CategoryImage = c.CategoryImage,
                                 multipleImage = (from s1 in ent.Product_Image where s1.Product_Id == p.Id select s1.ImageName).ToList()
                             };
                if (result != null)
                {
                    return Ok(new { result, status = 200, message = "SubCategory" });
                }
                else
                {
                    return BadRequest("Category Not Available");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        //[HttpGet, Route("api/ProductAPI/GetProduct/{id}")]
        //public IHttpActionResult GetProduct(int id)
        //{
        //    try
        //    {
        //        var result = from p in ent.Products
        //                     join c in ent.Categories on p.Category_Id equals c.Id
        //                     select new Product1()
        //                     {

        //                         Id = p.Id,
        //                         ProductName = p.ProductName,
        //                         ProductImage = p.ProductImage,
        //                         ProductDescription = p.ProductDescription,
        //                         Price = p.Price,
        //                         IsReviewsAllow = p.IsReviewsAllow,
        //                         Quantity = p.Quantity,
        //                         OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
        //                         DiscountPrice = p.DiscountPrice,
        //                         CategoryName = c.CategoryName,
        //                         CategoryImage = c.CategoryImage,
        //                         multipleImage = (from s1 in ent.Product_Image where s1.Product_Id == p.Id select s1.ImageName).ToList()
        //                     };
        //        if (result != null)
        //        {
        //            return Ok(new { result, status = 200, message = "SubCategory" });
        //        }
        //        else
        //        {
        //            return BadRequest("Category Not Available");
        //        }
        //    }
        //    catch
        //    {
        //        return BadRequest("Server Error");
        //    }
        //}



        //==== GetProduct/{id} api====

        [HttpGet, Route("api/ProductAPI/GetProduct/{id}")]
        public IHttpActionResult GetProduct(int categoryId, int? page)
        {
            var model = new ProductDisplayModel();
            page = page ?? 1;
            int pageSize = 20;
            decimal noOfPages = Math.Ceiling((decimal)2 / pageSize);
            var products = (from p in ent.Products
                            join c in ent.Categories on p.Category_Id equals c.Id
                            join m in ent.Metrics on p.Metric_Id equals m.MetricCode
                            where p.Category_Id == categoryId
                            select new ProductModel
                            {
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                CategoryName = c.CategoryName,
                                Metrics = m.Metrics,
                                Id = p.Id,
                                Price = p.Price,
                                OurPrice = p.Price - ((p.Price * p.DiscountPrice) / 100),
                                DiscountPrice = p.DiscountPrice,
                                ProductDescription = p.ProductDescription,
                                IsStock = p.IsStock,
                                IsStocks = p.IsStock == true ? "In-Stock" : "Out Of-Stock",
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
            return Ok(model);

        }


        //====Blog api====
        [HttpGet, Route("api/ProductAPI/Blog")]
        public IHttpActionResult Blog()
        {
            try
            {
                var result = ent.BlogMasters.ToList();
                if (result != null)
                {
                    return Ok(new { result, status = 200, message = "Blog" });
                }
                else
                {
                    return BadRequest("No Any Blog");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        //====CouponList api====
        [HttpGet, Route("api/ProductAPI/CouponList")]
        public IHttpActionResult CouponList()
        {
            try
            {
                // var result = (from c in ent.Discount_Coupon select c.CouponCode).ToList();
                var result = ent.Discount_Coupon.ToList().Select(x => new { x.id, x.CouponCode, x.Amount, x.Name, x.MinimumAmount, x.MaximumAmount });
                if (result != null)
                {
                    return Ok(new { result, status = 200, message = "Coupon List" });
                }
                else
                {
                    return BadRequest("No Coupon Available");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

    }
}
