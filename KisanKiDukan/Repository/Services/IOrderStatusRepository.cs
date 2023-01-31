using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    interface IOrderStatusRepository
    {
        bool AddStatus(OrderStatu OS);
        bool RemoveOrderStatus(OrderStatu OS);
        bool UpdateOrderStatus(OrderStatu OS);
        OrderStatu GetOrderStatusByID(int Id);
        IEnumerable<OrderStatu> GetAllOrderStatus();
    }
}
