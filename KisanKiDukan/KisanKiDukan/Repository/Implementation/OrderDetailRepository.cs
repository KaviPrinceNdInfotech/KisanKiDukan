using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private DbEntities ent = new DbEntities();
        public bool AddOrderDetails(OrderDetail OD)
        {
            try
            {
                ent.OrderDetails.Add(OD);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailByOrder_Id(int Id)
        {
            return ent.OrderDetails.Where(a => a.Order_Id == Id).ToList();
        }
        public CustomerOrderModel OrderItemDetailById(int Id)
        {
            var v = ent.Database.SqlQuery<CODOrderDTO>(@"select od.Id,
 od.Product_Id,
pd.ProductImage, 
 od.ProductName, 
 od.Price, 
 od.Order_Id, 
 od.Quantity,
 ord.Total,
 ord.OrderStatus_Id,
 ord.OrderDate,
 ord.PaymentType_Id,
 ord.Client_Id,
 ord.[Address],
 ord.Name,
 ord.PhoneNumber,
 ord.City,
 ord.PinCode,
 ord.[State],
 pt.PaymentMode,
 od.OrderStatus_Id from OrderDetail od
 join [Order] ord on od.Order_Id=ord.Id
 join PaymentType pt on ord.PaymentType_Id=pt.Id
 join Product pd on od.Product_Id=pd.Id
 where ord.Client_Id=" + Id).ToList();
            CustomerOrderModel CM = new CustomerOrderModel
            {
                Data = v,
            };
            return CM;
        }
    }
}