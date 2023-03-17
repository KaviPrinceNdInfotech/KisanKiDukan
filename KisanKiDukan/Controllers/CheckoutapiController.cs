using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using System.Web;
using System.Web.UI;
using System.Web.Http.Results;
using System.Web.Helpers;
using KisanKiDukan.Utilities;
using Razorpay.Api;

namespace KisanKiDukan.Controllers
{
    public class CheckoutapiController : ApiController
    {

        DbEntities ent = new DbEntities();

        [HttpPost, Route("api/Checkoutapi/DeliveryAddress/{sr}")]
        public IHttpActionResult DeliveryAddress(int sr, ShippingAddressModel model)
        {
            try
            {
                ShippingAddress emp = new ShippingAddress()
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    State = model.State,
                    City = model.City,
                    Address = model.Address,
                    Email = model.Email,
                    PinCode = model.PinCode,
                    UserId = sr
                };
                ent.ShippingAddresses.Add(emp);
                ent.SaveChanges();
                return Ok("Address Add SuccessFully");
            }
            catch
            {
                throw new Exception("Server Error");
            }
        }

        [HttpGet, Route("api/Checkoutapi/GetByAddress/{Id}")]
        public IHttpActionResult GetByAddress(int Id)
        {
            try
            {
                var result = ent.ShippingAddresses.Where(x => x.Id == Id).ToList();
                if (result != null)
                {
                    return Ok(new { status = 200, result, message = "Delivery Address" });
                }
                else
                {
                    return BadRequest("No Delevery Address");
                }
            }
            catch
            {
                throw new Exception("Server Error");
            }
        }

