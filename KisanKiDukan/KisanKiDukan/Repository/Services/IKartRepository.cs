using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisanKiDukan.Repository.Services
{
    interface IKartRepository
    {
        bool AddKart(Kart K);
        int GetKartIdByClientId(int Id);
        int GetClientIdByKartId(int Id);
    }
}
