using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RentalAdmin.Models
{
    [MetadataType(typeof( BlogMetaData))]
    public partial class Blog
    {
        public string getURl()
        {
            //string str = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia") + "/";str;// +
            return "/blog/"  + BlogID.ToString() + "/" + BlogSlug;
        }
     
    }
    public class BlogMetaData
    {
        [Display(Name ="slug")]

        [StringLength(maximumLength: 200, ErrorMessage = "{0} is {1}")]
        public string BlogSlug { get; set; }
        [Display(Name ="title")]
        [StringLength(maximumLength:200,ErrorMessage ="{0} is {1}")]
        public string BlogTitle { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string BlogBody { get; set; }
        public string BlogImageAddress { get; set; }
        [Display(Name = "subtitle seo")]
        [StringLength(maximumLength: 200, ErrorMessage = "{0} is {1}")]
        public string BlogSubTilte { get; set; }
        public string BlogImageAddressSmall { get; set; }
    }
}