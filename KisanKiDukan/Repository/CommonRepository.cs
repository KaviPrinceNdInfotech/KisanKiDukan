using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Repository
{
    public class CommonRepository
    {
        DbEntities ent = new DbEntities();
        public string GetDepotUsername()
        {
            string invoiceNm = "DEPOT1";
            var data = ent.LoginMasters.OrderByDescending(a => a.Id).Where(a => a.Username.StartsWith(invoiceNm)).FirstOrDefault();
            if (data != null)
            {
                var lastInvoiceNm = data.Username;
                string iPart = lastInvoiceNm.Substring(5, lastInvoiceNm.Length - 5);
                int n = int.Parse(iPart) + 1;
                invoiceNm = "DEPOT" + n;
            }
            return invoiceNm;
        }

        //public string GetPoNumber()
        //{
        //    string userNm = "VRFMPO100";
        //    var data = ent.PurchaseOrders.OrderByDescending(a => a.Id).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var lastPONumber = data.PONumber;
        //        string iPart = lastPONumber.Substring(8, lastPONumber.Length - 8);
        //        int n = int.Parse(iPart) + 1;
        //        userNm = "VRFMPO" + n;
        //    }
        //    return userNm;
        //}

        public int GetDepotId()
        {
            int id = string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? 0 : int.Parse(HttpContext.Current.User.Identity.Name);
            var user = ent.LoginMasters.FirstOrDefault(a => a.Id == id);
            var depot = ent.Stores.Where(a => a.LoginMaster_Id == user.Id).FirstOrDefault();
            return depot.Id;
        }

        public int GetVendorId()
        {
            int id = string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? 0 : int.Parse(HttpContext.Current.User.Identity.Name);
            var user = ent.LoginMasters.FirstOrDefault(a => a.Id == id);
            var vendor = ent.Vendors.Where(a => a.LoginMaster_Id == user.Id).FirstOrDefault();
            return vendor.Id;
        }


        public string GetInvoiceNumber()
        {
            string invoiceNo = "VF1";
            var data = ent.Orders.OrderByDescending(a => a.Id).FirstOrDefault(a => !string.IsNullOrEmpty(a.Invoice_No));
            if (data != null)
            {
                var lastInvoiceNo = data.Invoice_No;
                string iPart = lastInvoiceNo.Substring(2, lastInvoiceNo.Length - 2);
                int n = int.Parse(iPart) + 1;
                invoiceNo = "VF" + n;
            }
            return invoiceNo;
        }
        //public string GetPoInvoiceNumber()
        //{
        //    string userNm = "VRNFRM1";
        //    var data = ent.PoInvoices.OrderByDescending(a => a.Id).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var lastPONumber = data.InvoiceNo;
        //        string iPart = lastPONumber.Substring(6, lastPONumber.Length - 6);
        //        int n = int.Parse(iPart) + 1;
        //        userNm = "VRNFRM" + n;
        //    }
        //    return userNm;
        //}

        //public string GetChallanNo()
        //{
        //    string challanNm = "VRNFRMCLN1";
        //    var data = ent.Purchases.OrderByDescending(a => a.Id).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var lastChallanNo = data.ChallanNo;
        //        string iPart = lastChallanNo.Substring(9, lastChallanNo.Length - 9);
        //        int n = int.Parse(iPart) + 1;
        //        challanNm = "VRNFRMCLN" + n;
        //    }
        //    return challanNm;
        //}
    }
}