        [HttpGet, Route("api/Checkoutapi/ListAddress/{Usr}")]
        public IHttpActionResult ListAddress(int Usr)
        {
            try
            {
                var result = ent.ShippingAddresses.Where(x => x.UserId == Usr).ToList();
                if (result != null)
                {
                    return Ok(new { status = "Delivery Address", result, Status = 200 });
                }
                else
                {
                    return BadRequest("No Address");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpGet, Route("api/Checkoutapi/OrderDetail/{urdt}")]
        public IHttpActionResult OrderDetail(int urdt)
        {
            var model = new CustomerOrderModel();
            var data = (from od in ent.OrderDetails
                        join pd in ent.Products on od.Product_Id equals pd.Id
                        join ord in ent.Orders on od.Order_Id equals ord.Id
                        join os in ent.OrderStatus on od.OrderStatus_Id equals os.Id
                        where od.Order_Id == urdt
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
            model.Total_Price = data.Where(a => a.Order_Id == urdt).Select(a => a.Total_Price).FirstOrDefault();
            return Ok(model);
        }

        [HttpGet, Route("api/Checkoutapi/OrderHistory")]
        public IHttpActionResult OrderHistory(int urdt)
        {
            try
            { 
                var user = ent.Customers.FirstOrDefault(x => x.User_Id == urdt);
                var Adress = ent.ShippingAddresses.FirstOrDefault(x => x.UserId == user.User_Id);
                var result = ent.Orders.Where(x => x.Client_Id ==urdt).Select(x => new CODOrderDTO()
                              {
                                  Id = x.Id,
                                  Phone = x.PhoneNumber,
                                  Total_Price = x.Total,
                                  PaymentMode = x.PaymentMode,
                                  Address = x.Address,
                                  OrderDate = x.OrderDate,
                                  IsCancel = x.IsCancel,
                                  Total_Items = ent.OrderDetails.Where(a => a.Order_Id == x.Id).Count()
                              }).ToList();

                    //return Ok(new { result, message = "Order History", status = 200 });
                if (result != null)
                {
                    return Ok(new { result, message = "Order History", status = 200 });
                }
                else
                {
                    return BadRequest("No Order History");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }


        [HttpGet, Route("api/Checkoutapi/Invoice/{urdt}")]
        public IHttpActionResult Invoice(int urdt)
        {
            try
            {
                var result = ent.Orders.Where(x => x.Client_Id == urdt).Select(x => new CODOrderDTO()
                {
                    Id = x.Id,
                    Phone = x.PhoneNumber,
                    Total_Price = x.Total,
                    PaymentMode = x.PaymentMode,
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    IsCancel = x.IsCancel,
                     Invoice=x.Invoice_No,
                }).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new { result, status = 200, message = "Invoice" });
                }
                else
                {
                    return BadRequest("No Invoice");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpGet, Route("api/Checkoutapi/InvoiceV1/{Invoice}")]
        public IHttpActionResult InvoiceV1(string Invoice)
        {
            try
            {
                var result = ent.Orders.Where(x => x.Invoice_No == Invoice).Select(x => new CODOrderDTO()
                {
                    Id = x.Id,
                    Phone = x.PhoneNumber,
                    Total_Price = x.Total,
                    PaymentMode = x.PaymentMode,
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    IsCancel = x.IsCancel,
                    Invoice = x.Invoice_No,
                }).ToList();
                if (result != null)
                {
                    decimal GrandTotal = 0;
                    foreach (var item in result)
                    {
                        //GrandTotal = GrandTotal + item.
                    }
                    return Ok(new { result, GrandTotal = GrandTotal, status = 200, message = "Invoice" });
                }
                else
                {
                    return BadRequest("No Invoice");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpPost, Route("api/Checkoutapi/Orders/{sr}")]
        //public IHttpActionResult Orders(int sr)
        //{
        //    try
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            //bool isvalid = Orderes(sr);
        //            if (isvalid)
        //            {
        //                return Ok("Order SucessFully");
        //            }
        //            else
        //            {
        //                return BadRequest("Order Not SuccessFully");
        //            }

        //        }
        //        else
        //        {
        //            return BadRequest("Authenticated Failed");
        //        }
        //    }
        //    catch
        //    {
        //        return BadRequest("Server Error");
        //    }
        //}

        //public bool Orderes(int sr)
        //{
        //    try
        //    {
        //        var result = ent.ShippingAddresses.FirstOrDefault(x => x.UserId == sr);
        //        var emaiid = ent.Customers.FirstOrDefault(x => x.User_Id == sr);
        //        string InvoiceNos = InvoiceNo();
        //        Order emp = new Order()
        //        {
                   

        //        };
        //        ent.Reviews.Add(emp);
        //        ent.SaveChanges();
        //        return Ok();
           

        //        string message = "Health is calling.Order with order id " + InvoiceNos + "  is confirmed, Gyros is entering your life soon";
        //        string DltIt = "1207167143986864882";
        //        SMSMessage.SendSms(emaiid.Email_Id, message, DltIt);

        //        string msg = "<span style='color:#000000;font-weight:bold;'>Welcome to this ever growing family  </span>" + "<span style='color: #38761d;font-weight:bold;font-family:arial black,sans-serif'>GYROS FARM.</span>" + "" + "<br/>" + "<br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> We are delighted to be your trusted choice. " + "<span style='color: #38761d;font-weight:bold;font-family:arial black,sans-serif'>GYROS FARM.</span>" + "   is a family, it’s not a one way conversation. You, Our customer, are as important a part of this journey as anything else.</span>" + "<br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> Let’s communicate, talk, solve problems together, and build a community that is so deeply committed to giving back to the farmers who give us these great products.</span>" + "<br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> We support farmer communities, their children’s education and give them the best price and all support they require.</span>" + "<br/>" + "<br/>" + "";
        //        msg += "<span style='color:#38761d;font-weight:bold;font-family:arial,sans-serif'> How do we do it? </span>" + "<br/>" + "<br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> The answer is simple </span> - " + "<span style='color: #6aa84f;font-weight:bold;'>YOU MAKE IT POSSIBLE.</span>" + "" + "<br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> What you think is just a simple purchase of an oil , sets in motion a long chain of effects that reach the most marginalized farmer communities. You should be proud of your contribution. </span> " + " <br/>" + " <br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> Write to us, ask questions if you have any, and give us another chance to be a part of your life.In return you get a chance to change another life.See you soon. </span> " + " <br/>" + " <br/>" + "";
        //        msg += "<span style='color:#000000;font-weight:bold;'> Health and nutrition on the way. </span>" + " <br/>" + " <br/>" + "";
        //        msg += "<span style='color: #6aa84f;font-weight:bold;'>Regards</span>" + " <br/>" + "";
        //        msg += "<span style='color: #38761d;font-weight:bold;font-family:arial black,sans-serif'>GYROS FARM.</span>" + " <br/>" + "";
        //        MessageEmail.SendEmail(emaiid.Email_Id, msg);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        public string InvoiceNo()
        {
            try
            {
                var data = ent.Orders.OrderByDescending(a => a.Id).FirstOrDefault();
                string UserName = "2023BNW1";
                if (data != null)
                {
                    string IncrementValue = data.Invoice_No.Substring(7, data.Invoice_No.Length - 7);
                    int NewVal = Convert.ToInt32(IncrementValue) + 1;
                    return "2023BNW" + NewVal;
                }
                return UserName;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
