using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.DTO
{
    public class VendorDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter valid Vendor name.")]
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Please enter valid mobile no. 10 digit.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Contact Number must be numeric")]
        //[RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$", ErrorMessage = "Invalid Phone number")]
        public string ContactNumber { get; set; }
        //[Required,EmailAddress]
        [Required(ErrorMessage = "Email Id is required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        public string EmailId { get; set; }
        public int LoginMaster_Id { get; set; }
        //[Required]
        //public string Password { get; set; }
        //[Required,Compare("Password")]
        //public string ConfirmPassword { get; set; }
        public DateTime RegistrationDate { get; set; }
        //[Required]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Please enter valid city name.")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please enter valid Password.")]
        public string Password { get; set; }
        public string CompanyName { get; set; }
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Company Name")]
        public string LegalCompanyName { get; set; }
        public string BusinessFilingStatus { get; set; }
        [RegularExpression("([A-Z0-9 .&'-]+)", ErrorMessage = "Enter only alphabets and number of PAN Number")]
        public string PAN_No { get; set; }
        public string RegisteredAddress { get; set; }
        public string OperatingAddress { get; set; }
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Pay to Name")]
        public string PayToName { get; set; }
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Bank Name")]
        public string BankName { get; set; }
        [RegularExpression("^[0-9]{10,15}$", ErrorMessage = "Account Number must be numeric")]
        public string AccountNumber { get; set; }
        [RegularExpression("([A-Z0-9 .&'-]+)", ErrorMessage = "Enter only alphabets and number of IFSC Code")]
        public string IFSC_Code { get; set; }
        public string BranchAddress { get; set; }
        public string PanCard { get; set; }
        public string AddressProof { get; set; }
        public string CancelCheque { get; set; }
        public string SignedDocument { get; set; }
        public string GovtCertificate { get; set; }
        public string FoodLicence { get; set; }
        public string BusinessDocumnet { get; set; }
        public string PanCardImageBase64 { get; set; }
        public string AddressProofImageBase64 { get; set; }
        public string CancelChequeImageBase64 { get; set; }
        public string SignedDocumentImageBase64 { get; set; }
        public string GovtCertificateImageBase64 { get; set; }
        public string FoodLicenceImageBase64 { get; set; }
        public string BusinessDocumnetImageBase64 { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase DocumentFile1 { get; set; }
        public HttpPostedFileBase DocumentFile2 { get; set; }
        public HttpPostedFileBase DocumentFile3 { get; set; }
        public HttpPostedFileBase DocumentFile4 { get; set; }
        public HttpPostedFileBase DocumentFile5 { get; set; }
        public HttpPostedFileBase DocumentFile6 { get; set; }
        public HttpPostedFileBase DocumentFile7 { get; set; }
        public SelectList BusinessStatuList { get; set; }
        [Required]
        public string State { get; set; }
        public SelectList StateList { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}