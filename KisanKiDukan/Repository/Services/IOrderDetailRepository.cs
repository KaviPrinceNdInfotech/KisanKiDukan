using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    public interface IOrderDetailRepository
    {
        bool AddOrderDetails(OrderDetail OD);
        IEnumerable<OrderDetail> GetOrderDetailByOrder_Id(int Id);
        CustomerOrderModel OrderItemDetailById(int Id);
    }
}
