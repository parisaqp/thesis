using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public class EmailModel
    {
    }
    public class GeneralEmailModel //: RazorEngine.Templating.TemplateBase
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        // توی ایمیل های ثابت می توان در خود لای آوت پری هدر را نوشت و دیگر از این فیلد استفاده نکرد.
        // بستگی به قالب ایمیل دارد که بخواهد از این استفاده کند یا خیر
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "Name cannot be longer than 40 characters.")]
        [System.Web.Mvc.AllowHtml]
        public string EmailPreHearder { get; set; }
        public string UserName { get; set; }
        public string Goodbye { get; set; }
        /// <summary>
        /// گاهی ممکن است آدرس کامل باشد گاهی هم می تواند پارامتر ها را فقط داشته باشه 
        /// 4 example: from="dm",
        /// </summary>
        public string UnsubscribeUrl { get; set; }

        public string LinkUrl { get; set; }
        public string LinkTxt { get; set; }

    }

}