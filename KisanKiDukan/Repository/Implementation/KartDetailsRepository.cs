using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class KartDetailsRepository : IKartDetailsRepository
    {
        private DbEntities ent = new DbEntities();
        public bool AddKartDetails(KartDetail KD)
        {
            try
            {
                ent.KartDetails.Add(KD);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public KartDetail GetKartDetailsById(int Id)
        {
            return ent.KartDetails.Find(Id);
        }

        public double? GetTotalByKartId(int Id)
        {
            var v = ent.KartDetails.Where(a => a.Kart_Id == Id).ToList();
            double? Total = 0;
            foreach (var i in v)
            {
                Total += i.Price * i.Quantity;
            }
            return Total;
        }

        public KartModel ItemsInKartByKartId(int Id)
        {
            var v = ent.Database.SqlQuery<KartDetailsDTO>(@"Select k.Id,k.Kart_Id,
                                                           p.ProductName as Product,
                                                           p.ProductImage as Image,
                                                           p.Id as PID,
                                                           k.Quantity,
                                                           k.price 
                                                           from KartDetail k  
                                                            join Product p on k.Product_id=p.Id
                                                            where k.Kart_Id=" + Id).ToList();
            KartModel KM = new KartModel
            {
                Data = v,
                Total = GetTotalByKartId(Id),
            };
            return KM;
        }
        public void MinusOne(int Id)
        {
            var v = ent.KartDetails.Find(Id);
            v.Quantity--;
            if (v.Quantity < 1)
            {
                ent.KartDetails.Remove(v);
            }
            ent.SaveChanges();

        }

        public void PlusOne(int Id)
        {
            var v = ent.KartDetails.Find(Id);
            v.Quantity++;
            ent.SaveChanges();
        }

        public bool RemoveKartDetails(KartDetail KD)
        {
            ent.KartDetails.Remove(KD);
            ent.SaveChanges();
            return true;
        }
    }
}