using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class VendorController : Controller
    {
        DbEntities ent = new DbEntities();
        CommonRepository cr = new CommonRepository();

        public ActionResult SellerDashboard()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult Add()
        {
            var model = new VendorDTO();
            model.BusinessStatuList = new SelectList(ent.BusinessStatus.ToList(), "Id", "BusinessFStatus");
            model.StateList = new SelectList(ent.States.ToList(), "Id", "StateName");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(VendorDTO model,string prevBtn, string nextBtn)
        {
            model.StateList = new SelectList(ent.States.ToList(), "Id", "StateName");
            if (!ModelState.IsValid)
                return View(model);
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {  
                    if(model.Id== Convert.ToInt32(Session["VendorId"]) && nextBtn != null)
                    {
                        var login = new LoginMaster { Username = model.EmailId, Password = model.Password, Role = "Vendor" };
                        ent.LoginMasters.Add(login);
                        ent.SaveChanges();
                        var vendor = Mapper.Map<Vendor>(model);
                        vendor.RegistrationDate = DateTime.Now;
                        vendor.LoginMaster_Id = login.Id;
                        ent.Vendors.Add(vendor);
                        ent.SaveChanges();
                        tran.Commit();
                        string msg = "Hi " + vendor.VendorName + ", your kisankidukaan.in login credentials are";
                        msg += " Username: " + vendor.EmailId + " and Password : " + login.Password;
                        SmsOperation.SendSms(vendor.ContactNumber, msg);
                    }                  
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server error.";
                    tran.Rollback();
                    return View("Add",model);
                }
            }
            var data = ent.Vendors.Where(a => a.EmailId == model.EmailId && a.Password == model.Password).FirstOrDefault();
            Session["VendorId"] = data.Id;
            var model1 = new VendorBusinessDetailsDTO();
            model1.Id= data.Id;
            model1.BusinessStatuList = new SelectList(ent.BusinessStatus.ToList(), "Id", "BusinessFStatus");
            return View("BusinessDetails", model1);
        }

        public ActionResult BusinessDetails()
        {
            var model = new VendorBusinessDetailsDTO();
            model.BusinessStatuList = new SelectList(ent.BusinessStatus.ToList(), "Id", "BusinessFStatus");
            return View(model);
        }

        [HttpPost]
        public ActionResult BusinessDetails(VendorBusinessDetailsDTO model, string prevBtn, string nextBtn)
        {
            model.BusinessStatuList = new SelectList(ent.BusinessStatus.ToList(), "Id", "BusinessFStatus");

            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    int vendorid = Convert.ToInt32(Session["VendorId"]);
                    var data = ent.Vendors.Find(vendorid);
                    if(prevBtn != null)
                    {
                        var model1 = Mapper.Map<VendorDTO>(data);
                        return View("Add",model1);
                    }
                    if (data.Id == vendorid && nextBtn !=null)
                    {
                        data.CompanyName = model.CompanyName;
                        data.LegalCompanyName=model.LegalCompanyName;
                        data.BusinessFilingStatus = model.BusinessFilingStatus;
                        data.PAN_No = model.PAN_No;
                        data.RegisteredAddress = model.RegisteredAddress;
                        data.OperatingAddress = model.OperatingAddress;
                        ent.SaveChanges();
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server error.";
                    tran.Rollback();
                }
            }
            return View("BankAcDetails");
        }

        public ActionResult BankAcDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BankAcDetails(VendorBankAcDetailsDTO model, string prevBtn, string nextBtn)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    int vendorid = Convert.ToInt32(Session["VendorId"]);
                    var data = ent.Vendors.Find(vendorid);
                    if (prevBtn != null)
                    {
                        VendorBusinessDetailsDTO bd = new VendorBusinessDetailsDTO();
                        bd.CompanyName = data.CompanyName;
                        bd.LegalCompanyName = data.LegalCompanyName;
                        bd.BusinessFilingStatus = data.BusinessFilingStatus;
                        bd.PAN_No = data.PAN_No;
                        bd.RegisteredAddress = data.RegisteredAddress;
                        bd.OperatingAddress = data.OperatingAddress;
                        return View("BusinessDetails", bd);
                    }
                    if (data.Id == vendorid && nextBtn != null)
                    {
                        data.PayToName = model.PayToName;
                        data.BankName = model.BankName;
                        data.AccountNumber = model.AccountNumber;
                        data.IFSC_Code = model.IFSC_Code;
                        data.BranchAddress = model.BranchAddress;
                        ent.SaveChanges();
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server error.";
                    tran.Rollback();
                }
            }           
            return RedirectToAction("VendorUploadDoc", new { id= Convert.ToInt32(Session["VendorId"]) });
        }

        public ActionResult VendorUploadDoc(int id)
        {
            VendorDTO obj = new VendorDTO();
            var data = ent.Vendors.Find(id);
            obj.Id = data.Id;
            return View(obj);
        }

        [HttpPost]
        public ActionResult VendorUploadDoc(VendorDTO model,int id)
        {
            var data = ent.Vendors.Find(id);
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    if (data != null)
                    {
                        if(model.DocumentFile1 !=null && model.DocumentFile2 != null && model.DocumentFile3 != null && model.DocumentFile4 != null && model.DocumentFile5 != null && model.DocumentFile6 != null && model.DocumentFile7 != null)
                        {
                            data.PanCard = FileOperation.CheckFileSize(model.DocumentFile1);
                            data.AddressProof = FileOperation.CheckFileSize(model.DocumentFile2);
                            data.CancelCheque = FileOperation.CheckFileSize(model.DocumentFile3);
                            data.SignedDocument = FileOperation.CheckFileSize(model.DocumentFile4);
                            data.GovtCertificate = FileOperation.CheckFileSize(model.DocumentFile5);
                            data.FoodLicence = FileOperation.CheckFileSize(model.DocumentFile6);
                            data.BusinessDocumnet = FileOperation.CheckFileSize(model.DocumentFile7);
                            ent.SaveChanges();
                            trans.Commit();
                            TempData["msg"] = "Records has added successfully.";
                        }
                        else
                        {
                            trans.Rollback();
                            TempData["msg"] = "Please Upload All document.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "Server error";
                    return View(model);
                }
                return RedirectToAction("VendorUploadDoc");
            }
        }

        public ActionResult Reciept(int id, bool pdf = false)
        {
            var user = ent.Vendors.Where(a => a.Id == id).FirstOrDefault();
            return View(user);
        }
        
        public ActionResult All(string term = "", int page = 1)
        {
            var model = new VendorListVm();           
            var data = Mapper.Map<IEnumerable<VendorDTO>>(ent.Vendors.OrderByDescending(a => a.Id).ToList()).ToList();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                data = data.Where(a => a.VendorName.ToLower().Contains(term) || a.ContactNumber.Contains(term) || a.ContactPerson.ToLower().Contains(term)).ToList();
                page = 1;
            }
            int total = data.Count;
            int pageSize = 50;
            double totalPages = Math.Ceiling(total / (double)pageSize);
            int skip = pageSize * (page - 1);
            data = data.Skip(skip).Take(pageSize).ToList();
            model.Vendors = data;
            model.Term = term;
            model.Page = page;
            model.NumberOfPages = (int)totalPages;
            return View(model);
        }

        public ActionResult UpdateStatus(int id)
        {
            ent.Database.ExecuteSqlCommand(@"update Vendor set IsApproved=case when IsApproved=1 then 0 else 1 end where Id= " + id);
            return RedirectToAction("All");
        }

        public ActionResult Commercial(int id)
        {
            var vendor = ent.Vendors.Find(id);
            var model = Mapper.Map<VendorDTO>(vendor);
            return View(model);
        }

        [HttpPost]
        public ActionResult Commercial(VendorDTO model)
        {
            try
            {
                int executed = ent.UpdateVendorDetails(model.Commission,model.PaymentGatewayCharge,model.DeliveryCharge,model.Id);
                TempData["msg"] = "Record(s) has saved successfully.";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error.";
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var vendor = ent.Vendors.Find(id);
                    var login = ent.LoginMasters.Find(vendor.LoginMaster_Id);
                    ent.LoginMasters.Remove(login);
                    ent.Vendors.Remove(vendor);
                    ent.SaveChanges();
                    tran.Commit();
                    return Content("ok");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return Content("Server Error");
                }
            }
        }
        public ActionResult Edit(int id)
        {
            var vendor = ent.Vendors.Find(id);
            var model = Mapper.Map<VendorDTO>(vendor);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(VendorDTO model)
        {
            try
            {
                var vendor = Mapper.Map<Vendor>(model);
                ent.Entry(vendor).State=System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Record(s) has saved successfully.";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Server error.";
                return View(model);
            }
        }

        public ActionResult PaymentHistory()
        {
            return View();
        }
        public ActionResult OrderHistory(int? page, string term, string oid, DateTime? from, DateTime? to, int statusCode = 0)
        {
            var model = new OrderShowListModelAdmin();
            var VId = Convert.ToInt32(Session["AddBy"]);
            if (User.IsInRole("Vendor"))
            {
                model.OrderStatusList = new SelectList(ent.OrderStatus.ToList(), "StatusCode", "StatusName");
                var data = (from ord in ent.Orders
                            join cu in ent.Customers on ord.Client_Id equals cu.User_Id
                            join os in ent.OrderStatus on ord.OrderStatus_Id equals os.StatusCode
                            where ord.VendorId== VId
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
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult VendorUpdateOrderDetailStatus(int OrderDetailId, int StatusCode)
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
        public ActionResult VendorUpdateOrderStatus(int OrderDetailId, int StatusCode)
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
        public ActionResult VendorPayStatus(int id)
        {
            var model = new Order();
            int qu = ent.Database.ExecuteSqlCommand(@"update [Order] set PaymentStatus=case when PaymentStatus=0 then 1 else 0 end where Id= " + id);
            return RedirectToAction("Orders");
        }

        public ActionResult VendorOrderDetail(int id)
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
        
        public ActionResult ShowADCategory()
        {
            var data1 = (from adc in ent.ADCategories
                         where adc.IsActive == true
                         select new ADCategoryModel
                         {
                             Id = adc.Id,
                             CategoryName = adc.CategoryName,
                             ParentCategory = adc.ParentCategory,
                         }).ToList();
            var data2 = (from subadc in ent.ADSubCategories
                         select new ADSubCategoryModel
                         {
                             Name = subadc.Name,
                             MainCat_Id = subadc.MainCat_Id
                         }).ToList();
            AllADCategoriesModel model = new AllADCategoriesModel
            {
                ADCategoryList = data1,
                SubCategoryList = data2
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult AddADProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddADProduct(ADProductModel model, IEnumerable<HttpPostedFileBase> images)
        {
            using (var trans = ent.Database.BeginTransaction())
            {
                try
                {
                    if (model.ImageFile != null)
                    {
                        if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                        {
                            TempData["msg"] = "Image should not succeed 2 mb";
                            return View(model);
                        }
                        var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                        if (uploadResult == "not allowed")
                        {
                            TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                            return View(model);
                        }
                        model.ADImage = uploadResult;
                        ViewBag.ImageUrl = "Images/" + uploadResult;
                    }
                    var adProduct = Mapper.Map<ADProduct>(model);
                    adProduct.VendorId = Convert.ToInt32(Session["AddBy"]);
                    ent.ADProducts.Add(adProduct);
                    HttpPostedFileBase file = Request.Files["images"];
                    if (file.ContentLength != 0)
                    {
                            List<ADProductDetail> prodimg = new List<ADProductDetail>();
                            if (images != null && images.Count() <= 5)
                            {
                                foreach (var image in images)
                                {
                                    if (image.ContentLength > 0)
                                    {
                                        var path = FileOperation.UploadImage(image, "Images");
                                        var imageList = new ADProductDetail()
                                        {
                                            ADImages = path,
                                            ADProductId = adProduct.Id
                                        };
                                        prodimg.Add(imageList);
                                    }
                                }
                                adProduct.ADProductDetails = prodimg;
                                ent.ADProducts.Add(adProduct);
                            }
                        else
                        {
                            TempData["msg"] = "Please select five images.";
                            return RedirectToAction("AddADProduct");
                        }
                    }
                    ent.SaveChanges();
                    trans.Commit();
                    TempData["msg"] = "Records has added successfully.";
                    return RedirectToAction("AddADProduct");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    TempData["msg"] = "Server error";
                    return View();
                }
            }
        }



    }
}