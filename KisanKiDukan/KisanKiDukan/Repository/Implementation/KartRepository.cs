using KisanKiDukan.Models.Domain;
using KisanKiDukan.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository.Implementation
{
    public class KartRepository : IKartRepository
    {
        private DbEntities ent = new DbEntities();
        public bool AddKart(Kart K)
        {
            try
            {
                ent.Karts.Add(K);
                ent.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetClientIdByKartId(int kId)
        {
            return ent.Karts.Find(kId).Id;
        }

        public int GetKartIdByClientId(int Id)
        {
            return ent.Karts.Where(a => a.Client_Id == Id).Select(a => a.Id).FirstOrDefault();
        }
    }
}