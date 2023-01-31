using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KisanKiDukan.Models.ViewModels;
using KisanKiDukan.Models.Domain;
using KisanKiDukan.Models.DTO;
using AutoMapper;
using Product = KisanKiDukan.Models.Domain.Product;

namespace KisanKiDukan.Infrastructure
{
    public class AutomapperConfig
    {
        public static void MapIt()
        {
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<ProductModel, Product>();

            Mapper.CreateMap<PremiumAmount, premiumMembershipAmount>();
            Mapper.CreateMap<premiumMembershipAmount, PremiumAmount>();

            Mapper.CreateMap<CategoryModel, Category>();
            Mapper.CreateMap<Category, CategoryModel>();

            Mapper.CreateMap<CategoryModel, SubCategory>();
            Mapper.CreateMap<SubCategory, CategoryModel>();

            //Mapper.CreateMap<GalleryModel, Gallery>();
            //Mapper.CreateMap<Gallery, GalleryModel>();

            //Mapper.CreateMap<City, CityMasterDTO>();

            Mapper.CreateMap<Customer, CustomerDTO>();
            Mapper.CreateMap<CustomerDTO, Customer>();

            Mapper.CreateMap<ContentPage, ContentPageModel>();
            Mapper.CreateMap<ContentPageModel, ContentPage>();

            Mapper.CreateMap<AddMetricsModel, Metric>();
            Mapper.CreateMap<Metric, AddMetricsModel>();

            Mapper.CreateMap<BannerImage, BannerAddDTO>();
            Mapper.CreateMap<BannerAddDTO, BannerImage>();

            Mapper.CreateMap<AddLocationDTO, DeliveryLocation>();
            Mapper.CreateMap<DeliveryLocation, AddLocationDTO>();

            Mapper.CreateMap<DeliveryOption, DeliveryOptionDTO>();
            Mapper.CreateMap<DeliveryOptionDTO, DeliveryOption>();

            Mapper.CreateMap<ProductBrand, BrandDTO>();
            Mapper.CreateMap<BrandDTO, ProductBrand>();

            Mapper.CreateMap<Product_Availability, PAvailDTO>();
            Mapper.CreateMap<PAvailDTO, Product_Availability>();

            Mapper.CreateMap<Vendor, VendorDTO>();
            Mapper.CreateMap<VendorDTO, Vendor>();

            Mapper.CreateMap<Vendor, VendorBusinessDetailsDTO>();
            Mapper.CreateMap<VendorBusinessDetailsDTO, Vendor>();

            Mapper.CreateMap<Vendor, VendorBankAcDetailsDTO>();
            Mapper.CreateMap<VendorBankAcDetailsDTO, Vendor>();

            Mapper.CreateMap<DeliveryPincode, DeliveryPincodeDTO>();
            Mapper.CreateMap<DeliveryPincodeDTO, DeliveryPincode>();

            Mapper.CreateMap<DeliveryChargeMasterDTO, DeliveryChargeMaster>();
            Mapper.CreateMap<DeliveryChargeMaster, DeliveryChargeMasterDTO>();

            Mapper.CreateMap<DeliveryTimeSlot, DeliveryTimeSlotDTO>();
            Mapper.CreateMap<DeliveryTimeSlotDTO, DeliveryTimeSlot>();

            //Mapper.CreateMap<Store, StoreDTO>();
            //Mapper.CreateMap<StoreDTO, Store>();

            //Mapper.CreateMap<DCMasterEntry, DCMasterDTO>();
            //Mapper.CreateMap<DCMasterDTO, DCMasterEntry>();

            //Mapper.CreateMap<DebitCreditNote, Debit_CreditDTO>();
            //Mapper.CreateMap<Debit_CreditDTO, DebitCreditNote>();

            //Mapper.CreateMap<PurchaseOrderDetail, ProductData>();
            //Mapper.CreateMap<ProductData, PurchaseOrderDetail>();

            Mapper.CreateMap<Membership, MembershipDTO>();
            Mapper.CreateMap<MembershipDTO, Membership>();


            Mapper.CreateMap<promotionalbanner, promotionalbannerDTO>(); //Get
            Mapper.CreateMap<promotionalbannerDTO, promotionalbanner>(); //Set

            Mapper.CreateMap<DownPyment, DownPymentDTO>();
            Mapper.CreateMap<DownPymentDTO, DownPyment>();

            //Mapper.CreateMap<TypeOfVehicle, TypeOfVehiclesDTO>();
            //Mapper.CreateMap<TypeOfVehiclesDTO, TypeOfVehicle>();

            //Mapper.CreateMap<YearVehicle, YearVehiclesDTO>();
            //Mapper.CreateMap<YearVehiclesDTO, YearVehicle>();

            Mapper.CreateMap<State, StateModel>();
            Mapper.CreateMap<StateModel, State>();

            Mapper.CreateMap<City, CityModel>();
            Mapper.CreateMap<CityModel, City>();

            Mapper.CreateMap<BlogMaster, BlogMasterDTO>();
            Mapper.CreateMap<BlogMasterDTO, BlogMaster>();

            Mapper.CreateMap<ADCategoryModel, ADCategory>();
            Mapper.CreateMap<ADCategory, ADCategoryModel>();

            Mapper.CreateMap<ADCategoryModel, ADSubCategory>();
            Mapper.CreateMap<ADSubCategory, ADCategoryModel>();

            Mapper.CreateMap<ADCategory, ADCategoryDTO>();
            Mapper.CreateMap<ADCategoryDTO, ADCategory>();

            Mapper.CreateMap<ADSubCategory, ADSubCategoryDTO>();
            Mapper.CreateMap<ADSubCategoryDTO, ADSubCategory>();

            Mapper.CreateMap<ADProduct, ADProductModel>();
            Mapper.CreateMap<ADProductModel, ADProduct>();
        }
    }
}