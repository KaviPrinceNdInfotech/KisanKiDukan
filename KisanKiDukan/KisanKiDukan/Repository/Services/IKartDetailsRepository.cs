using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    interface IKartDetailsRepository
    {
        bool AddKartDetails(KartDetail KD);
        bool RemoveKartDetails(KartDetail KD);
        double? GetTotalByKartId(int Id);
        KartDetail GetKartDetailsById(int Id);
        KartModel ItemsInKartByKartId(int Id);
        void PlusOne(int Id);
        void MinusOne(int Id);
    }
}
