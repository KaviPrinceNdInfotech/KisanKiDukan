using KisanKiDukan.Utilities;
using KisanKiDukan.Models.APIModels.RequstModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.NewRepository;
using KisanKiDukan.Repository.Implementation;
using KisanKiDukan.Repository.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace KisanKiDukan.Controllers
{
    public class CheckoutController : Controller
    {
        DbEntities ctx = new DbEntities();
        IPaymentTypeRepository PTRepo = new PaymentTypeRepository();
        IOrderDetailRepository ODRepo = new OrderDetailRepository();
        KartOperation kartOp = new KartOperation();

        public ActionResult CreateOrder()
        {
            Cust_OrderInfo COI = new Cust_OrderInfo();
            if (Request.Cookies["user"] != null)
            {
                var userName = Request.Cookies["user"].Value;
                var user = ctx.Customers.FirstOrDefault(a => a.Email_Id == userName || a.Password == userName);
                COI.Client_Id = user.User_Id;
                COI.Name = user.FullName;
                COI.Email = user.Email_Id;
                COI.PhoneNumber = user.Phone;
                COI.paymentMode = new SelectList(PTRepo.GetAllPaymentTypes(), "Id", "PaymentMode");
                return View(COI);
            }
            else
            {
                COI.paymentMode = new SelectList(PTRepo.GetAllPaymentTypes(), "Id", "PaymentMode");
                return View(COI);
            }
        }

        [HttpPost]
        public ActionResult CreateOrder(Cust_OrderInfo customerInfo)
        {
            if (customerInfo.PinCode != null)
            {
                var pin = ctx.DeliveryPincodes.Where(a => a.Pincode == customerInfo.PinCode).FirstOrDefault();
                if (pin == null)
                {
                    var pinc = new DeliveryPincode
                    {
                        City = customerInfo.City,
                        Pincode = customerInfo.PinCode,
                        Location = customerInfo.Address
                    };
                    ctx.DeliveryPincodes.Add(pinc);
                    ctx.SaveChanges();
                }
            }

            if (!ModelState.IsValid)
            {
                customerInfo.paymentMode = new SelectList(PTRepo.GetAllPaymentTypes(), "Id", "PaymentMode");
                return View(customerInfo);
            }
            if (!ctx.DeliveryPincodes.Any(a => a.Pincode == customerInfo.PinCode))
            {
                customerInfo.paymentMode = new SelectList(PTRepo.GetAllPaymentTypes(), "Id", "PaymentMode");
                TempData["msg"] = "Sorry! delivery not avaliable for this pincode '" + customerInfo.PinCode + "'";
                return View(customerInfo);
            }

            using (var trans = ctx.Database.BeginTransaction())
            {
                int OrdId = 0;
                try
                {
                    int ClientId = 0;
                    var model = new AllKartDetailModel();
                    if (Request.Cookies["user"] == null)
                    {
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        var userName = Request.Cookies["user"].Value;
                        var user = ctx.Customers.FirstOrDefault(a => a.Email_Id == userName || a.Password == userName);
                        if (user != null)
                            ClientId = user.User_Id;
                        var kart = ctx.Karts.FirstOrDefault(a => a.Client_Id == ClientId);
                        if (kart == null)
                        {
                            TempData["msg"] = "Kart is empty";
                            return View(model);
                        }
                        int kartId = (kart == null) ? 0 : kart.Id;
                        model = kartOp.GetCart(ClientId);
                        var order = new Order
                        {
                            Name = customerInfo.Name,
                            PhoneNumber = customerInfo.PhoneNumber,
                            Email = customerInfo.Email,
                            Client_Id = ClientId,
                            PinCode = customerInfo.PinCode,
                            Address = customerInfo.Address,
                            City = customerInfo.City,
                            State = customerInfo.State,
                            PaymentStatus = false,
                            PaymentType_Id = customerInfo.paymentType, // 2 for cod
                            PaymentMode = ctx.PaymentTypes.Where(a => a.Id == customerInfo.paymentType).Select(a => a.PaymentMode).FirstOrDefault(),
                            OrderStatus_Id = 1, // 1 for pending
                            OrderDate = DateTime.Now,
                            Total = 0
                        };
                        ctx.Orders.Add(order);
                        ctx.SaveChanges();
                        // saving order detail
                        var orderDetail = new List<OrderDetail>();
                        if (order.Id > 0)
                        {
                            foreach (var item in model.KartDetail)
                            {
                                var od = new OrderDetail
                                {
                                    Order_Id = order.Id,
                                    ProductName = item.ProductName,
                                    Product_Id = item.Product_Id,
                                    User_Id = order.Client_Id,
                                    Price = item.Price,
                                    FinalPrice = (item.Price) * item.Quantity,
                                    Quantity = item.Quantity,
                                    OrderStatus_Id = 1,
                                    Metrics = item.Metric,
                                    Volume = item.Weight,
                                    VendorId = item.VendorId
                                };
                                ctx.OrderDetails.Add(od);
                                orderDetail.Add(od);
                            }
                            // Updating the total of order
                            order.Total = orderDetail.Sum(a => a.FinalPrice);
                            ctx.SaveChanges();
                            ViewBag.TotalAmt = order.Total;
                        }

                        ////Razor Payment
                        string transactionId = order.Id.ToString();
                        Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_qkvWoVAUxLt3M4", "57CC5Xc70R5XKNb5JnDjmVqS");
                        Dictionary<string, object> options = new Dictionary<string, object>();
                        options.Add("amount", order.Total * 100);  // Amount will in paise
                        options.Add("receipt", transactionId);
                        options.Add("currency", "INR");
                        options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                             //options.Add("notes", "-- You can put any notes here --");
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        Razorpay.Api.Order orderResponse = client.Order.Create(options);
                        string orderId = orderResponse["id"].ToString();

                        CheckoutDetail coutDetail = new CheckoutDetail
                        {
                            razorId = orderResponse.Attributes["id"],
                            razorpayKey = "rzp_test_qkvWoVAUxLt3M4",
                            OrderId = order.Id,
                            Total = order.Total * 100,
                            currency = "INR",
                            FirstName = customerInfo.Name,
                            Email = customerInfo.Email,
                            Mobile = customerInfo.PhoneNumber,
                            Address = customerInfo.Address,
                        };
                        if (customerInfo.paymentType == 1)
                        {
                            if (kart != null)
                            {
                                var kartDetail = ctx.KartDetails.Where(a => a.Kart_Id == kart.Id).ToList();
                                foreach (var item in kartDetail)
                                {
                                    ctx.KartDetails.Remove(item);
                                    ctx.SaveChanges();
                                }
                                ctx.Karts.Remove(kart);
                                ctx.SaveChanges();
                            }
                            //send email user to track order detail
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Hi ");
                            sb.Append(order.Name);
                            sb.Append(", we have recieved your order with order no ");
                            sb.Append(order.Id);
                            sb.Append(" and amount of Rs. ");
                            sb.Append(order.Total);
                            sb.Append(". You can track your order status at the desiutpad.in.");
                            string msg = sb.ToString();
                            EmailOperations.SendEmail(order.Email, "Desiutpad order", msg, true);
                            trans.Commit();
                            return RedirectToAction("OrderSucess", new { orderId = order.Id });
                        }
                        else
                        {
                            trans.Commit();
                            // go to payment gateway page
                            return View("PaymentPage", coutDetail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return RedirectToAction("Failure", new { orderId = OrdId });
                }
            }
            //Generate order end
        }
        // private methods
        #region private methods
        private List<KartDetailsDTO> GetKartFromCookie(string cookieValue)
        {
            string serializedKartDetail = Request.Cookies["ProductData"].Value;
            List<KartDetail> kartDetails = new List<KartDetail>();
            if (!string.IsNullOrEmpty(serializedKartDetail))
            {
                kartDetails = JsonConvert.DeserializeObject<List<KartDetail>>(serializedKartDetail).ToList();
            }
            List<KartDetailsDTO> kartItems = new List<KartDetailsDTO>();
            if (kartDetails.Count > 0)
            {
                foreach (var kart in kartDetails)
                {
                    DbEntities context = new DbEntities();
                    var product = context.Products.Find(kart.Product_Id);
                    var cartModel = new KartDetailsDTO
                    {
                        Kart_Id = 0,
                        PID = kart.Product_Id,

                        Image = context.Products.Where(a => a.Id == kart.Product_Id).Select(a => a.ProductImage).FirstOrDefault(),
                        Product = product.ProductName,
                        Price = product.Price ?? 0,
                        Quantity = kart.Quantity == null ? 0 : (int)kart.Quantity,
                        Id = 0,
                    };
                    kartItems.Add(cartModel);
                }
            }
            return kartItems;
        }
        #endregion
        #region RazorPay_Integration
        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url
            var OrderId = Request.Params["OrderId"];
            //var OrderId = Request.Url.Segments.Last();
            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];
            // This is orderId
            string orderId = Request.Params["rzp_orderid"];
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_qkvWoVAUxLt3M4", "57CC5Xc70R5XKNb5JnDjmVqS");
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];
            //// Check payment made successfully
            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("OrderSucess", new { orderId = OrderId });
            }
            else
            {
                return RedirectToAction("Failure", new { orderId = OrderId });
            }
        }

        //----------------------------------------------------
        #endregion RazorPay_Integration
        public ActionResult OrderSucess(int orderId)
        {
            var model = new OrderSuccessViewModel();
            var order = ctx.Orders.Find(orderId);
            //check order details
            var orderDetail = ctx.OrderDetails.Where(a => a.Order_Id == order.Id).ToList();
            model.Id = order.Id;
            model.OrderDate = order.OrderDate;
            var paymentType = ctx.PaymentTypes.Find(order.PaymentType_Id);
            model.PaymentMode = paymentType.PaymentMode;
            model.Total = order.Total;
            model.Order_No = orderId;
            
            // Empty the cart
            if (Request.Cookies["user"] != null)
            {
                var userName = Request.Cookies["user"].Value;
                var user = ctx.Customers.FirstOrDefault(a => a.Email_Id == userName || a.Password == userName);
                int ClientId = 0;
                if (user != null)
                    ClientId = user.User_Id;
                var kart = ctx.Karts.FirstOrDefault(a => a.Client_Id == ClientId);
                if (kart != null)
                {
                    var kartDetail = ctx.KartDetails.Where(a => a.Kart_Id == kart.Id).ToList();

                    foreach (var item in kartDetail)
                    {
                        ctx.KartDetails.Remove(item);
                        ctx.SaveChanges();
                    }
                    ctx.Karts.Remove(kart);
                    ctx.SaveChanges();
                }
            }

            if (Response.Cookies["ProductData"] != null)
            {
                Session.Abandon();
                Response.Cookies.Clear();
                Response.Cookies["ProductData"].Expires = DateTime.Now.AddDays(-1d);
            }

            //updating central store product quantity

            foreach (var item in orderDetail)
            {
                var metric = ctx.Metrics.Where(a => a.Metrics.Equals(item.Metrics)).FirstOrDefault();

                var csData = ctx.Database.SqlQuery<CentralStore>(@"exec getCSExistProductBy @productId = " + item.Product_Id + " , @storeId=0 , @wheight =" + item.Volume + " , @metricCode = " + metric.MetricCode + "").FirstOrDefault();
                if (csData != null && csData.Stock > 0)
                {
                    int lesQnt = csData.Stock - item.Quantity;
                    //ctx.Database.ExecuteSqlCommand(@"update CentralStore set Stock=" + lesQnt + " where id=" + csData.Id + "");
                    csData.Stock = lesQnt;
                    ctx.SaveChanges();
                }
                else
                {
                    //updating  product available status
                    var prodData = ctx.Products.Where(a => a.Id == item.Product_Id && a.Quantity == item.Volume && a.Metric_Id == metric.MetricCode).FirstOrDefault();
                    if (prodData != null)
                    {
                        prodData.IsStock = false;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        //check product varient if main product not found and change available status
                        var prodVariant = ctx.Product_Availability.Where(a => a.Id == item.Product_Id && a.Quantity == item.Volume && a.Metrics_Id == metric.MetricCode).FirstOrDefault();
                        if (prodVariant != null)
                            prodVariant.IsAvailable = false;
                        ctx.SaveChanges();
                    }
                }
            }
            return View(model);
        }

        #region Paynear

        public ActionResult PaynearCheckout()
        {
            var order = (Order)Session["order"];
            PayneerForm model = new PayneerForm();
            model.amount = order.Total;
            model.billingContactName = order.Name;
            model.billingAddress = order.Address;
            model.billingCity = order.City;
            model.billingEmail = order.Email;
            model.billingPhone = order.PhoneNumber;
            model.billingPostalCode = order.PinCode;
            model.billingState = order.State;
            model.description = "Desi Utpad products";
            model.outletId = 0;
            model.apiVersion = "2.0.1";
            model.currencyCode = "INR";
            model.locale = "EN-US";
            model.billingCountry = "IND";
            model.channel = 3;
            model.referenceNo = "OD0" + order.Id;
            model.responseURL = Request.Url.ToString().Substring(0, Request.Url.ToString().Length - Request.Url.PathAndQuery.Length + Url.Content("~").Length) + "Checkout/PaynearResponse";
            return View(model);
        }

        [HttpPost]
        public ActionResult PaynearCheckoutPost()
        {
            List<KeyValuePair<string, string>> inparams = new List<KeyValuePair<string, string>>();
            //Get data from front-end form and binding data to "inparams"
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                inparams.Add(new KeyValuePair<string, string>(Request.Form.Keys[i], Request[Request.Form.Keys[i]]));
            }
            //Calling initiatePayment() methord from PaynearEpay class
            PaynearEpay PaymentService = new PaynearEpay();

            List<KeyValuePair<string, string>> responseMapp = PaymentService.initiatePayment(inparams);

            this.responseMapp = responseMapp;
            //Merchant need check redirectURL and responseCode
            if (responseMapp != null && responseMapp.Find(x => x.Key == "redirectURL").Value != null && "000".Equals(responseMapp.Find(x => x.Key == "responseCode").Value))
            { //Success
                //now customer will redirected to payment page to proceed the payment
                //redirection should be post only
                return Redirect(responseMapp.Find(x => x.Key == "redirectURL").Value);
            }

            else if (responseMapp != null)
            {
                //failed
                //Merchant need to set data to Session for response
                AddDataToSession();
            }
            return RedirectToAction("PaynearFailure");

        }

        List<KeyValuePair<string, string>> responseMapp;
        //Merchant need to create Session method for sorting responseMapp
        private void AddDataToSession()
        {
            Session.Add("amount", responseMapp.Find(x => x.Key == "amount").Value);
            Session.Add("referenceNo", responseMapp.Find(x => x.Key == "referenceNo").Value);
            Session.Add("responseCode", responseMapp.Find(x => x.Key == "responseCode").Value);
            Session.Add("responseMessage", responseMapp.Find(x => x.Key == "responseMessage").Value);
        }


        public ActionResult PaynearResponse()
        {
            //After getting Response from payment page we sorting data in to "inparams"
            List<KeyValuePair<string, string>> inparams = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                inparams.Add(new KeyValuePair<string, string>(Request.Form.Keys[i], Request[Request.Form.Keys[i]]));
            }
            //Here we creating PaynearEpay Class object and calling "getPaymentResponse" method with parameter
            PaynearEpay PaymentService = new PaynearEpay();
            List<KeyValuePair<string, string>> statusResponseMap = PaymentService.getPaymentResponse(inparams);
            //After getreturn "statusResponseMap" from EpayController we checking "statusResponseMap" null or not-null
            PaynearResponseModel resp = new PaynearResponseModel();
            if (statusResponseMap != null)
            {

                // success
                //merchant need to be bind data with Response page
                resp.orderRefNo = statusResponseMap.Find(x => x.Key == "orderRefNo").Value;
                resp.amount = statusResponseMap.Find(x => x.Key == "amount").Value;
                resp.paymentId = statusResponseMap.Find(x => x.Key == "paymentId").Value;
                resp.merchantId = statusResponseMap.Find(x => x.Key == "merchantId").Value;
                resp.transactionId = statusResponseMap.Find(x => x.Key == "transactionId").Value;
                resp.responseMessage = statusResponseMap.Find(x => x.Key == "responseMessage").Value;
                resp.responseCode = statusResponseMap.Find(x => x.Key == "responseCode").Value;
                resp.transactionType = statusResponseMap.Find(x => x.Key == "transactionType").Value;
                resp.transactionDate = statusResponseMap.Find(x => x.Key == "transactionDate").Value;
                resp.currencyCode = statusResponseMap.Find(x => x.Key == "currencyCode").Value;
                resp.paymentMethod = statusResponseMap.Find(x => x.Key == "paymentMethod").Value;
                resp.paymentBrand = statusResponseMap.Find(x => x.Key == "paymentBrand").Value;
                string sOrderId = resp.orderRefNo.Substring(3);
                int oid = Convert.ToInt32(sOrderId);
                var order = ctx.Orders.Find(oid);
                if (order != null)
                {
                    order.PaymentStatus = true;
                    ctx.SaveChanges();
                }
                if (Request.Cookies["user"] != null)
                {
                    var userName = Request.Cookies["user"].Value;
                    var user = ctx.Customers.FirstOrDefault(a => a.Email_Id == userName || a.Password == userName);
                    int ClientId = 0;
                    if (user != null)
                        ClientId = user.User_Id;
                    var kart = ctx.Karts.FirstOrDefault(a => a.Client_Id == ClientId);
                    if (kart != null)
                    {
                        var kartDetail = ctx.KartDetails.Where(a => a.Kart_Id == kart.Id).ToList();
                        foreach (var item in kartDetail)
                        {
                            ctx.KartDetails.Remove(item);
                            ctx.SaveChanges();
                        }
                        ctx.Karts.Remove(kart);
                        ctx.SaveChanges();
                    }
                }

                if (Response.Cookies["ProductData"] != null)
                {
                    Session.Abandon();
                    Response.Cookies.Clear();
                    Response.Cookies["ProductData"].Expires = DateTime.Now.AddDays(-1d);
                }
            }

            return View(resp);
        }


        public ActionResult PaynearFailure()
        {
            var failure = new PaynearFailure();
            failure.amount = Session["amount"].ToString();
            failure.referenceNo = Session["referenceNo"].ToString();
            failure.responseCode = Session["responseCode"].ToString();
            failure.responseMessage = Session["responseMessage"].ToString();

            if (Response.Cookies["ProductData"] != null)
            {
                Session.Abandon();
                Response.Cookies.Clear();
                Response.Cookies["ProductData"].Expires = DateTime.Now.AddDays(-1d);
            }
            return View(failure);
        }

        #endregion


        public ActionResult Failure(int oId = 0)
        {
            var order = ctx.Orders.Find(oId);
            using (var trans = ctx.Database.BeginTransaction())
            {
                try
                {
                    if (order != null)
                    {
                        //delete record fro order detail table
                        ctx.Database.ExecuteSqlCommand(@"delete from OrderDetail where Order_Id=" + oId);
                        //delete record fro order table
                        ctx.Database.ExecuteSqlCommand(@"delete from [order] where Id=" + oId);
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            return View();
        }

        public ActionResult GenerateInvoice(int orderId)
        {
            var model = new GenerateInvoiceViewModel();
            var order = ctx.Orders.Find(orderId);
            try
            {
                var Data = (from od in ctx.OrderDetails
                            where od.Order_Id == orderId
                            select new OrderDetailModel
                            {
                                ProductName = od.ProductName,
                                Price = od.Price,
                                Quantity = od.Quantity,
                                FinalPrice = od.FinalPrice
                            }).ToList();
                model.OrderId = order.Id;
                model.Adress = order.Address;
                model.city = order.City;
                model.EmailId = order.Email;
                model.ContactNumber = order.PhoneNumber;
                model.FinalPrice = order.Total;
                model.Invoice_No = order.Invoice_No;
                ViewBag.OrderDate = order.OrderDate;
                ViewBag.UserData = "" + order.Name + "\r\n" + order.Address + " " + order.City + "\r\n" + order.State + " " + order.PinCode + "\r\nMo. :" + order.PhoneNumber + "";
                if (Data.Count > 0)
                {
                    model.OrderDetailData = Data;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}