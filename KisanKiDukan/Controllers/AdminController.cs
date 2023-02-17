using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System.Configuration;
using System.Collections;
using System.Net.Mail;
using KisanKiDukan.Models.DTO;
using System.IO;
using AutoMapper;
using System.Data.Entity;
using System.Web.Security;
using System.Data.Entity.SqlServer;


namespace KisanKiDukan.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        DbEntities ent = new DbEntities();
        public ActionResult Dashboard()
        {
            try
            {
                //Total Revenue
                double sumPrice = 0;
                var totalRevenue = ent.Database.SqlQuery<double>(@"select FinalPrice from OrderDetail where OrderStatus_Id=3").ToList();
                foreach (var tr in totalRevenue)
                {
                    sumPrice = sumPrice + Convert.ToDouble(tr);
                }
                ViewBag.TotalRevenue = sumPrice;

                //Total Users
                var totalUsers = ent.Database.SqlQuery<int>(@"select COUNT(User_Id) from Customer").FirstOrDefault();
                ViewBag.TotalUsers = totalUsers;

                //Total No. of Sale
                int countsale = ent.Database.SqlQuery<int>(@"SELECT count(Id) AS total_Sale FROM OrderDetail where OrderStatus_Id between 1 and 3").FirstOrDefault();
                ViewBag.Totalsale = countsale;

                //Monthly Sale            
                var currentDate = DateTime.Now;
                var prevDate = currentDate.AddMonths(-1);
                var startDate = new DateTime(prevDate.Year, prevDate.Month, prevDate.Day);
                var endDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                var monthlySale = ent.Database.SqlQuery<double>(@"select SUM(FinalPrice) from OrderDetail where DeliveryDate between '" + startDate + "' and '" + endDate + "' and OrderStatus_Id between 1 and 3").FirstOrDefault();
                ViewBag.MonthlySale = monthlySale;

                //Weekly Sale
                //var currentDate1 = DateTime.Now;
                var prevDate1 = currentDate.AddDays(-7);
                var weeklySale = ent.Database.SqlQuery<double>(@"select SUM(FinalPrice) from OrderDetail where DeliveryDate between '" + prevDate1 + "' and '" + currentDate + "' and OrderStatus_Id between 1 and 3").FirstOrDefault();
                ViewBag.WeeklySale = weeklySale;

                //Daily Sale   select SUM(FinalPrice) from OrderDetail where DeliveryDate > GETDATE() and OrderStatus_Id between 1 and 3
                var dailySale = ent.Database.SqlQuery<double>(@"select SUM(FinalPrice) from OrderDetail where DeliveryDate >'" + currentDate + "' and OrderStatus_Id between 1 and 3").FirstOrDefault();
                if (dailySale != 0)
                {
                    ViewBag.DailySale = dailySale;
                }
                else
                {
                    ViewBag.DailySale = 0;
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public ContentResult CheckUserAuthentication(LoginModel model)
        {
            try
            {
                var user = ent.Database.SqlQuery<LoginMaster>("select * from LoginMaster where username='" + model.Username + "' and password='" + model.Password + "'").FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    Session["AddBy"] = user.Id;
                    return Content("ok");
                }
                return Content("Invalid username or password");
            }
            catch
            {
                return Content("Server error");
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return Content("Any fields can not be blank.");
            if (model.Password != model.ConfirmPassword)
            {
                return Content("your password and confirm password are not same");
            }
            int id = Convert.ToInt32(User.Identity.Name);
            if (id > 0)
            {
                var record = ent.LoginMasters.Find(id);
                if (record != null)
                {
                    record.Password = model.Password;
                    ent.SaveChanges();
                    return Content("ok");
                }
            }
            return Content("Some error has occured.");
        }

        public ActionResult Orders(int? page, string term, string oid, DateTime? from, DateTime? to, int statusCode = 0)
        {
            var model = new OrderShowListModelAdmin();
            model.OrderStatusList = new SelectList(ent.OrderStatus.ToList(), "StatusCode", "StatusName");
            var data = (from ord in ent.Orders
                        join cu in ent.Customers on ord.Client_Id equals cu.User_Id
                        join os in ent.OrderStatus on ord.OrderStatus_Id equals os.StatusCode
                        select new OrderShowModelAdmin
                        {
                            Id = ord.Id,
                            StatusUpdateDate = ord.StatusUpdateDate,
                            Client_Id = ord.Client_Id,
                            Name = cu.FullName,
                            PhoneNumber = cu.Phone,
                            Total_Price = ord.Total,
                            Email = cu.Email_Id,
                            PaymentMode = ord.PaymentMode,
                            PaymentStatus = ord.PaymentStatus,
                            Address = ord.Address,
                            OrderDate = ord.OrderDate,
                            DeliveryDate = ord.DeliveryDate,
                            OrderStatus_Id = ord.OrderStatus_Id,
                            IsCancel = ord.IsCancel,
                            City = ord.City,
                            State = ord.State,
                            PinCode = ord.PinCode,
                            Total_Items = ent.OrderDetails.Where(a => a.Order_Id == ord.Id).Count(),
                            OrderStatus = os.StatusName,
                            ScheduledDeliveryDate = ord.ScheduleDate,
                            SlotCode = ord.SlotCode,
                            SlotTiming = ord.TimeSlot
                            //VendorName=vd.VendorName
                        }).ToList();
            if (statusCode > 0)
            {
                data = data.Where(a => a.OrderStatus_Id == statusCode).ToList();
            }
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                data = data.Where(a => (a.Name.ToLower().Contains(term) || a.PhoneNumber.StartsWith(term) || a.Email.StartsWith(term))).ToList();
            }
            if (!string.IsNullOrEmpty(oid))
            {
                int orid = Convert.ToInt32(oid);
                data = data.Where(a => a.Id.Equals(orid)).ToList();
            }
            if (from != null && to != null)
            {

                data = data.Where(a => a.OrderDate.Date >= from &&
               a.OrderDate.Date <= to
                ).ToList();
            }
            int total = data.Count;
            page = page ?? 1;
            int pageSize = 30;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            data = data.OrderByDescending(a => a.OrderDate).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Order = data;
            return View(model);
        }
        public ActionResult OrderDetail(int id)
        {
            var model = new OrderDetailShowListModelAdmin();
            model.OrderStatusList = new SelectList(ent.OrderStatus.ToList(), "StatusCode", "StatusName");

            var data = (from od in ent.OrderDetails
                        join pd in ent.Products on od.Product_Id equals pd.Id
                        join ord in ent.Orders on od.Order_Id equals ord.Id
                        join os in ent.OrderStatus on od.OrderStatus_Id equals os.StatusCode
                        where od.Order_Id == id
                        select new OrderDetailShowModelAdmin
                        {
                            Id = od.Id,
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
            model.Total_Price = data.Where(a => a.Order_Id == id).Sum(a => a.FinalPrice);
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateOrderDetailStatus(int OrderDetailId, int StatusCode)
        {
            try
            {
                var od = ent.OrderDetails.Find(OrderDetailId);
                if (od == null)
                    throw new Exception("Invalid order id");
                od.OrderStatus_Id = StatusCode;
                od.UpdateDate = DateTime.Now;
                od.StatusUpdateDate = DateTime.Now;
                ent.SaveChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }

        public ActionResult UpdateOrderStatus(int OrderDetailId, int StatusCode)
        {
            try
            {
                var od = ent.Orders.Find(OrderDetailId);
                if (od == null)
                    throw new Exception("Invalid order id");
                od.OrderStatus_Id = StatusCode;
                od.StatusUpdateDate = DateTime.Now;
                ent.SaveChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("error");
            }
        }
        public ActionResult PayStatus(int id)
        {
            var model = new Order();
            int qu = ent.Database.ExecuteSqlCommand(@"update [Order] set PaymentStatus=case when PaymentStatus=0 then 1 else 0 end where Id= " + id);
            return RedirectToAction("Orders");
        }
        public ActionResult Customers(int? page, string term)
        {
            var model = new CustomerListModel();
            var data = (from cu in ent.Customers orderby cu.User_Id descending
                        //join wlt in ent.Wallets on cu.User_Id equals wlt.User_Id
                        select new CustomerModel
                        {
                            User_Id = cu.User_Id,
                            Name = cu.FullName,
                            Email_Id = cu.Email_Id,
                            Phone = cu.Phone,
                            Address = cu.Address,
                            State = cu.State,
                            City = cu.City,
                            Pincode = cu.Pincode,
                            StrwalletAmount = SqlFunctions.StringConvert((double)cu.Mebr_Amount),
                            IsPremiumMember = cu.IsPremiumMember,
                            PremiumMemberOn = cu.PremiumMemberOn
                        }).ToList();
            var data1 = data.OrderByDescending(x=>x.User_Id);
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                data = data.Where(a => (a.Name.ToLower().Contains(term) || a.Phone.StartsWith(term) || a.Email_Id.StartsWith(term))).ToList();
            }

            int total = data.Count;
            page = page ?? 1;
            int pageSize = 30;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            var orderByResult = from s in data
                                orderby s.User_Id
                                select s;

            //var orderByDescendingResult = from s in data
            //                              orderby s.User_Id descending
            //                              select s;
            data = data.Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            var dlata = from v in data orderby v.User_Id descending select v;
            //model.Customer = orderByDescendingResult;
            model.Customer = dlata;
            return View(model);
        }

        public ActionResult MembershipUser(int? page, string term)
        {
            var model = new MembershipUserShowModelList_Admin();
            string query = @"select cu.FullName,cu.User_Id,cu.Email_Id,cu.Phone,cu.Address,
                             cu.State,cu.City,cu.Pincode,wlt.Mebr_Amount 
                             from Customer cu join Wallet wlt on cu.User_Id=wlt.User_Id
                             where wlt.Mebr_Amount>=5000";
            var data = ent.Database.SqlQuery<MembershipUserShowModel_Admin>(query).ToList();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                data = data.Where(a => (a.FullName.ToLower().Contains(term) || a.Phone.StartsWith(term) || a.Email_Id.StartsWith(term))).ToList();
            }

            int total = data.Count;
            page = page ?? 1;
            int pageSize = 30;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            data = data.OrderBy(a => a.User_Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Member = data;
            return View(model);
        }

        public ActionResult JoinMembers(int? page, string term)
        {
            var model = new JoinMemberModelListAdmin();
            var data = (from cu in ent.Customers
                        join rem in ent.RefferalMembers on cu.User_Id equals rem.Refer_Id
                        join wlt in ent.Wallets on cu.User_Id equals wlt.User_Id
                        orderby rem.Id descending
                        //group rem by rem.Refer_Id into rm
                        group new { cu, rem, wlt } by new { rem.Refer_Id, cu.FullName, cu.Email_Id, cu.Address, cu.Phone, wlt.Mebr_Amount } into g
                        select new JoinMemberModelAdmin
                        {
                            User_Id = g.Key.Refer_Id,
                            FullName = g.Key.FullName,
                            Email_Id = g.Key.Email_Id,
                            Address = g.Key.Address,
                            Phone = g.Key.Phone,
                            Wallet_Amount = g.Key.Mebr_Amount,
                            Total_Member = ent.RefferalMembers.Where(a => a.Refer_Id == g.Key.Refer_Id).Count()
                        }).ToList();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                data = data.Where(a => (a.FullName.ToLower().Contains(term) || a.Phone.StartsWith(term) || a.Email_Id.StartsWith(term))).ToList();
            }

            int total = data.Count;
            page = page ?? 1;
            int pageSize = 30;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            data = data.OrderByDescending(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Member = data;
            return View(model);
        }
        public ActionResult MemberDetail(int id, int? page)
        {
            var model = new MemberDetailShowListAdmin();
            var data = (from rem in ent.RefferalMembers
                        join cu in ent.Customers on rem.Login_Id equals cu.User_Id
                        join wlt in ent.Wallets on rem.Login_Id equals wlt.User_Id
                        where rem.Refer_Id == id
                        orderby rem.Id descending
                        select new MemberDetailShowAdmin
                        {
                            Id = rem.Id,
                            Refer_Id = rem.Refer_Id,
                            Login_Id = rem.Login_Id,
                            FullName = cu.FullName,
                            Email_Id = cu.Email_Id,
                            Address = cu.Address,
                            Phone = cu.Phone,
                            Wallet_Amount = wlt.Mebr_Amount,
                            CreateDate = rem.CreateDate
                        }).ToList();
            int total = data.Count;
            page = page ?? 1;
            int pageSize = 30;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            data = data.OrderByDescending(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.MbDetail = data;
            return View(model);
        }
        public ActionResult SubmitCashback(int Id, int cbamt)
        {
            var data = ent.RefferalMembers.Find(Id);
            try
            {
                if (data != null)
                {

                    double logamt = ent.Wallets.Where(a => a.User_Id == data.Login_Id).Select(a => a.Mebr_Amount).FirstOrDefault();
                    double refamt = ent.Wallets.Where(a => a.User_Id == data.Refer_Id).Select(a => a.Mebr_Amount).FirstOrDefault();
                    double cashback = logamt * cbamt / 100;
                    var loguser = ent.Wallets.Where(a => a.User_Id == data.Login_Id).FirstOrDefault();
                    loguser.Mebr_Amount = logamt + cashback;
                    var refuser = ent.Wallets.Where(a => a.User_Id == data.Refer_Id).FirstOrDefault();
                    refuser.Mebr_Amount = refamt + cashback;
                    ent.SaveChanges();

                    var refermail = ent.Customers.Where(a => a.User_Id == data.Refer_Id).Select(a => a.Email_Id).FirstOrDefault();
                    var joinmail = ent.Customers.Where(a => a.User_Id == data.Login_Id).Select(a => a.Email_Id).FirstOrDefault();
                    string refer = refermail;
                    string join = joinmail;
                    string sender = ConfigurationManager.AppSettings["smtpUser"];
                    string password = ConfigurationManager.AppSettings["smtpPass"];
                    ArrayList mailToArr = new ArrayList();
                    mailToArr.Add(refer);
                    mailToArr.Add(join);
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(sender);
                    for (int i = 0; i < mailToArr.Count; i++)
                    {
                        msz.To.Add(new MailAddress((string)mailToArr[i]));
                    }

                    msz.Subject = "Congrates ! your joined member cashback";
                    msz.Body = "Cashback: " + "&#8377" + cashback;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(sender, password);
                    smtp.EnableSsl = true;
                    smtp.Send(msz);

                    return Content("ok");
                }
                return Content("Server error");
            }
            catch (Exception ex)
            {

                return Content("Server error");
            }
        }

        public ActionResult GetChartData()
        {
            List<Order> data = new List<Order>();

            data = ent.Orders.ToList();

            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Date",
                "Total Amount"

            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] { i.OrderDate.Date.ToString(), i.Total };
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Downpaymentlist()
        {
            var data = ent.DownPyments.ToList();
            return View(data);
        }

        //======Add Blog=======

        //[HttpGet]
        //public ActionResult AddBlog()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult AddBlog(BlogMasterDTO model)
        //{
        //    try
        //    {
        //        if (model.BlogFile != null)
        //        {
        //            if (model.BlogFile.ContentLength > 3 * 1024 * 1024)
        //            {
        //                TempData["msg"] = "Image should not exceed 3 mb";
        //                return View(model);
        //            }
        //            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" };
        //            string ext = Path.GetExtension(model.BlogFile.FileName);
        //            if (!allowedExtensions.Contains(ext))
        //            {
        //                TempData["msg"] = "only .jpg, .jpeg, .gif and .png files are allowed";
        //                return View(model);
        //            }
        //            var filrName = Guid.NewGuid().ToString() + Path.GetExtension(model.BlogFile.FileName);
        //            model.BlogFile.SaveAs(Server.MapPath("/BlogImages/") + filrName);
        //            model.BlogImage = "/BlogImages/" + filrName;
        //        }
        //        var domain = Mapper.Map<BlogMaster>(model);
        //        string encodeUrl = model.Url.Replace(" ", "-");
        //        domain.Url = Convert.ToString(encodeUrl);
        //        domain.BlogImage = model.BlogImage;
        //        domain.Date = DateTime.Now;
        //        ent.BlogMasters.Add(domain);
        //        ent.SaveChanges();
        //        TempData["msg"] = "Successfully Saved";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg"] = "Server error..";
        //        return RedirectToAction("AddBlog");
        //    }
        //    return RedirectToAction("AddBlog");
        //}
       
        public ActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBlog(BlogMasterDTO model)
        {
            try
            {
                if (model.BlogFile != null)
                {
                    if (model.BlogFile.ContentLength > 3 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not exceed 3 mb";
                        return View(model);
                    }
                    var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" };
                    string ext = Path.GetExtension(model.BlogFile.FileName);
                    if (!allowedExtensions.Contains(ext))
                    {
                        TempData["msg"] = "only .jpg, .jpeg, .gif and .png files are allowed";
                        return View(model);
                    }
                    var filrName = Guid.NewGuid().ToString() + Path.GetExtension(model.BlogFile.FileName);
                    model.BlogFile.SaveAs(Server.MapPath("/BlogImages/") + filrName);
                    model.BlogImage = "/BlogImages/" + filrName;
                }
                var domain = Mapper.Map<BlogMaster>(model);
                string encodeUrl = model.Url.Replace(" ", "-");
                domain.Url = Convert.ToString(encodeUrl);
                domain.BlogImage = model.BlogImage;
                domain.Date = DateTime.Now;
                ent.BlogMasters.Add(domain);
                ent.SaveChanges();
                TempData["msg"] = "Successfully Saved";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error..";
                return RedirectToAction("AddBlog");
            }
            return RedirectToAction("AddBlog");
        }

        public ActionResult BlogDetails()
        {
            var model = new BlogMasterDTO();
            string q = @"select * from BlogMaster order by Id desc";
            var data = ent.Database.SqlQuery<Blogs>(q).ToList();
            model.Blog =data;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditBlog(int Id)
        {
            var data = ent.BlogMasters.Find(Id);
            var domain = Mapper.Map<BlogMasterDTO>(data);
            return View(domain);
        }

        [HttpPost]
        public ActionResult EditBlog(BlogMasterDTO model)
        {
            var domain = Mapper.Map<BlogMaster>(model);
            try
            {
                if (model.BlogFile != null)
                {
                    if (model.BlogFile.ContentLength > 3 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not exceed 3 mb";
                        return View(model);
                    }
                    var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" };
                    string ext = Path.GetExtension(model.BlogFile.FileName);
                    if (!allowedExtensions.Contains(ext))
                    {
                        TempData["msg"] = "only .jpg, .jpeg, .gif and .png files are allowed";
                        return View(model);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.BlogFile.FileName);
                    model.BlogFile.SaveAs(Server.MapPath("/BlogImages/") + fileName);
                    model.BlogImage = "/BlogImages/" + fileName;
                }
                if (model.BlogImage != null) 
                {
                    domain.BlogImage = model.BlogImage;
                    domain.Date = DateTime.Now;
                    string encodeUrl = model.Url.Replace(" ", "-");
                    domain.Url = Convert.ToString(encodeUrl);
                    ent.Entry<BlogMaster>(domain).State = EntityState.Modified;
                    ent.SaveChanges();
                    TempData["msg"] = "Successfully Saved";
                    return RedirectToAction("BlogDetails");
                }
                else
                {
                    TempData["msg"] = "Please select image.";
                    return RedirectToAction("EditBlog", new { id = model.Id });
                }
            }
            catch
            {
                return RedirectToAction("EditBlog", new { id = model.Id });
            }
        }
        public ActionResult DeleteBlog(int Id)
        {
            string q = @"delete from BlogMaster where Id=" + Id;
            ent.Database.ExecuteSqlCommand(q);
            return RedirectToAction("BlogDetails");
        }
        
        //====create coupon====

        public ActionResult ListCoupon()
        {
            return View(ent.Discount_Coupon.ToList());
        }

        public ActionResult DeleteCoupon(int id)
        {
            try
            {
                var result = ent.Discount_Coupon.FirstOrDefault(x => x.id == id);
                if (result != null)
                {
                    ent.Discount_Coupon.Remove(result);
                    ent.SaveChanges();

                }
                TempData["coumpon-delete"] = "Coupon Delete SuccessFully";
                return RedirectToAction("ListCoupon");
            }
            catch
            {
                throw new Exception("Server Error");
            }
        }
        public ActionResult CreateCuopon()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCuopon(Discount_Coupon model)
        {
            try
            {
                int otpValue = new Random().Next(10000, 99999);
                Discount_Coupon emp = new Discount_Coupon()
                {
                    CouponCode = "Gyros" + otpValue,
                    Amount = model.Amount,
                    Name = model.Name,
                    MaximumAmount = model.MaximumAmount,
                    MinimumAmount = model.MinimumAmount
                };
                ent.Discount_Coupon.Add(emp);
                ent.SaveChanges();
                ModelState.Clear();
                TempData["Coupon"] = "Coupon Create successfully";
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception("Server Error");
            }
        }


        //====GetReviewAdmin====
        [HttpGet]
        public ActionResult GetReviewAdmin()
        {
            try
            {
                var result = ent.Reviews.ToList();
                return View(result);
            }
            catch
            {
                throw new Exception("Server Error");
            }
        }

        public ActionResult DeleteReview(int id)
        {
            try
            {
                var result = ent.Reviews.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    ent.Reviews.Remove(result);
                    ent.SaveChanges();
                }
                return RedirectToAction("GetReviewAdmin");
            }
            catch
            {
                throw new Exception("Server Error");
            }
        }
    }
}
