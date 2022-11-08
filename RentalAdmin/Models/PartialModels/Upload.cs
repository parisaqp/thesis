using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public partial class Upload
    {
        public string GetPublicUrl()
        {
            string temp = BasicAddressVirtual + UploadAddressOffSet;
            if (!string.IsNullOrEmpty(temp))
            {
                temp = temp.Replace("\\", "/");
                //temp = temp.Replace("//", "/");
                //temp = temp.Replace("//", "/");
            }
            return temp;
        }
        public int GetUploadHeight()
        {
            if (this.UploadHeight > 1)
            {
                return UploadHeight;
            }
            else
            {
                if (this.UploadType == 3)
                {
                    return 720;
                }
                else
                {
                    return 360;
                }
            }
        }
        public int GetUploadWidth()
        {
            if (this.UploadWidth > 1)
            {
                return UploadWidth;
            }
            else
            {
                if (this.UploadType == 3)
                {
                    return 1080;
                }
                else
                {
                    return 540;
                }
            }
        }
    }
}