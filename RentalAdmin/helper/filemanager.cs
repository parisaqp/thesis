using System.IO;
using System.Web;
using System.Web.Hosting;
using RentalAdmin.Models;

namespace RentalAdmin.helper
{
    public static class filemanager
    {
        public static Upload saveFile(HttpPostedFileBase file,string fromsource,long? id=null,string userid=null)
        {
           string basfiletosave= System.Configuration.ConfigurationManager.AppSettings.Get("filesave");
            Upload up = new Upload();
            if (file != null && file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                byte selectedType = 1;// (byte)((fileName.Contains(".jpg") || fileName.Contains(".png") || fileName.Contains(".jpeg")) ? 1 : 2);
                string directoryToSave = "";
                switch (fromsource)
                {
                    case "property":
                        selectedType = 1;
                        //     redirectTo = "index";
                        directoryToSave = "/propertyImages";
                        break;
                    case "map":
                        //   redirectTo = "videos";
                        selectedType = 2;
                        directoryToSave = "/mapImages";
                        break;
                    case "area":
                        //   redirectTo = "videos";
                        selectedType = 2;
                        directoryToSave = "/areaImages";
                        break;
                    case "user":
                        //   redirectTo = "videos";
                        selectedType = 2;
                        directoryToSave = "/"+ userid;
                        break;
                }
                string folderPath = basfiletosave + "\\" + directoryToSave;
                if(id!=null)
                {
                    folderPath += "\\" + id.ToString();
                }
                if (!System.IO.Directory.Exists(basfiletosave+"\\" + directoryToSave))
                {
                    Directory.CreateDirectory(basfiletosave + "\\" + directoryToSave);
                }
                if (!System.IO.Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var path = Path.Combine(folderPath + "\\", fileName);
                int i = 0;
                bool saved = false;
                string finalUploadName = null;
                while (!saved && i < 100)
                {
                    if (finalUploadName == null||System.IO.File.Exists(path))
                    {
                        //System.Random rnd = new System.Random();
                        //int month = rnd.Next();
                        System.Guid sdssd =  System.Guid.NewGuid();
                        string temp = sdssd.ToString();
                        if (temp.Length > 32)
                            temp = temp.Substring(0, 30);
                        finalUploadName = temp;
                        i++;
                        string extention =System.IO.Path.GetExtension(fileName);
                        //string firstpart = System.IO.Path.GetFileName(fileName);
                        finalUploadName = finalUploadName + i.ToString() + extention;
                        path = Path.Combine(folderPath + "\\", finalUploadName);
                    }
                    else
                    {
                        saved = true;
                        file.SaveAs(path);
                    }


                }
                int UploadWidth = 1;
                int UploadHeight = 1;
                ImageDimensions(path, out UploadWidth, out UploadHeight);
                var uploadAddressOffSet = "/"
                        + directoryToSave;
                if(id!=null)
                {
                    uploadAddressOffSet += "/" + id.ToString();
                }
                uploadAddressOffSet += "/" + finalUploadName;
                up = new Upload()
                    {
                        UploadDate = System.DateTime.UtcNow,
                        UploadName = finalUploadName,
                        UploadType = selectedType,
                        UploadWidth = UploadWidth,
                        UploadHeight = UploadHeight,
                        UploadTime = 0,
                        UploadSize = 0,
                        UploadSourceIsOut = false,
                        BasicAddressPhysical = basfiletosave,
                        UploadAddressOffSet= uploadAddressOffSet,
                        BasicAddressVirtual = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia"),
                        UserName= userid
                };
                
            }
            return up;
        }
        public static Upload replaceFile(HttpPostedFileBase file,  Upload up)
        {

            if (file != null && file.ContentLength > 0)
            {
                bool deletefile = deleteFile(up);

              

                    //string basfiletosave = System.Configuration.ConfigurationManager.AppSettings.Get("filesave");
                    // replace file
                    file.SaveAs(@up.BasicAddressPhysical + @up.UploadAddressOffSet);
                    // get image details
                    int UploadWidth = 1;
                    int UploadHeight = 1;
                    ImageDimensions(up.BasicAddressPhysical + up.UploadAddressOffSet, out UploadWidth, out UploadHeight);
                    up.UploadWidth = UploadWidth;
                    up.UploadHeight = UploadHeight;
                    return up;
                
            }
            return up;
        }
        /// <summary>
        /// delete file if exist
        /// </summary>
        /// <param name="up"></param>
        /// <returns></returns>
        public static bool deleteFile(Upload up)
        {
            //string basfiletosave = System.Configuration.ConfigurationManager.AppSettings.Get("filesave");
            try
            {
                string filePath =  @up.BasicAddressPhysical + @up.UploadAddressOffSet;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch 
            {

               return false;
            }
            return true;
        }
        private static void ImageDimensions(string path, out int Width, out int Height)
        {
            path = System.Configuration.ConfigurationManager.AppSettings.Get("filesave") + "\\" + path;
            if (System.IO.File.Exists(path))
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                // you then access properties like so:
                Width = image.Width;
                Height = image.Height;
            }
            else
            {
                Width = 1;
                Height = 1;
            }

        }
    }
}