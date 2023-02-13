using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KisanKiDukan.Controllers
{
    public class MemberShipController : ApiController
    {
        DbEntities ent = new DbEntities();
        [HttpGet, Route("api/MemberShip/GetMembership")]
        public IHttpActionResult GetMembership()
        {

            var model = new MembershipMessageModel();
            //int per = 0;
            var data = (from mb in ent.Memberships
                        select new MemberShipModel
                        {
                            Id = mb.Id,
                            Amount = mb.Amount,
                            Percentage = mb.Percentage
                        }).ToList();
            if (data.Count > 0)
            {
                model.MembCategory = data;
                model.Message = "Sucess";
                model.Status = "1";
                return Ok(model);
            }

            model.Message = "No Membership";
            model.Status = "0";
            return Ok(model);
        }


        [HttpPost, Route("api/MemberShip/JoinMember")]
        public IHttpActionResult JoinMember(CustomerDTO model)
        {
            JoinMembReturnModel ud = new JoinMembReturnModel();


            if (ent.Customers.Any(a => a.Email_Id == model.Email_Id))
            {
                ud.Message = "Emailid is already exists.";
                ud.Status = "0";
                return Ok(ud);
            }
            try
            {
                // Add joined customer data into customer table
                var customer = Mapper.Map<Customer>(model);
                ent.Customers.Add(customer);
                ent.SaveChanges();


                var RefMember = new RefferalMember();
                var wlt = new Wallet();

                //add refferal member user id into RefferalMember Table as reference key 
                RefMember.Refer_Id = model.Refer_Id;
                //add joined member login id into RefferalMember Table as reference key 
                RefMember.Login_Id = customer.User_Id;

                RefMember.CreateDate = System.DateTime.Now;
                ent.RefferalMembers.Add(RefMember);
                wlt.User_Id = RefMember.Login_Id;
                wlt.Mebr_Amount = model.Wallet_Amount;
                ent.Wallets.Add(wlt);
                ent.SaveChanges();
                ud.Message = "success";
                ud.Status = "1";
                return Ok(ud);

            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/MemberShip/MemberDetail")]
        public IHttpActionResult MemberDetail(int id)
        {
            var model = new MemberDetailAPIListModel();
            var data = (from rem in ent.RefferalMembers
                        join cu in ent.Customers on rem.Login_Id equals cu.User_Id
                        join wlt in ent.Wallets on rem.Login_Id equals wlt.User_Id
                        where rem.Refer_Id == id
                        orderby rem.Id descending
                        select new MemberDetailAPIModel
                        {
                            Id = rem.Id,
                            //Refer_Id = rem.Refer_Id,
                            User_Id = rem.Login_Id,
                            FullName = cu.FullName,
                            Email_Id = cu.Email_Id,
                            Address = cu.Address,
                            Phone = cu.Phone,
                            //Wallet_Amount = wlt.Mebr_Amount,
                            AddingDate = rem.CreateDate
                        }).ToList();
            if (data.Count > 0)
            {
                model.Members = data;
                model.Message = "success";
                model.Status = "1";
                return Ok(model);
            }
            model.Message = "No member found";
            model.Status = "0";
            return Ok(model);
        }
    }
}
