using AutoMapper;
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
    public class LocationController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult LocationAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LocationAdd(AddLocationDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return View(model);
                }
                var data = Mapper.Map<DeliveryLocation>(model);
                ent.DeliveryLocations.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Record added successfully";
                return RedirectToAction("LocationAdd");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Some error";
                return View(model);
            }

        }

        public ActionResult Locations(int? page)
        {
            var model = new LocationListModel();
            var location = (from dl in ent.DeliveryLocations

                            select new LocationViewModel
                            {

                                Id = dl.Id,
                                LocationName = dl.LocationName,
                                TimeDay = dl.TimeDay
                            }
                           ).ToList();

            int total = location.Count;

            page = page ?? 1;
            int pageSize = 100;
            decimal noOfPages = Math.Ceiling((decimal)total / pageSize);
            model.NumberOfPages = (int)noOfPages;
            model.Page = page;
            location = location.OrderBy(a => a.Id).Skip(pageSize * ((int)page - 1)).Take(pageSize).ToList();
            model.Location = location;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var data = Mapper.Map<AddLocationDTO>(ent.DeliveryLocations.Find(id));

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(AddLocationDTO model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            try
            {
                var data = Mapper.Map<DeliveryLocation>(model);
                ent.Entry(data).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                TempData["msg"] = "Record added successfully";
                return RedirectToAction("Locations");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Some error";
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand(@"delete from DeliveryLocation where Id=" + id);
                return RedirectToAction("Locations");
            }
            catch
            {
                return RedirectToAction("Locations");
            }

        }
    }
}