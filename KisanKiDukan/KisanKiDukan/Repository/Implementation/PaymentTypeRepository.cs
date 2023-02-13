using KisanKiDukan.Models.Domain;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class PaymentTypeRepository: IPaymentTypeRepository
    {
        DbEntities ent = new DbEntities();
        public bool AddPaymentType(PaymentType PT)
        {
            try
            {
                ent.PaymentTypes.Add(PT);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<PaymentType> GetAllPaymentTypes()
        {
            return ent.PaymentTypes.ToList();
        }

        public PaymentType GetPaymentTypeById(int Id)
        {
            return ent.PaymentTypes.Find(Id);
        }

        public bool RemovePaymentType(PaymentType PT)
        {
            try
            {
                ent.PaymentTypes.Remove(PT);
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