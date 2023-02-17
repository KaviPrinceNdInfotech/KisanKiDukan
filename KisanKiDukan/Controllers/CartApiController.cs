using KisanKiDukan.Models.APIModels.RequstModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.NewRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KisanKiDukan.Controllers
{
    public class CartApiController : ApiController
    {
        DbEntities ent = new DbEntities();
        KartOperation kartOp = new KartOperation();


        [HttpPost, Route("api/CartApi/AddToCart")]
        public IHttpActionResult AddToCart(AddToCartReq model)
        {
            var rm = new AddToCartReturnModel();
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ",
ModelState.Values
.SelectMany(a => a.Errors)
.Select(a => a.ErrorMessage));
                rm.Status = 0;
                rm.Message = message;
                return Ok(rm);
            }

            try
            {
                if (!ent.Customers.Any(a => a.User_Id == model.User_Id))
                {
                    rm.Status = 0;
                    rm.Message = "Inavalid User_Id";
                    return Ok(rm);
                }
                var kart = ent.Karts.FirstOrDefault(a => a.Client_Id == model.User_Id);
                if (kart == null)
                {
                    kart = new Kart
                    {
                        Client_Id = model.User_Id
                    };
                    ent.Karts.Add(kart);
                    ent.SaveChanges();

                    var newItem = new KartDetail
                    {
                        Kart_Id = kart.Id,
                        Metric_Id = model.Metric_Id,
                        Quantity = 1,
                        Volume = model.Weight,
                        Product_Id = model.Product_Id,
                        VendorId = model.VendorId
                    };
                    ent.KartDetails.Add(newItem);
                }
                else
                {
                    if (ent.KartDetails.Any(a => a.Kart_Id == kart.Id))
                    {
                        var item = ent.KartDetails.FirstOrDefault(a => a.Kart_Id == kart.Id && a.Product_Id == model.Product_Id && a.Volume == model.Weight && a.Metric_Id == model.Metric_Id);
                        if (item != null)
                        {
                            item.Quantity++;
                        }
                        else
                        {

                            var newItem = new KartDetail
                            {
                                Kart_Id = kart.Id,
                                Metric_Id = model.Metric_Id,
                                Quantity = 1,
                                Volume = model.Weight,
                                Product_Id = model.Product_Id,
                                VendorId = model.VendorId
                            };
                            ent.KartDetails.Add(newItem);
                        }
                    }
                    else
                    {
                        var newItem = new KartDetail
                        {
                            Kart_Id = kart.Id,
                            Metric_Id = model.Metric_Id,
                            Quantity = 1,
                            Volume = model.Weight,
                            Product_Id = model.Product_Id,
                            VendorId = model.VendorId
                        };
                        ent.KartDetails.Add(newItem);
                    }
                }
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Item added to cart successfully";
                rm.Kart = kartOp.GetCart(model.User_Id);
            }
            catch (Exception ex)
            {
                rm.Status = 0;
                rm.Message = "Server exception";
            }
            return Ok(rm);
        }

        [HttpGet, Route("api/CartApi/GetCart")]
        public IHttpActionResult GetCart(int userId)
        {
            var cart = kartOp.GetCart(userId);
            return Ok(cart);
        }

        [HttpGet, Route("api/CartApi/RemoveItemFromCart")]
        public IHttpActionResult RemoveItemFromCart(int cartDetail_Id)
        {
            var rm = new AddToCartReturnModel();
            try
            {
                var item = ent.KartDetails.Find(cartDetail_Id);
                if (item != null)
                {
                    var kart = ent.Karts.Find(item.Kart_Id);
                    int client_id = Convert.ToInt32(kart.Client_Id);
                    ent.KartDetails.Remove(item);
                    ent.SaveChanges();
                    if (!ent.KartDetails.Any(a => a.Kart_Id == kart.Id))
                    {
                        ent.Karts.Remove(kart);
                        ent.SaveChanges();
                    }
                    rm.Status = 1;
                    rm.Message = "Item removed successfully";
                    rm.Kart = kartOp.GetCart(client_id);
                }
                else
                {
                    rm.Status = 0;
                    rm.Message = "Item does not found in cart";
                }
            }
            catch (Exception ex)
            {
                rm.Status = 0;
                rm.Message = "Server Error";
            }
            return Ok(rm);
        }

        [HttpPost, Route("api/CartApi/UpdateQuantity")]
        public IHttpActionResult UpdateQuantity(UpdateCarReq model)
        {
            var rm = new AddToCartReturnModel();
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ",
ModelState.Values
.SelectMany(a => a.Errors)
.Select(a => a.ErrorMessage));
                rm.Status = 0;
                rm.Message = message;
                return Ok(rm);
            }

            try
            {
                var types = new string[] { "inc", "dec" };
                if (!types.Contains(model.Type))
                {
                    rm.Status = 0;
                    rm.Message = "Invalid type";
                    return Ok(rm);
                }
                if (!ent.Customers.Any(a => a.User_Id == model.User_Id))
                {
                    rm.Status = 0;
                    rm.Message = "Inavalid User_Id";
                    return Ok(rm);
                }
                var kart = ent.Karts.FirstOrDefault(a => a.Client_Id == model.User_Id);
                if (kart == null)
                {
                    rm.Status = 0;
                    rm.Message = "Kart does not exist";
                    return Ok(rm);
                }
                var kartDetail = ent.KartDetails.FirstOrDefault(
                                 a => a.Kart_Id == kart.Id
                                 && a.Product_Id == model.Product_Id
                                 && a.Metric_Id == model.Metric_Id
                                 && a.Volume == model.Weight);
                if (kartDetail == null)
                {
                    rm.Status = 0;
                    rm.Message = "Kart does not contain such item.";
                    return Ok(rm);
                }
                if (model.Type == "inc")
                {
                    kartDetail.Quantity++;
                }
                else
                {
                    if (kartDetail.Quantity > 1)
                        kartDetail.Quantity--;
                    else
                        ent.KartDetails.Remove(kartDetail);
                }
                ent.SaveChanges();
                if (!ent.KartDetails.Any(a => a.Kart_Id == kart.Id))
                {
                    ent.Karts.Remove(kart);
                    ent.SaveChanges();
                }
                rm.Status = 1;
                rm.Message = "Item's quantity has updated successfully";
                rm.Kart = kartOp.GetCart(Convert.ToInt32(kart.Client_Id));
            }
            catch (Exception ex)
            {
                rm.Status = 0;
                rm.Message = "Server exception";
            }
            return Ok(rm);
        }
    }
}
