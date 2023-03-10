using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Utilities
{
    public class FileOperation
    {

        public static string UploadImage(HttpPostedFileBase File,string Images)
        {
            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" };
            return UploadFile(File, "Images", allowedExtensions);
        }

        private static string UploadFile(HttpPostedFileBase File, string Images, string[] allowedExtensions)
        {
            DateTime dt = DateTime.Now;
            //var allowedExtensions = new[] { ".pdf" };

            string savedFileName = "" + dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + dt.Second + dt.Millisecond + dt.Millisecond + dt.Second + dt.Millisecond + dt.Millisecond + dt.Millisecond + dt.Second + dt.Millisecond + dt.Millisecond + dt.Millisecond;

            string ImageName = Path.GetFileName(File.FileName);


            string ext = Path.GetExtension(ImageName);

            string physicalPath = System.Web.HttpContext.Current.Server.MapPath("~/" + Images + "/" + savedFileName + ext);

            if (!allowedExtensions.Contains(ext))
            {
                return "not allowed";
            }
            // save image in folder
            File.SaveAs(physicalPath);
            return savedFileName + ext;
        }

        public static bool DeleteFile(string fileNameWithPath)
        {

            System.IO.FileInfo file = new System.IO.FileInfo(fileNameWithPath);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            return false;
        }

        public static string UploadFileWithBase64(string folderName, string imageName, string base64Image, string[] extensions)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            string[] arr = imageName.Split('.');
            string ext = "." + arr[1];
            if (!extensions.Contains(ext))
                return "not allowed";
            string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Guid.NewGuid().ToString();
            var filePath = HttpContext.Current.Server.MapPath("~/" + folderName);
            File.WriteAllBytes(filePath + "/" + fileName + ext, imageBytes);
            return fileName + ext;
        }

        public static string CheckFileSize(HttpPostedFileBase DocumentFile)
        {
            //HttpPostedFileBase DocumentFile;
            string message,filename = "";
            if (DocumentFile != null)
            {
                if (DocumentFile.ContentLength > 2 * 1024 * 1024)
                {
                    message = "Image should not succeed 2 mb";
                    return message;
                }
                var uploadResult = FileOperation.UploadImage(DocumentFile, "Images");
                if (uploadResult == "not allowed")
                {
                    message = "Only .jpg,.jpeg,.png and .gif files are allowed";
                    return message;
                }
                filename = uploadResult;
            }
            return filename;
        }
    }
}