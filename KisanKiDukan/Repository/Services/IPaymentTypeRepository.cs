using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    public interface IPaymentTypeRepository
    {
        bool AddPaymentType(PaymentType PT);
        bool RemovePaymentType(PaymentType PT);
        IEnumerable<PaymentType> GetAllPaymentTypes();
        PaymentType GetPaymentTypeById(int Id);
    }
}
