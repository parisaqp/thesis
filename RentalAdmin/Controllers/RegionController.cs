using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class RegionController : Controller
    {
        // GET: Region
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
    }
}