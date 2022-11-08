using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace RentalAdmin.helper
{
    public class HeaderAllUserRequest:ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            RentalAdmin.Models.RentalEntities db = new Models.RentalEntities();
           var links= db.Areas.OrderBy(a => a.AreaOrder).Take(7).ToList();
            filterContext.Controller.TempData["FooterLink"]  = 
                links.Select(a => new RentalAdmin.Models.PageViewModel.SimpleLink { link=a.getURl(),txt=a.getName()}).ToList();
        }
    }
}