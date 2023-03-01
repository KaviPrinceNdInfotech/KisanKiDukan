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

        //[HttpPost, Route("api/Order/Orders/{sr}")]
        //public IHttpActionResult Orders(int sr)
        //{
        //    try
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            bool isvalid = ent.Orders(sr);
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
    }
}
