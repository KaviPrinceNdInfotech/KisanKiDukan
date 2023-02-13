using AutoMapper;
using KisanKiDukan.Utilities;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class OfferBannerController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult AddOfferBanner()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOfferBanner(BannerAddDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var banner = Mapper.Map<BannerImage>(model);
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
                    banner.BannerPath = uploadResult;
                }
                banner.IsActive = true;
                ent.BannerImages.Add(banner);
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";

            }
            return RedirectToAction("AddOfferBanner");
        }

        public ActionResult ShowBanner(int? page)
        {
            var model = new BannerShowModel();
            var banner = (from bi in ent.BannerImages
                          where bi.IsActive == true
                          select new BannersModel
                          {
                              Id = bi.Id,
                              BannerPath = bi.BannerPath
                          }
                           ).ToList();
            int total = banner.Count;
            page = page ?? 1;
            int pageSize = 100;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            banner = banner.OrderBy(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Banner = banner;
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var img = ent.BannerImages.Find(id);
            if (img != null)
            {
                var path = Server.MapPath("/Images/" + img.BannerPath);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                    ent.BannerImages.Remove(img);
                    ent.SaveChanges();
                    return RedirectToAction("ShowBanner");
                }
                else
                {
                    img.IsActive = false;
                    ent.SaveChanges();
                }
            }
            return RedirectToAction("ShowBanner");
        }

        public ActionResult Pbaner()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pbaner(promotionalbannerDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var banner = Mapper.Map<promotionalbanner>(model);
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
                banner.promotionalbannerpath = uploadResult;
            }
            ent.promotionalbanners.Add(banner);
            ent.SaveChanges();
            TempData["msg"] = "Records has added successfully.";
            return RedirectToAction("Pbaner");
        }
        public ActionResult ShowPBanner(int? page)
        {
            var model = new pbannershowmodel();
            var banner = (from bi in ent.promotionalbanners
                          where bi.IsActive == true
                          select new PBannersModel
                          {
                              Id = bi.Id,
                              promotionalbannerpath = bi.promotionalbannerpath
                          }
                           ).ToList();

            int total = banner.Count;
            page = page ?? 1;
            int pageSize = 100;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            banner = banner.OrderBy(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.ProBanner = banner;
            return View(model);
        }
        public ActionResult PBDelete(int id)
        {
            var img = ent.promotionalbanners.Find(id);
            if (img != null)
            {
                var path = Server.MapPath("/Images/" + img.promotionalbannerpath);
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                    ent.promotionalbanners.Remove(img);
                    ent.SaveChanges();
                    return RedirectToAction("ShowBanner");
                }
                else
                {
                    img.IsActive = false;
                    ent.SaveChanges();
                }
            }
            return RedirectToAction("ShowBanner");
        }
    }
}