using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Controllers
{
    public class StateCityLocManagerController : Controller
    {
        DbEntities ent = new DbEntities();
        public ActionResult AddState()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddState(StateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var state = Mapper.Map<State>(model);
                state.StateName = model.StateName;
                ent.States.Add(state);
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
            }
            catch 
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("AddState");
        }
        public ActionResult ShowState()
        {
            var data = ent.States.AsNoTracking().ToList();
            return View(data);
        }
        public ActionResult DeleteState(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("Delete from State where Id=" + id);
            }
            catch { }
            return RedirectToAction("ShowState");
        }
        public ActionResult AddCity()
        {
            var data = new CityModel();
            data.StateList = new SelectList(ent.States.Where(a=>a.StateName!=null).ToList(),"Id", "StateName");
            return View(data);
        }

        [HttpPost]
        public ActionResult AddCity(CityModel model)
        {
            try
            {
                model.StateList = new SelectList(ent.States.Where(a => a.StateName != null).ToList(), "Id", "StateName");
                if (!ModelState.IsValid)
                {
                    model.StateList = new SelectList(ent.States.Where(a => a.StateName != null).ToList(), "Id", "StateName");
                    return View(model);
                }
                var data = Mapper.Map<City>(model);
                data.State_Id = model.State_Id;
                data.CityName = model.CityName;
                ent.Cities.Add(data);
                ent.SaveChanges();
                TempData["msg"] = "Records has added successfully.";
            }
            catch
            {
                TempData["msg"] = "Error has been occured.";
            }
            return RedirectToAction("AddCity");
        }
        public ActionResult ShowCity()
        {
            var data = ent.Cities.AsNoTracking().ToList();
            return View(data);
        }
        public ActionResult DeleteCity(int id)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("Delete from City where Id=" + id);
            }
            catch { }
            return RedirectToAction("ShowCity");
        }
        public ActionResult AddLocality()
        {
            return View();
        }
        public ActionResult ShowLocality()
        {
            return View();
        }
    }
}