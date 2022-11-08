using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RentalAdmin.Models;

namespace RentalAdmin.Controllers
{

    [Authorize(Roles = "admin")]
    public class dashboardController : Controller
    {
        private RentalEntities db = new RentalEntities();
        // GET: dashboard
        public ActionResult Index()
        {
          var res=  db.ContactUs.OrderByDescending(a => a.ContactUsDate).Take(12).ToList();
            return View(res);
        }
    }
}