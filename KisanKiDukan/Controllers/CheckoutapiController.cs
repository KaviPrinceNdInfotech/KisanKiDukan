using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KisanKiDukan.Models.DTO;

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
    }
}
