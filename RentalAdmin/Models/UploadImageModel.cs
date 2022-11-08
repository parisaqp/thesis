using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public class UploadImageModel
    {
        public Upload UploadOb { get; set; }
        public string htmlName { get; set; }
        public string htmlId { get; set; }
        public string obname { get; set; }
        public string idNameHidden { get; set; }
    }
}