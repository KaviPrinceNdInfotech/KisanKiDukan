using KisanKiDukan.Models.APIModels;
using KisanKiDukan.Models.APIModels.RequstModels;
using KisanKiDukan.Models.APIModels.ResponseModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Utilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;



namespace KisanKiDukan.Controllers
{
    public class CustomerController : ApiController
    {
        private DbEntities ent = new DbEntities();
        RerturnModel rm = new RerturnModel();
        [HttpPost]
        [Route("api/Customer/SignupCustomer")]
        public IHttpActionResult SignupCustomer(Customer model)
        {
            var walt = new Wallet();
            SignupCustomerReturnModel ud = new SignupCustomerReturnModel();
            if (!ModelState.IsValid)
            {
                ud.Message = "Please fill all the necessary fields";
                return Ok(ud);
            }
            if (ent.Customers.Any(a => a.Email_Id == model.Email_Id))
            {
                ud.Message = "This email id  already exists.";
                ud.Status = "0";
                return Ok(ud);
            }
            try
            {
                model.IsPremiumMember = false;
                model.PremiumMemberOn = DateTime.Now;
                ent.Customers.Add(model);
                ent.SaveChanges();
                //ud.Message = "Success";
                if (model != null)
                {
                    var Usd = ent.Customers.Where(a => a.User_Id == model.User_Id).FirstOrDefault();
                    ud.User_Id = Usd.User_Id;
                    ud.FullName = Usd.FullName;
                    ud.Phone = Usd.Phone;
                    ud.Password = Usd.Password;
                    ud.Email_Id = Usd.Email_Id;
                    walt.User_Id = Usd.User_Id;
                    ent.Wallets.Add(walt);
                    ent.SaveChanges();
                    ud.Message = "success";
                    ud.Status = "1";
                    return Ok(ud);
                }
                ud.Message = "Not success";
                ud.Status = "0";
                return Ok(ud);
            }
            catch (Exception ex)
            {
                //ud.Message = ex.Message;
                return InternalServerError(ex);
            }

        }

        public string GetToken()
        {

            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "https://localhost:44366";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("Id", "1"));
            permClaims.Add(new Claim("userid", "1"));
            permClaims.Add(new Claim("name", "xyz"));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddYears(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;

        }

