using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.NewRepository
{
    public class UserCookie
    {
        private static DbEntities ent = new DbEntities();
        public static int ClientId
        {
            get
            {
                int ClientId = 0;
                if (HttpContext.Current.Request.Cookies["user"] != null)
                {
                    var userName = HttpContext.Current.Request.Cookies["user"].Value;
                    var user = ent.Customers.FirstOrDefault(a => a.Email_Id == userName);
                    if (user != null)
                        ClientId = user.User_Id;
                }
                return ClientId;
            }
        }

        public static bool IsInCookie
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["user"] != null) return true;
                else return false;
            }
        }

        public static string UserName
        {
            get
            {
                string name = "";
                if (HttpContext.Current.Request.Cookies["user"] != null)
                {
                    var userName = HttpContext.Current.Request.Cookies["user"].Value;
                    var user = ent.Customers.FirstOrDefault(a => a.Email_Id == userName);
                    if (user != null)
                        name = user.FullName;
                }
                return name;
            }
        }
    }
}