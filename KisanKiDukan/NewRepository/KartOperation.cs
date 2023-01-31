using KisanKiDukan.Models.APIModels.RequstModels;
using KisanKiDukan.Models.APIModels.ResponseModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.NewRepository
{
    public class KartOperation
    {
        private DbEntities ent = new DbEntities();
        public AllKartDetailModel GetCart(int userId)
        {
            AllKartDetailModel rm = new AllKartDetailModel();
            var cart = ent.Karts.FirstOrDefault(a => a.Client_Id == userId);

            if (cart != null)
            {
                var userData = ent.Customers.Find(userId);
                if (userData.IsPremiumMember == true)
                {
                    var kartDetail = (from kart in ent.Karts
                                      join kd in ent.KartDetails on kart.Id equals kd.Kart_Id
                                      join product in ent.Products on kd.Product_Id equals product.Id
                                      join metric in ent.Metrics on kd.Metric_Id equals metric.Id
                                      where kart.Id == cart.Id
                                      select new CartDetailModel
                                      {
                                          CartDetail_Id = kd.Id,
                                          Metric_Id = kd.Metric_Id ?? 0,
                                          Weight = kd.Volume,
                                          ProductName = product.ProductName,
                                          Product_Id = product.Id,
                                          Quantity = kd.Quantity ?? 0,
                                          Metric = metric.Metrics,
                                          Price = product.PremiumAmount ?? product.Price ?? 0,
                                          ProductImage = product.ProductImage,
                                          Category_Id = product.Category_Id,
                                          VendorId = product.VendorId
                                      }).ToList();
                    foreach (var kd in kartDetail)
                    {
                        var variant = ent.Product_Availability.FirstOrDefault(a => a.Product_Id == kd.Product_Id
                        && a.Metrics_Id == kd.Metric_Id && a.Quantity == kd.Weight
                        );
                        if (variant != null)
                            kd.Price = kd.Price;
                    }
                }
                else
                {
                    var kartDetail = (from kart in ent.Karts
                                      join kd in ent.KartDetails on kart.Id equals kd.Kart_Id
                                      join product in ent.Products on kd.Product_Id equals product.Id
                                      join metric in ent.Metrics on kd.Metric_Id equals metric.Id
                                      where kart.Id == cart.Id
                                      select new CartDetailModel
                                      {
                                          CartDetail_Id = kd.Id,
                                          Metric_Id = kd.Metric_Id ?? 0,
                                          Weight = kd.Volume,
                                          ProductName = product.ProductName,
                                          Product_Id = product.Id,
                                          Quantity = kd.Quantity ?? 0,
                                          Metric = metric.Metrics,
                                          Price = product.OurPrice ?? product.Price ?? 0,
                                          ProductImage = product.ProductImage,
                                          Category_Id = product.Category_Id,
                                          VendorId = product.VendorId
                                      }).ToList();
                    foreach (var kd in kartDetail)
                    {
                        var variant = ent.Product_Availability.FirstOrDefault(a => a.Product_Id == kd.Product_Id
                        && a.Metrics_Id == kd.Metric_Id && a.Quantity == kd.Weight
                        );
                        if (variant != null)
                            kd.Price = variant.OurPrice ?? variant.Price;
                    }
                    rm.Kart_Id = cart.Id;
                    rm.KartDetail = kartDetail;
                }
            }
            return rm;
        }

        public CartReturnDTO GetCartCounts(int userId)
        {
            var rm = new CartReturnDTO { NOI = 0, Total = 0 };
            var data = GetCart(userId);
            if (data != null && data.KartDetail != null && data.KartDetail.Count > 0)
            {
                rm.NOI = data.KartDetail.Count;
                rm.Total = data.KartDetail.Sum(a => a.Quantity * a.Price);
            }
            return rm;
        }
    }
}