using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    interface IProductRepository
    {
        int AddProduct(Product product);
        bool RemoveProduct(Product product);
        bool UpdateProduct(Product product);
        double GetPriceByProductId(int Id);
        IEnumerable<Product> GetProductByCategoryId(int Id);
        Product GetProductById(int Id);
    }
}
