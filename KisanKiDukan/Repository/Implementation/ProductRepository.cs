using KisanKiDukan.Models.Domain;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private DbEntities ent = new DbEntities();

        //IProductRepository repos = new ProductRepository();

        public int? OfferType { get; private set; }
        public int AddProduct(Product product)
        {
            try
            {
                ent.Products.Add(product);
                ent.SaveChanges();
                return product.Id;
            }
            catch
            {
                return 0;
            }
        }

        public double GetPriceByProductId(int Id)
        {
            var data = ent.Products.Where(a => a.Id == Id).Select(a => a.Price).FirstOrDefault();
            return data ?? 0;
        }

        public IEnumerable<Product> GetProductByCategoryId(int Id)
        {
            try
            {
                return ent.Products.Where(a => a.Category_Id == Id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public int GetUserVenderId()
        {
            int Id = int.Parse(HttpContext.Current.User.Identity.Name);
            var userData = ent.Vendors.Where(A => A.LoginMaster_Id == Id).FirstOrDefault();
            if (userData != null)
            {
                return userData.Id;
            }
            return 0;
        }

        public Product GetProductById(int Id)
        {
            try
            {
                return ent.Products.Where(a => a.Id == Id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public bool RemoveProduct(Product product)
        {
            try
            {
                ent.Products.Remove(product);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                ent.Entry<Product>(product).State = System.Data.Entity.EntityState.Modified;
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}