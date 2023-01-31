using KisanKiDukan.Models.Domain;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        DbEntities ent = new DbEntities();
        public bool AddStatus(OrderStatu OS)
        {
            try
            {
                ent.OrderStatus.Add(OS);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<OrderStatu> GetAllOrderStatus()
        {
            return ent.OrderStatus.ToList();
        }

        public OrderStatu GetOrderStatusByID(int Id)
        {
            return ent.OrderStatus.Find(Id);
        }

        public bool RemoveOrderStatus(OrderStatu OS)
        {
            try
            {
                ent.OrderStatus.Remove(OS);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateOrderStatus(OrderStatu OS)
        {
            try
            {
                ent.Entry(OS).State = EntityState.Modified;
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