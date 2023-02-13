using AutoMapper;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KisanKiDukan.Controllers
{
    public class ADCategoryApiController : ApiController
    {
        DbEntities ent = new DbEntities();

        [HttpPost, Route("api/ADCategoryApi/Add")]
        public IHttpActionResult Add(ADCategoryDTO model)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var record = Mapper.Map<ADCategory>(model);
                    record.CategoryName = model.CategoryName;
                    record.IsActive = true;
                    record.IsFeature = true;
                    ent.ADCategories.Add(record);
                    ent.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok(model);
            }
        }

        [HttpGet, Route("api/ADCategoryApi/GetADCategory")]
        public IHttpActionResult GetADCategory()
        {
            var record = ent.ADCategories.ToList();
            return Ok(record);
        }

        [HttpPut, Route("api/ADCategoryApi/UpdateADCategory")]
        public IHttpActionResult UpdateADCategory(ADCategoryDTO adCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.ADCategories.Where(s => s.Id == adCategory.Id).FirstOrDefault<ADCategory>();
                    if (data != null)
                    {
                        data.CategoryName = adCategory.CategoryName;
                        ent.SaveChanges();
                        tran.Commit();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok("Data updated successfuly!!");
            }
        }
        
        [HttpDelete, Route("api/ADCategoryApi/DeleteADCategory")]
        public IHttpActionResult DeleteADCategory(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid id");

            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.ADCategories.Where(s => s.Id == id).FirstOrDefault();
                    ent.Entry(data).State = System.Data.Entity.EntityState.Deleted;
                    ent.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok("Data deleted successfuly!!");
            }
        }
        
        [HttpPost, Route("api/ADCategoryApi/AddSub")]
        public IHttpActionResult AddSub(ADSubCategoryDTO model)
        {
            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var record = Mapper.Map<ADSubCategory>(model);
                    record.Name = model.Name;
                    //record.CategoryImage = model.CategoryImage;
                    //record.UpToText = model.UpToText;
                    ent.ADSubCategories.Add(record);
                    ent.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok(model);
            }
        }

        [HttpGet, Route("api/ADCategoryApi/GetSubADCategory")]
        public IHttpActionResult GetSubADCategory()
        {
            var record = ent.ADSubCategories.ToList();
            return Ok(record);
        }

        [HttpPut, Route("api/ADCategoryApi/UpdateADSubCategory")]
        public IHttpActionResult UpdateADSubCategory(ADSubCategoryDTO adSubCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.ADSubCategories.Where(s => s.Id == adSubCategory.Id).FirstOrDefault<ADSubCategory>();
                    if (data != null)
                    {
                        data.Name = adSubCategory.Name;
                        data.MainCat_Id = adSubCategory.MainCat_Id;
                        ent.SaveChanges();
                        tran.Commit();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok("Data updated successfuly!!");
            }
        }
        
        [HttpDelete, Route("api/ADCategoryApi/DeleteADSubCategory")]
        public IHttpActionResult DeleteADSubCategory(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid id");

            using (var tran = ent.Database.BeginTransaction())
            {
                try
                {
                    var data = ent.ADSubCategories.Where(s => s.Id == id).FirstOrDefault();
                    ent.Entry(data).State = System.Data.Entity.EntityState.Deleted;
                    ent.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                return Ok("Data deleted successfuly!!");
            }
        }
    }
}
