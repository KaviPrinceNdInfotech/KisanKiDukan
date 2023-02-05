using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.APIModels.ResponseModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace KisanKiDukan.Controllers
{
    public class VendorApiController : ApiController
    {
        DbEntities ent = new DbEntities();
        MessageModel rm = new MessageModel();
     
        [HttpGet, Route("api/VendorApi/VendorDashboard")]
        public IHttpActionResult VendorDashboard(int VendorId)
        {
            var model = new VendorDashboardModel();
            try
            {
                //Total Revenue
                double sumPrice = 0;
                var totalRevenue = ent.Database.SqlQuery<double>(@"select FinalPrice from OrderDetail where OrderStatus_Id=3 and VendorId=" + VendorId).ToList();
                foreach (var tr in totalRevenue)
                {
                    sumPrice = sumPrice + Convert.ToDouble(tr);
                }
                var countsale = ent.Database.SqlQuery<int>(@"SELECT count(Id) AS total_Sale FROM OrderDetail where OrderStatus_Id between 1 and 3 and VendorId=" + VendorId).FirstOrDefault();
                var orderData = ent.Database.SqlQuery<OrderData>(@" select * from OrderDetail od join [Order] ord on od.Order_Id=ord.Id
                 Where CONVERT(VARCHAR(10), OrderDate, 105)= CONVERT(VARCHAR(10), GETDATE(), 105)
                 and od.VendorId=" + VendorId).ToList();
                if (orderData != null || sumPrice != 0 || countsale != 0)
                {
                    model.OrderDataList = orderData;
                    model.TotalRevenue = sumPrice;
                    model.TotalSale = countsale;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        [HttpPost, Route("api/VendorApi/Add")]
        public IHttpActionResult Add(VendorDTO model)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    if (model.PanCardImageBase64 != null && model.AddressProofImageBase64 != null && model.CancelChequeImageBase64 != null
                        && model.SignedDocumentImageBase64 != null && model.GovtCertificateImageBase64 != null && model.FoodLicenceImageBase64 != null
                        && model.BusinessDocumnetImageBase64 != null)
                    {
                        var PanImage = FileOperation.UploadFileWithBase64("Images", model.PanCard, model.PanCardImageBase64, allowedExtensions);
                        var AddImage = FileOperation.UploadFileWithBase64("Images", model.AddressProof, model.AddressProofImageBase64, allowedExtensions);
                        var ChequeImage = FileOperation.UploadFileWithBase64("Images", model.CancelCheque, model.CancelChequeImageBase64, allowedExtensions);
                        var SignDocImage = FileOperation.UploadFileWithBase64("Images", model.SignedDocument, model.SignedDocumentImageBase64, allowedExtensions);
                        var GovtDocImage = FileOperation.UploadFileWithBase64("Images", model.GovtCertificate, model.GovtCertificateImageBase64, allowedExtensions);
                        var LicenceImage = FileOperation.UploadFileWithBase64("Images", model.FoodLicence, model.FoodLicenceImageBase64, allowedExtensions);
                        var BusinessDocImage = FileOperation.UploadFileWithBase64("Images", model.BusinessDocumnet, model.BusinessDocumnetImageBase64, allowedExtensions);
                        if (PanImage == "not allowed" && AddImage == "not allowed" && ChequeImage == "not allowed" && SignDocImage == "not allowed" && GovtDocImage == "not allowed" && LicenceImage == "not allowed" && BusinessDocImage == "not allowed")
                        {
                            rm.Status = 0;
                            rm.Message = "Only png,jpg,jpeg files are allowed as Licence document";
                            return Ok(rm);
                        }
                        model.PanCard = PanImage;
                        model.AddressProof = AddImage;
                        model.CancelCheque = ChequeImage;
                        model.SignedDocument = SignDocImage;
                        model.GovtCertificate = GovtDocImage;
                        model.FoodLicence = LicenceImage;
                        model.BusinessDocumnet = BusinessDocImage;
                    }
                    var login = new LoginMaster { Username = model.EmailId, Password = model.Password, Role = "Vendor" };
                    ent.LoginMasters.Add(login);
                    ent.SaveChanges();
                    var vendor = Mapper.Map<Vendor>(model);
                    vendor.PanCard = model.PanCard;
                    vendor.AddressProof = model.AddressProof;
                    vendor.CancelCheque = model.CancelCheque;
                    vendor.SignedDocument = model.SignedDocument;
                    vendor.GovtCertificate = model.GovtCertificate;
                    vendor.FoodLicence = model.FoodLicence;
                    vendor.BusinessDocumnet = model.BusinessDocumnet;
                    vendor.RegistrationDate = DateTime.Now;
                    vendor.LoginMaster_Id = login.Id;
                    ent.Vendors.Add(vendor);
                    ent.SaveChanges();
                    tran.Commit();
                    string msg = "Hi " + vendor.VendorName + ", your varnifarm.net login credentials are";
                    msg += " Username: " + vendor.EmailId + " and Password : " + login.Password;
                    SmsOperation.SendSms(vendor.ContactNumber, msg);
                    model.Message = "Record saved successfully.";
                    model.Status = "1";
                    var data = ent.Vendors.Where(a => a.EmailId == model.EmailId && a.Password == model.Password).FirstOrDefault();
                    model.Id = data.Id;
                    return Ok(model);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    model.Message = "Server error.";
                    model.Status = "0";
                    return Ok(model);
                }
            }
        }

        [HttpPost, Route("api/VendorApi/AdminLogin")]
        public IHttpActionResult AdminLogin(LoginMaster ud)
        {
            var model = new LoginModel();
            try
            {
                var data = ent.LoginMasters.Where(a => a.Username == ud.Username && a.Password == ud.Password).FirstOrDefault();
                if (data == null)
                {
                    model.Message = "Invalid username or password";
                    model.Status = "0";
                    return Ok(model);
                }
                var data1 = ent.Vendors.Where(a => a.LoginMaster_Id == data.Id).FirstOrDefault();
                if (data.Role == "Vendor")
                {
                    model.Username = data.Username;
                    model.Password = data.Password;
                    model.VendorId = data.Id;
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/VendorApi/YourProducts")]
        public IHttpActionResult YourProducts(int vendorID)
        {
            var model = new ProductDisplayModelAdmin();
            var data = (from pd in ent.Products
                        join ct in ent.Categories on pd.Category_Id equals ct.Id
                        join sct in ent.SubCategories on pd.SubId equals sct.Id
                        join me in ent.Metrics on pd.Metric_Id equals me.MetricCode
                        where pd.VendorId == vendorID
                        select new ProductModel
                        {
                            subId = pd.SubId,
                            Id = pd.Id,
                            ProductName = pd.ProductName,
                            Category_Id = pd.Category_Id,
                            ProductImage = pd.ProductImage,
                            Metric_Id = pd.Metric_Id,
                            Metrics = me.Metrics,
                            CategoryName = ct.CategoryName,
                            Price = pd.Price,
                            ProductDescription = pd.ProductDescription,
                            Quantity = pd.Quantity,
                            ShippingCharge = pd.ShippingCharge,
                            OurPrice = pd.OurPrice,
                            VendorId = pd.VendorId,
                            PremiumAmount = pd.PremiumAmount
                        }).ToList();
            model.Products = data.OrderBy(a => a.Id);
            if (vendorID > 1)
            {
                if (data != null)
                {
                    return Ok(model);
                }
            }
            return NotFound();
        }

        [HttpGet, Route("api/VendorApi/YourOrders")]
        public IHttpActionResult YourOrders(int vendorID)
        {
            var model = new OrderDetailShowListModelAdmin();
            var data = (from od in ent.OrderDetails
                        join pd in ent.Products on od.Product_Id equals pd.Id
                        join ord in ent.Orders on od.Order_Id equals ord.Id
                        join os in ent.OrderStatus on od.OrderStatus_Id equals os.StatusCode
                        where pd.VendorId == vendorID && od.IsCancel == false
                        select new OrderDetailShowModelAdmin
                        {
                            Id = od.Id,
                            User_Id = od.User_Id,
                            Order_Id = od.Order_Id,
                            Product_Id = od.Product_Id,
                            ProductName = od.ProductName,
                            Product_Price = od.Price,
                            ProductImage = pd.ProductImage,
                            Quantity = od.Quantity,
                            Metrics = od.Metrics,
                            FinalPrice = od.FinalPrice,
                            Total_Price = ord.Total,
                            OrderStatus = os.StatusName,
                            IsCancel = od.IsCancel,
                            Weight = od.Volume,
                            OrderStatus_Id = od.OrderStatus_Id,
                            OrderStatusDate = od.StatusUpdateDate
                        }).ToList();
            model.OrderDetail = data.OrderBy(a => a.Id);
            return Ok(model);
        }

        [HttpGet, Route("api/VendorApi/VendorOrderDetails")]
        public IHttpActionResult VendorOrderDetails(int OrderId, int VendorId)
        {
            IList<OrderDetailDTO> model = null;
            using (ent)
            {
                model = ent.OrderDetails.Where(s => s.Order_Id == OrderId && s.VendorId == VendorId)
                            .Select(s => new OrderDetailDTO()
                            {
                                Id = s.Id,
                                Product_Id = s.Product_Id,
                                Client_Id = s.User_Id,
                                ProductName = s.ProductName,
                                Price = s.Price,
                                Quantity = s.Quantity,
                                IsCancel = s.IsCancel,
                                OrderStatus_Id = s.OrderStatus_Id,
                                VendorId = s.VendorId
                            }).ToList<OrderDetailDTO>();
            }
            if (model.Count == 0)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpGet, Route("api/VendorApi/VendorCancelOrder")]
        public IHttpActionResult VendorCancelOrder(int orderDetailId, int userId, int ProductId, int vendorId)
        {
            var rm = new MessageModel();
            try
            {
                var data = ent.OrderDetails.Where(a => a.Id == orderDetailId && a.Product_Id == ProductId && a.User_Id == userId && a.VendorId == vendorId).FirstOrDefault();
                if (data != null)
                {
                    data.IsCancel = true;
                    data.CancellationDate = DateTime.Now;
                    data.StatusUpdateDate = DateTime.Now;
                    data.OrderStatus_Id = 4; // 4 for cancellation
                    ent.SaveChanges();
                }
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

        [HttpPost, Route("api/VendorApi/AddProduct")]
        public IHttpActionResult AddProduct(ProductModel model)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            using (var trans = ent.Database.BeginTransaction())
            {
                model.PAvail = new List<PAvailDTO> { new PAvailDTO { Id = 0, Product_Id = 0, Quantity = 0, Metrics_Id = 0, Price = 0, OurPrice = 0 } };
                try
                {
                    if (model.ImageBase64 != null)
                    {
                        var ProdImage = FileOperation.UploadFileWithBase64("Images", model.ProductImage, model.ImageBase64, allowedExtensions);

                        if (ProdImage == "not allowed")
                        {
                            rm.Status = 0;
                            rm.Message = "Only png,jpg,jpeg files are allowed as Licence document";
                            return Ok(rm);
                        }
                        model.ProductImage = ProdImage;
                    }
                    model.ProductImage = model.ProductImage;
                    var product = Mapper.Map<Product>(model);
                    product.IsStock = true;
                    product.Price = model.Price;
                    product.OurPrice = model.OurPrice;
                    product.Category_Id = model.Category_Id;
                    product.SubId = model.subId;
                    product.VendorId = model.VendorId;
                    product.SubId = model.subId;
                    if (product.Category_Id != null && product.Price != null && product.SubId != null && product.VendorId != null)
                    {
                        ent.Products.Add(product);
                        ent.SaveChanges();
                        //save product multiple varients
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
                        trans.Commit();
                        rm.Message = "Data Save Successfuly.";
                        rm.Status = 1;
                    }
                    else
                    {
                        rm.Message = "Server error Please login again.";
                        rm.Status = 0;
                    }
                    return Ok(rm);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Ok("Server error.");
                }
            }
        }

        [HttpPost, Route("api/VendorApi/UpdateProduct")]
        public IHttpActionResult UpdateProduct(ProductModel model)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            using (var trans = ent.Database.BeginTransaction())
            {
                var data = ent.Products.Where(a => a.Id == model.Id).FirstOrDefault();
                try
                {
                    if (data != null)
                    {
                        if (model.ImageBase64 != null)
                        {
                            var ProdImage = FileOperation.UploadFileWithBase64("Images", model.ProductImage, model.ImageBase64, allowedExtensions);

                            if (ProdImage == "not allowed")
                            {
                                rm.Status = 0;
                                rm.Message = "Only png,jpg,jpeg files are allowed as Licence document";
                                return Ok(rm);
                            }
                            model.ProductImage = ProdImage;
                        }
                        data.ProductName = model.ProductName;
                        data.ProductImage = model.ProductImage;
                        data.Category_Id = model.Category_Id;
                        data.Price = model.Price;
                        data.ProductDescription = model.ProductDescription;
                        data.Metric_Id = model.Metric_Id;
                        data.IsReplacement = model.IsReplacement;
                        data.Quantity = model.Quantity;
                        data.IsFeatured = model.IsFeatured;
                        data.IsFreeShipping = model.IsFreeShipping;
                        data.ShippingCharge = model.ShippingCharge;
                        data.IsReviewsAllow = model.IsReviewsAllow;
                        data.IsActive = model.IsActive;
                        data.IsVariant = model.IsVariant;
                        data.IsRecomend = model.IsRecomend;
                        data.SubId = model.subId;
                        data.IsHotdeals = model.IsHotdeals;
                        data.IsFeatureProduct = model.IsFeatureProduct;
                        data.IsSpecial = model.IsSpecial;
                        data.IsStock = true;
                        data.VendorId = model.VendorId;
                        data.SubId = model.subId;
                        ent.Entry<Product>(data).State = EntityState.Modified;
                        ent.SaveChanges();
                        trans.Commit();
                        rm.Message = "Data Save Successfuly.";
                        rm.Status = 1;
                    }
                    else
                    {
                        rm.Message = "Data not found.";
                        rm.Status = 0;
                    }
                    return Ok(rm);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Ok("Server error.");
                }
            }
        }

        [HttpGet, Route("api/VendorApi/DeleteProduct")]
        public IHttpActionResult DeleteProduct(int id)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.Products.Find(id);
                    ent.Products.Remove(data);
                    ent.SaveChanges();
                    tran.Commit();
                    rm.Message = "Data deleted successfuly.";
                    rm.Status = 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rm.Message = "Data not deleted try agin.";
                    rm.Status = 0;
                }
                return Ok(rm);
            }
        }

        [HttpGet, Route("api/VendorApi/GetCategory")]
        public IHttpActionResult GetCategory()
        {
            try
            {
                var data = ent.Categories.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok("Server error");
            }
        }

        [HttpGet, Route("api/VendorApi/GetSubCategory")]
        public IHttpActionResult GetSubCategory()
        {
            try
            {
                var data = ent.SubCategories.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok("Server error");
            }
        }

        [HttpGet, Route("api/VendorApi/GetMetrics")]
        public IHttpActionResult GetMetrics()
        {
            try
            {
                var data = ent.Metrics.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok("Server error");
            }
        }
    }
}