        [HttpPost, Route("api/Customer/Registration")]
        public IHttpActionResult Registration(Customer model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new
                    {
                        Status = 400,
                        model.FullName,
                        model.Password,
                        model.Phone,
                        model.Email_Id,
                        Message = "Properties Name Are Must be Same"
                    });
                }
                var EmailIsValid = ent.Customers.Any(x => x.Email_Id == model.Email_Id);
                var MobileValid = ent.Customers.Any(x => x.Phone == model.Phone);
                if (MobileValid == true)
                {
                    return Ok(new { Status = 400, Message = "Mobile No Already Exist" });
                }
                if (EmailIsValid == true)
                {
                    return Ok(new { Status = 400, Message = "Email Id Already Exist" });
                }
                else
                {
                    ent.Customers.Add(model);
                    ent.SaveChanges();
                    var result = ent.Customers.FirstOrDefault(x => x.Phone == model.Phone);
                    if (result != null)
                    {
                        int otpValue = new Random().Next(1000, 9999);
                        result.otp = otpValue;
                        ent.SaveChanges();
                        string mobile = "918601703418";
                        string msg = "Dear User, \n";
                        msg += "Your OTP for login to Gyros is " + otpValue + ". Valid for 30 minutes. Please do not share this OTP.\n";
                        msg += "Regards,\n";
                        msg += "Gyros Team";
                        string dltid = "1207166254332687769";
                        bool isvalid = SMSMessage.SendSms(model.Phone, msg, dltid);
                        if (isvalid)
                        {
                            return Ok("Otp Send SuccessFully");
                        }
                        else
                        {
                            return BadRequest("Otp Not Send");
                        }
                    }
                    return Ok("data not found");
                }
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }


        [HttpPost, Route("api/Customer/OtpVerifywithRegistration")]
        public IHttpActionResult OtpVerifywithRegistration(MobileLogin model)
        {
            try
            {
                var result = ent.Customers.FirstOrDefault(x => x.otp == model.Otp);
                if (result != null)
                {
                    result.Isverify = true;
                    if (result != null)
                    {
                        result.otp = 0;
                    }
                    ent.SaveChanges();
                    return Ok("Registration Successfully");
                }
                return BadRequest("Otp Invalid");
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }
        [HttpPost]
        [Route("api/Customer/LoginMobileOrEmail")]
        public IHttpActionResult LoginMobileOrEmail(MobileLogin model)
        {
            try
            {
                
                var result = ent.Customers.FirstOrDefault(x => x.Phone == model.MobileOrEmail);
                if (result != null)
                {
                    int otpValue = new Random().Next(1000, 9999);
                    result.otp = otpValue;
                    ent.SaveChanges();

                    string mobile = "918601703418";
                    string msg = "Dear User, \n";
                    msg += "Your OTP for login to Gyros is " + otpValue + ". Valid for 30 minutes. Please do not share this OTP.\n";
                    msg += "Regards,\n";
                    msg += "Gyros Team";
                    string dltid = "1207166254332687769";
                    bool isvalid = SMSMessage.SendSms(model.MobileOrEmail, msg, dltid);
                    if (isvalid)
                    {
                        return Ok(new { status = 200, message = "Otp Send SuccessFully" });
                    }
                    else
                    {
                        return BadRequest("Otp Not Send");
                    }

                }
                else
                {
                    var emailresult = ent.Customers.FirstOrDefault(x => x.Email_Id == model.MobileOrEmail);
                    int otpValue = new Random().Next(1000, 9999);
                    model.Otp = otpValue;
                    ent.SaveChanges();
                    string msg = "" + otpValue + " Is the code that will unloack health and nutrition.\nValid for 30 minutes. \nPlease do not share this OTP." + "<br/>" + "Regards\n" + "<br/>" + "Gyros Team";
                    msg += "";
                    msg = "" + otpValue + " Is the code that will unloack health and nutrition";
                    bool Isvalid = MessageEmail.SendEmail(model.MobileOrEmail, msg);

                    if (Isvalid)
                    {
                        string msg1 = "<span style='color:black;'>Welcome to this ever growing family -</span> " + "<b style='color:green;'>GYROS</b> " + " " + "<br/>" + "";
                        msg1 += "<span style='color:black;'>We are delighted to be your trusted choice. </span>" + "<br/>" + "";
                        msg1 += "<b style='color:green;'> Gyros is a family, it’s not a one way conversation. </b><span style='color:black;'> You, our customer, are as important a part of this journey as anything else. </span>" + "<br/>" + " ";
                        msg1 += "<span style='color:black;'>Let’s communicate, talk, solve problems together, and build a community that is so deeply committed to giving back to the farmers who give us these great products. </span>" + "<br/>" + "";
                        msg1 += "<span style='color:black;'>We support farmer communities, their children’s education and give them the best price and all support they require. </span>" + "<br/>" + "";
                        msg1 += "<span style='color:black;'>How do we do it? </span>" + "<br/>" + "";
                        msg1 += "<span style='color:black;'>The answer is simple -</span>" + "<b style='color:green;'> YOU MAKE IT POSSIBLE.</b>" + " " + "<br/>" + "";
                        msg1 += "<span style='color:black'>What you think is just </span>" + "<b style='color:green;'>a  simple purchase of an oil ,  </b>" + " <span style='color:black;'> sets in motion a long chain of effects that reach the </span>" + "<b style='color:green;'> most marginalized farmer communities.</b>" + " " + "<br>" + " ";
                        msg1 += " " + "<b style='color:green;'>You should be proud of your contribution. </b>" + " " + "<br>" + " ";
                        msg1 += "<span style='color:black;'>Write to us, ask questions if you have any, and give us another chance to be a part of your life.In return you get a chance to </span>" + "<b style='color:green;'>change another life. </b>" + " " + "<br/>" + " ";
                        msg1 += "<span style='color:black;'>See you soon.</span>" + "<br/>" + " ";
                        msg1 += "<span style='color:black;'>Health and nutrition on the way. </span>" + "<br/>" + " ";
                        msg1 += "<span style='color:black;'>Gyros family.</span>";
                        MessageEmail.SendEmail(model.MobileOrEmail, msg1);
                        return Ok(new { status = 200, message = "Otp Send SuccessFully" });
                    }
                }


                return BadRequest("Otp Not Send");




            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpPost, Route("api/Customer/MobileOrEmailOtpVerify")]
        public IHttpActionResult MobileOrEmailOtpVerify(MobileLogin model)
        {
            try
            {
                var result = ent.Customers.FirstOrDefault(x => x.otp == model.Otp);
                if (result != null)
                {
                    result.Isverify = true;
                    if (result != null)
                    {
                        result.otp = 0;
                    }
                    ent.SaveChanges();
                    return Ok("login Successfully");
                }
                return BadRequest("Otp Invalid");
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }


        [HttpPost, Route("api/Customer/LoginWithEmail")]
        [AllowAnonymous]
        public IHttpActionResult LoginWithEmail(LoginDTOModel model)
        {
            try
            {

                bool isvalid = ent.Customers.Any(x => x.Email_Id == model.Email_Id && x.Password == model.Password);
                var result = ent.Customers.FirstOrDefault(x => x.Email_Id == model.Email_Id && x.Password == model.Password);
                if (isvalid)
                {
                    var token = GetToken();
                    //if (token != null)
                    //{
                    //    result.Token = token;
                    //}                   
                    //ent.SaveChanges();
                    return Ok("Login Successfully");

                }
                else
                {
                    return BadRequest("Email And PassWord Invalid");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }


        [HttpGet, Route("api/Customer/UserProfile")]
        public IHttpActionResult UserProfile(string email)
        {
            try
            {
                var data = ent.Customers.Where(a => a.Email_Id == email).FirstOrDefault();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Route("api/Customer/ChangePWD")]
        public IHttpActionResult ChangePWD(ChangePWDReturnModel log)
        {
            var frm = new ChangePWDReturnModel();
            try
            {
                var data = ent.Customers.Where(a => a.Email_Id == log.Email_Id && a.Password == log.Password).FirstOrDefault();
                if (data != null)
                {
                    data.Password = log.NewPassword;
                    ent.SaveChanges();
                    frm.Message = "Change success";
                    frm.Status = "1";
                    return Ok(frm);
                }
                frm.Message = "Not success";
                frm.Status = "0";
                return Ok(frm);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("api/Customer/ChangeProfile")]
        public IHttpActionResult ChangeProfile(Customer log)
        {
            var frm = new ChangeProfileReturnModel();
            try
            {

                var data = ent.Customers.Where(a => a.Email_Id == log.Email_Id).FirstOrDefault();
                if (data != null)
                {

                    data.Phone = log.Phone;
                    data.Address = log.Address;
                    data.City = log.City;
                    data.State = log.State;
                    data.Pincode = log.Pincode;
                    ent.SaveChanges();
                    frm.Message = "Change success";
                    frm.Status = "1";
                    return Ok(frm);
                }
                frm.Message = "Not success";
                frm.Status = "0";
                return Ok(frm);

            }
            catch (Exception ex)
            {
                // frm.Message = ex.Message;
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Customer/ForgotPWD")]
        public IHttpActionResult ForgotPWD(ForgotReturnModel ud)
        {
            var frm = new ForgotReturnModel();
            try
            {

                var data = ent.Customers.Where(p => p.Email_Id == ud.Email_Id).FirstOrDefault();
                if (data == null)
                {
                    frm.Message = "Invalid Email";
                    frm.Status = "0";
                    return Ok(frm);
                }
                data.Password = ud.NewPassword;
                ent.SaveChanges();
                frm.Message = "success";
                frm.Status = "1";
                return Ok(frm);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("api/Customer/GetUserByEmail")]
        public IHttpActionResult GetUserByEmail(ForgotReturnUserModel ud)
        {
            var frm = new ForgotReturnUserModel();
            try
            {

                var data = ent.Customers.Where(p => p.Email_Id == ud.Email_Id).FirstOrDefault();
                if (data == null)
                {
                    frm.Message = "Invalid Email";
                    frm.Status = "0";
                    return Ok(frm);
                }
                frm.Message = "success";
                frm.Status = "1";
                frm.User_Detail = data;
                return Ok(frm);
            }
            catch (Exception ex)
            {

                //frm.Message = ex.Message;
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Customer/GetAmountBy")]
        public IHttpActionResult GetAmountBy(int id)
        {
            var model = new MembAmountReturnModel();
            var data = ent.Wallets.Where(a => a.User_Id == id).FirstOrDefault();
            if (data != null)
            {
                var pr = data.Mebr_Amount;
                model.Mebr_Amount = Convert.ToDouble(pr);
                model.Message = "success";
                model.Status = "1";
                return Ok(model);
            }
            model.Message = "Not Found";
            model.Status = "0";
            return Ok(model);
        }


        [HttpPost, Route("api/Customer/AddWalletAmount")]
        public IHttpActionResult AddWalletAmount(WalletAmountUpdateModel wap)
        {
            var model = new WalletAmountUpdateModel();
            var data = ent.Wallets.Where(a => a.User_Id == wap.User_Id).FirstOrDefault();
            try
            {
                if (data == null)
                {
                    model.Message = "User Not Exist";
                    model.Status = "0";
                    return Ok(model);
                }
                else
                {
                    /*double Amount = data.Mebr_Amount*/
                    ;
                    data.Mebr_Amount = wap.Mebr_Amount;
                    ent.SaveChanges();
                    model.User_Id = data.User_Id;
                    model.Mebr_Amount = data.Mebr_Amount;
                    model.Message = "success";
                    model.Status = "1";
                    return Ok(model);

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet, Route("api/Customer/GetPremiumAmt")]
        public IHttpActionResult GetPremiumAmt()
        {
            string query = @"select Top 1 Amount from premiumMembershipAmount order by Id desc";
            var data = ent.Database.SqlQuery<PremiumAmount>(query).FirstOrDefault();
            return Ok(data);
        }

        [HttpPost, Route("api/Customer/PremiumUser")]
        public IHttpActionResult PremiumUser(PremiumAmountModel model)
        {
            if (model.UserId != 0)
            {
                string query = @"update Customer set IsPremiumMember = 1 where User_Id=" + model.UserId;
                ent.Database.ExecuteSqlCommand(query);
                rm.Message = "Success";
                rm.Status = 1;
            }
            else
            {
                rm.Message = "Failed";
                rm.Status = 0;
            }
            return Ok(rm);
        }

        [HttpPost, Route("api/Customer/Bulkorder")]
        public IHttpActionResult Bulkorder(Bulkorder model)
        {
            try
            {
                Bulkorder emp = new Bulkorder()
                {
                    Fullname = model.Fullname,
                    CompanyName = model.CompanyName,
                    Phone = model.Phone,
                    Email = model.Email,    
                    Address = model.Address,    
                    City = model.City,
                    State = model.State,
                    Pincode = model.Pincode,
                    Message=model.Message,

                };
                ent.Bulkorders.Add(emp);
                ent.SaveChanges();
                return Ok("Add SuccessFully");
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }
    }
}
