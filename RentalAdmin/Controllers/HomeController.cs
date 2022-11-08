using PagedList;
using RentalAdmin.helper;
using RentalAdmin.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace RentalAdmin.Controllers
{
    //[helper.HeaderAllUserRequest]
    public class HomeController : Controller
    {
        
        private readonly string allregion = "All Areas";
        private RentalEntities db = new RentalEntities();
        [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.VillaProperties = db.Properties
                .Include(a => a.Area).Include(a => a.PropertyGalleries)
                .Where(a => a.IsExpired == false && a.PropertyTypeID == 4)
                .OrderByDescending(a => a.PropertyOurPoint).ThenByDescending(a => a.InsertDatetime).Take(9).ToList();

            model.TopProperties = db.Properties
                .Include(a => a.Area).Include(a => a.PropertyGalleries)
                .Where(a => a.IsExpired == false).OrderByDescending(a => a.PropertyOurPoint).ThenByDescending(a => a.InsertDatetime).Take(9).ToList();
            model.NewProperties = db.Properties
                .Include(a => a.Area).Include(a => a.PropertyGalleries)
                .Where(a => a.IsExpired == false).OrderByDescending(a => a.InsertDatetime).Take(9).ToList();

            model.LuxuryProperties = db.Properties
               .Include(a => a.Area).Include(a => a.PropertyGalleries)
               .Where(a => a.IsExpired == false && a.PropertyPrice > 3200).OrderByDescending(a => a.InsertDatetime).Take(9).ToList();
            model.FurinshedProperties = db.Properties
               .Include(a => a.Area).Include(a => a.PropertyGalleries)
               .Where(a => a.IsExpired == false && a.PropertyTypeID == 1 && a.PropertyIsFernished == 2)
                 .OrderByDescending(a => a.PropertyOurPoint).ThenByDescending(a => a.InsertDatetime).Take(9).ToList();
            model.ShortTermProperties = db.Properties
               .Include(a => a.Area).Include(a => a.PropertyGalleries)
               .Where(a => a.IsExpired == false  && a.PropertyIsFernished == 2 &&a.PropertyIsShortTerm==true)
                 .OrderByDescending(a => a.PropertyOurPoint).ThenByDescending(a => a.InsertDatetime).Take(9).ToList();

            var TourType = db.BlogTypes.Where(a => a.TypeName == "Tour").FirstOrDefault();
            var BlogDefault= db.BlogTypes.Where(a => a.TypeName == "BlogDefault").FirstOrDefault();
            model.TopTour = db.ComplexBlogs.Include(a => a.Blog).Where(a => a.BlogTypeID == TourType.BlogTypeID)
                .Where(a => a.Blog.BlogIsDraft == false).OrderBy(a => a.Blog.BlogOrder).Take(4).ToList();
            model.TopBlogs = db.ComplexBlogs.Include(a => a.Blog).Where(a => a.BlogTypeID == BlogDefault.BlogTypeID)
                .Where(a => a.Blog.BlogIsDraft == false).Select(a=>a.Blog).OrderBy(a => a.BlogOrder).Take(6).ToList();
            //db.Blogs.Where(a => a.BlogIsDraft == false).OrderBy(a => a.InsertDateTime).Take(6).ToList();
            model.topRegions = db.Areas.OrderBy(a => a.AreaOrder).Take(6).ToList();
            int _difYear = DateTime.Now.Year - 2020;
            DateTime t1 = DateTime.Now.AddYears(-_difYear).AddDays(-4);
            DateTime t2 = DateTime.Now.AddYears(-_difYear).AddDays(+3);
            model.BirthDayEmbassy = db.Embassies.Where(a => a.EmbassyBirthDate >= t1 && a.EmbassyBirthDate <= t2).OrderBy(a => a.EmbassyBirthDate).FirstOrDefault();

            return View(model);
        }

        public ActionResult SearchPage(Models.PageViewModel.SearchViewModel model, int page = 1)
        {
            bool isOurCode = false;
            //if(model.ex)
            var query = db.Properties.Where(a => a.IsExpired == false);

            //var lastYear = DateTime.Now.AddYears(-1);
            //query = query.Where(a => a.InsertDatetime > lastYear);
            if (!string.IsNullOrEmpty(model.AreaName) && model.AreaName != allregion)
            {
                model.AreaName = model.AreaName.Trim();
                int pid = 0;
                var isInt = int.TryParse(model.AreaName, out pid);
                if (isInt)
                {
                    query = query.Where(a => a.PropertyOurCode == pid);
                    isOurCode = true;
                }
                else
                {
                    System.Collections.Generic.List<int> areaList = new System.Collections.Generic.List<int>();
                    string areaNameToDo = model.AreaName;
                    Area AreaTemp = null;
                    if (!model.AreaName.Contains(','))
                    {
                        AreaTemp = db.Areas.Where(a => a.AreaName == model.AreaName).FirstOrDefault();
                        areaList.Add(AreaTemp.AreaID);
                    }
                    else
                    {
                        var areaListString = model.AreaName.Split(',');
                        if (areaListString != null && areaListString.Count() > 0)
                        {
                            foreach (var item in areaListString)
                            {
                                string arenameTemp = item.Trim();
                                if (!string.IsNullOrEmpty(arenameTemp))
                                {
                                    AreaTemp = db.Areas.Where(a => a.AreaName == arenameTemp).FirstOrDefault();
                                    if (AreaTemp != null)
                                    {
                                        areaList.Add(AreaTemp.AreaID);
                                    }
                                }
                            }
                        }
                    }

                    query = query.Where(a => areaList.Any(c => c == a.AreaID));

                }


            }
            if (!isOurCode)
            {
                if (model.pricemin != null && model.pricemin.Count > 0)
                {
                    if (model.pricemin.Any(a => a.HasValue))
                    {
                        int priceminselected = model.pricemin.Where(a => a.HasValue).Max(a => a.Value);
                        query = query.Where(a => a.PropertyPrice >= priceminselected);
                        model.pricemin = new System.Collections.Generic.List<int?> { priceminselected };
                    }
                    else
                    {
                        model.pricemin = new System.Collections.Generic.List<int?> { 0 };
                    }
                }
                if (model.pricemax != null)
                {
                    query = query.Where(a => a.PropertyPrice < model.pricemax);
                }
                if (model.PropertyFurnisheType != null && model.PropertyFurnisheType.Count > 0 && model.PropertyFurnisheType.Count < 3)
                {

                    var tempArray3 = model.PropertyFurnisheType.ToList();
                    query = query.Where(a => tempArray3.Any(c => c == a.PropertyIsFernished));
                }

                if (model.PropertyTypeIDList != null && model.PropertyTypeIDList.Count > 0)
                {
                    if (model.PropertyTypeIDList[0] != null && model.PropertyTypeIDList[0] != 0)
                    {
                        var tempArray = model.PropertyTypeIDList.ToList();
                        query = query.Where(a => tempArray.Any(c => c == a.PropertyTypeID));
                    }
                }
                if (model.PropertyBedroomList != null && model.PropertyBedroomList.Count > 0)
                {

                    var tempArray2 = model.PropertyBedroomList.Where(a => a > 0).ToList();
                    if (tempArray2 != null && tempArray2.Count > 0)
                    {
                        if (model.PropertyBedroomList.Any(a => a > 3))
                        {
                            query = query.Where(a => tempArray2.Any(c => c == a.PropertyRoom) || a.PropertyRoom > 3);
                        }
                        else
                        {
                            query = query.Where(a => tempArray2.Any(c => c == a.PropertyRoom));
                        }
                    }

                }


                if (model.IsSpecial != null)
                {
                    query = query.Where(a => a.IsSpecial == model.IsSpecial);
                }
                if (model.PropertyAge != null)
                {
                    query = query.Where(a => a.PropertyAge <= model.PropertyAge);
                }
                if (model.PropertyAllFloor != null)
                {
                    query = query.Where(a => a.PropertyAllFloor == model.PropertyAllFloor);
                }
                if (model.PropertyFloor != null)
                {
                    query = query.Where(a => a.PropertyFloor == model.PropertyFloor);
                }


                if (model.PropertyHasGaurd != null)
                {
                    query = query.Where(a => a.PropertyHasGaurd == model.PropertyHasGaurd);
                }
                if (model.PropertyHasGym != null)
                {
                    query = query.Where(a => a.PropertyHasGym == model.PropertyHasGym);
                }
                if (model.PropertyHasLobby != null)
                {
                    query = query.Where(a => a.PropertyHasLobby == model.PropertyHasLobby);
                }
                if (model.PropertyHasLobbyMan != null)
                {
                    query = query.Where(a => a.PropertyHasLobbyMan == model.PropertyHasLobbyMan);
                }


                if (model.PropertyJacuzzi != null)
                {
                    query = query.Where(a => a.PropertyJacuzzi == model.PropertyJacuzzi);
                }

                if (model.PropertyParkingNumber != null)
                {
                    query = query.Where(a => a.PropertyParkingNumber == model.PropertyParkingNumber);
                }
                if (model.PropertyPool != null)
                {
                    query = query.Where(a => a.PropertyPool == model.PropertyPool);
                }
                if (User.Identity.IsAuthenticated)
                {
                    var user = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                    var accesslevel = db.AspNetUserRoles.Where(a => a.UserId == user.Id).FirstOrDefault();
                    if (accesslevel != null)
                    {
                        if (model.PropertyOurCode != null)
                        {
                            query = query.Where(a => a.PropertyOurCode == model.PropertyOurCode);
                        }
                        if (model.IsExpired != null)
                        {
                            query = query.Where(a => a.IsExpired == model.IsExpired);
                        }
                        if (model.InsertDatetime != null)
                        {
                            query = query.Where(a => a.InsertDatetime > model.InsertDatetime);
                        }
                    }
                }

                if (model.PropertyRoofGarden != null)
                {
                    query = query.Where(a => a.PropertyRoofGarden == model.PropertyRoofGarden);
                }
                //if (model.PropertyRoom != null)
                //{
                //    query = query.Where(a => a.PropertyRoom == model.PropertyRoom);
                //}
                if (model.PropertySauna != null)
                {
                    query = query.Where(a => a.PropertySauna == model.PropertySauna);
                }
                if (model.PropertyIsShortTerm != null)
                {
                    query = query.Where(a => a.PropertyIsShortTerm == model.PropertyIsShortTerm);
                }
                else
                {
                    query = query.Where(a => a.PropertyIsShortTerm == false);
                }
                if (model.PropertySpace != null)
                {
                    query = query.Where(a => a.PropertySpace == model.PropertySpace);
                }


                if (model.PropertyUnitsPerFloor != null)
                {
                    query = query.Where(a => a.PropertyUnitsPerFloor == model.PropertyUnitsPerFloor);
                }
            }


            //query=query.Where(a=>a.PropertyOurCode==model.PropertyOurCode).Take(50)
            model.properties = query.OrderByDescending(a => a.PropertyOurCode).ToPagedList(page, 12);
            return View(model);
        }

        [ActionName("rent-short-term-apartment-in-tehran")]
        [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult Short_term(int page = 1)
        {
            var model = db.Properties.Where(a => a.IsExpired == false && a.PropertyIsShortTerm == true)
                 .OrderBy(a => a.PropertyPriceShortMonthly).ToPagedList(page, 12);
            return View("~/Views/Home/Short_term.cshtml", model);
        }
        [ActionName("luxury-apartments-in-tehran")]
        [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult luxury(int page = 1)
        {
            var model = db.Properties.Where(a => a.IsExpired == false && a.PropertyIsShortTerm == false&&
            a.PropertyPrice>2100)
                 .OrderByDescending(a => a.PropertyOurPoint).ToPagedList(page, 12);
            return View("~/Views/Home/luxury.cshtml", model);
        }
        [ActionName("Tour-In-Iran")]
       // [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult Tour(int page = 1)
        {
            var TourType = db.BlogTypes.Where(a => a.TypeName == "Tour").FirstOrDefault();
            var blogs = db.ComplexBlogs.Include(a => a.Blog).Where(a => a.BlogTypeID == TourType.BlogTypeID)
                .Where(a => a.Blog.BlogIsDraft == false).OrderBy(a => a.Blog.BlogOrder).ToPagedList(page, 16);
            //var model = db.Blogs.Include(a=>a.ComplexBlogs).Where(a => a.BlogIsDraft == false).OrderBy(a => a.BlogOrder).OrderByDescending(a => a.InsertDateTime).ToPagedList(page, 16);
            return View("~/Views/Home/Tour.cshtml", blogs);
        }
        [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult RegionsPage()
        {
            var topRegions = db.Areas.Where(a => a.AreaOrder < 99).OrderByDescending(a => (a.Properties.Count() * 2) - a.AreaOrder).ToList();
            System.Collections.Generic.List<AreaWhitRelated> model = new System.Collections.Generic.List<AreaWhitRelated>();
            if (topRegions != null && topRegions.Count > 0)
            {
                logic.RegionManage rm = new logic.RegionManage(db);
                foreach (var item in topRegions)
                {
                    model.Add(rm.GetAreaWhitRelated(item));
                }
            }
            return View(model);
        }
        [OutputCache(Duration = 43200, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult EmbassiesPage()
        {
            var topEmbassies = db.Embassies.OrderBy(a => a.EmbassyName).ToList();
            return View(topEmbassies);
        }
         
        [OutputCache(Duration = 43200, VaryByParam = "*", Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult EmbassyPage(long id, string FriendlyUrl, int page = 1)
        {
            Models.PageViewModel.EmbassyViewModel model = new Models.PageViewModel.EmbassyViewModel();
            int pageSize = 12;
            var theEmbassy = db.Embassies.Include(a => a.Area).Where(a => a.EmbassyID == id).FirstOrDefault();
            if (theEmbassy != null)
            {
                if (theEmbassy.getURl() != Request.Url.AbsolutePath)
                {
                    return RedirectPermanent(theEmbassy.getURl());

                }
            }
            model.embassy = theEmbassy;
            model.area = theEmbassy.Area;
            model.map = db.Maps.Include(a => a.UploadBig).Where(a => a.MapID == theEmbassy.MapID).FirstOrDefault();

            logic.RegionManage rm = new logic.RegionManage(db);
            model.areaWhitRelated = rm.GetAreaWhitRelated(model.area);
            System.Collections.Generic.List<int> AreaList = new System.Collections.Generic.List<int>();
            AreaList.Add(model.area.AreaID);
            if (model.areaWhitRelated != null &&
                model.areaWhitRelated.AreaRelatedList != null &&
                model.areaWhitRelated.AreaRelatedList.Count > 0)
            {
                AreaList.AddRange(model.areaWhitRelated.AreaRelatedList.Select(a => a.AreaID).ToList());
            }
            var topProperties = db.Properties.Where(a => a.IsExpired == false && AreaList.Any(c => c == a.AreaID)).OrderByDescending(a => a.PropertyOurPoint).ToPagedList(page, pageSize);

            model.properties = topProperties;
            int _difYear=  DateTime.Now.Year - 2020;
            DateTime t1 = DateTime.Now.AddYears(-_difYear).AddDays(-4);
            DateTime t2 = DateTime.Now.AddYears(-_difYear).AddDays(+3);
            
            if (model.embassy.EmbassyBirthDate > t1 && model.embassy.EmbassyBirthDate < t2)
            {
                model.isCelebrate = true;
            }
            return View(model);
        }

        //[OutputCache(Duration = 21600, VaryByParam = "*",Location =System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult PropertyPage(long id, string FriendlyUrl)
        {
            var model = new Models.PageViewModel.PropertyViewModel();
            var theProperty = db.Properties.Include(a => a.Area).Include(a => a.PropertyType).Where(a => a.PropertyID == id).FirstOrDefault();
            if (theProperty != null)
            {
                if (theProperty.getURl() != Request.Url.AbsolutePath)
                {
                    return RedirectPermanent(theProperty.getURl());
                }
            }
            model.property = theProperty;
            model.region = theProperty.Area;
            var ebmassiesProperty = db.Embassies.Where(a => a.AreaID == theProperty.AreaID).ToList();
            model.allEmbassy = ebmassiesProperty;
            var metrosProperty = db.Metroes.Where(a => a.AreaID == theProperty.AreaID).ToList();
            model.allMetro = metrosProperty;
            model.allPic = db.PropertyGalleries.Include(a => a.Upload).Where(a => a.PropertyID == id && a.PropertyGalleryOrder != 99).OrderBy(a => a.PropertyGalleryOrder).ToList();
            model.map = db.Maps.Include(a => a.UploadBig).Include(a => a.UploadSmall).Where(a => a.MapID == theProperty.MapID).FirstOrDefault();
            model.propertyType = theProperty.PropertyType;
            logic.RegionManage rm = new logic.RegionManage(db);
            model.regionWithRelated = rm.GetAreaWhitRelated(model.region);
            try
            {
                long modelMinprice = model.minprice;
                long modelMaxprice = model.maxprice;
                int modelminbed = 0;
                int modelmaxbed = 0;
                logic.PropertyManager propertyManager = new logic.PropertyManager(db);
                model.sameProperties = propertyManager.getSameProperty(theProperty, out modelMinprice, out modelMaxprice
                    , out modelminbed, out modelmaxbed);
                model.samePropertiesNearNeighborhood= propertyManager.getSamePropertyNearNeighborhood(theProperty,model.regionWithRelated
                    ,  modelMinprice,  modelMaxprice
                    , modelminbed,  modelmaxbed);
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [OutputCache(Duration = 43200, VaryByParam = "*", Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult RegionPage1(long id, string FriendlyUrl, int page = 1)
        {
            Models.PageViewModel.RegionViewModel model = new Models.PageViewModel.RegionViewModel();
            int pageSize = 12;
            var theRegion = db.Areas.Where(a => a.AreaID == id).FirstOrDefault();
            if (theRegion != null)
            {
                if (theRegion.getURl() != Request.Url.AbsolutePath)
                {
                    return RedirectPermanent(theRegion.getURl());
                }
            }
            var topProperties = db.Properties.Where(a => a.IsExpired == false && a.AreaID == theRegion.AreaID).OrderByDescending(a => a.PropertyOurPoint)
                .ToPagedList(page, pageSize);
            model.region = theRegion;
            model.properties = topProperties;
            model.allEmbassy = db.Embassies.Where(a => a.AreaID == id).OrderBy(a => a.EmbassyName).ToList();
            model.metroCount = db.Metroes.Count(a => a.AreaID == id);
            model.propertiesCount = db.Properties.Count(a => a.IsExpired == false && a.AreaID == id);
            model.embassyCount = 0;
            logic.RegionManage rm = new logic.RegionManage(db);
            model.regionWithRelated = rm.GetAreaWhitRelated(model.region);

            if (model.allEmbassy != null && model.allEmbassy.Count > 0)
            {
                model.embassyCount = model.allEmbassy.Count;
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult BlogIndex(int page = 1)
        {
            var TourType = db.BlogTypes.Where(a => a.TypeName == "BlogDefault").FirstOrDefault();
            var blogs = db.ComplexBlogs.Include(a => a.Blog).Where(a => a.BlogTypeID == TourType.BlogTypeID)
                .Where(a => a.Blog.BlogIsDraft == false).OrderBy(a => a.Blog.BlogOrder).ToPagedList(page, 16);
            //var model= db.Blogs.Where(a => a.BlogIsDraft == false).OrderBy(a => a.BlogOrder).OrderByDescending(a => a.InsertDateTime).ToPagedList(page,16);
            return View(blogs);
        }
        public ActionResult BlogPage()
        {
            return View();
        }
        [OutputCache(Duration = 21600, VaryByParam = "*", Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult OneBlogPage(long id, string FriendlyUrl)
        {
            bool CheckSlug = true;
            var model = new Models.PageViewModel.BlogViewModel();
            var theBlog = db.Blogs.Include(a => a.DisplayLanguage).Where(a => a.BlogID == id).FirstOrDefault();
            if (theBlog == null)
            {
                theBlog = db.Blogs.Where(a => a.BlogSlug == FriendlyUrl).FirstOrDefault();
                CheckSlug = false;
            }
            var _complexBlog = db.ComplexBlogs.Where(a => a.BlogID == theBlog.BlogID).FirstOrDefault();
            if (_complexBlog != null)
            {
               var _ComplexRelatedBlogs=  db.ComplexBlogs.Include(a => a.Blog).Where(a => a.BlogTypeID == _complexBlog.BlogTypeID
               && a.Blog.BlogIsDraft == false && a.Blog.DisplayLanguageID == theBlog.DisplayLanguageID
                  && a.ComplexBlogID != _complexBlog.ComplexBlogID).ToList();
                if(_ComplexRelatedBlogs!=null)
                {
                    model.RelatedBlogs= _ComplexRelatedBlogs.Select(a => a.Blog).Take(3).ToList();
                }
            }
            else
            {
                model.RelatedBlogs = db.Blogs.Include(a => a.DisplayLanguage)
               .Where(a => a.BlogID != id && a.BlogIsDraft == false && a.DisplayLanguageID == theBlog.DisplayLanguageID
               && a.IsBlogNormal == theBlog.IsBlogNormal
               ).Take(3).ToList();
            }
          
           
            
            model.LanguageDisplay = theBlog.DisplayLanguage.DisplayLanguageStandardName;

            //if (theBlog != null&& CheckSlug)
            //{
            //    if (theBlog.getURl() != Request.Url.AbsolutePath)
            //    {
            //        return RedirectPermanent(theBlog.getURl());

            //    }
            //}
            if (id == 6)
            {
                var topRegions = db.Areas.Where(a => a.AreaOrder < 99).OrderBy(a => a.AreaOrder).ToList();
                System.Collections.Generic.List<AreaWhitRelated> modelarea = new System.Collections.Generic.List<AreaWhitRelated>();
                if (topRegions != null && topRegions.Count > 0)
                {
                    foreach (var item in topRegions)
                    {
                        AreaWhitRelated temp = new AreaWhitRelated(item);
                        var allrelated = db.AreaRelations.Include(a => a.Area).Include(a => a.Area1).Where(a => a.AreaID1 == item.AreaID || a.AreaID2 == item.AreaID).ToList();
                        temp.AreaRelated = "";
                        if (allrelated != null && allrelated.Count > 0)
                        {
                            System.Collections.Generic.HashSet<string> areaname = new System.Collections.Generic.HashSet<string>();
                            foreach (var therelated in allrelated)
                            {
                                areaname.Add(therelated.Area.AreaName);
                                areaname.Add(therelated.Area1.AreaName);
                            }
                            if (areaname.Any(a => a == item.AreaName))
                            {
                                areaname.Remove(item.AreaName);
                            }
                            temp.AreaRelated = string.Join(", ", areaname.ToList());
                        }
                        modelarea.Add(temp);
                    }
                }
                model.regions = modelarea;
            }
            model.theBlog = theBlog;
            return View(model);
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [GoogleRecaptchaActionFilter]
        [ActionName("ContactUs")]
        public ActionResult ContactPost(string messagestr, string emailFrom, string name, string returnurl)
        {
            ViewBag.Message = "Your contact page.";
            //helper.EmailManager em = new helper.EmailManager();
            //em.sendEmail(messagestr, emailFrom, name);
            var contactus = new Models.ContactU()
            {
                ContactUsEmail = emailFrom,
                ContactUsMessage = messagestr,
                ContactUsName = name,
                ContactUsDate = DateTime.Now
            };
            db.ContactUs.Add(contactus);
            db.SaveChanges();
            TempData["messagetoshow"] = "thank you. your message has been sent. we will reply soon.";
            if (string.IsNullOrEmpty(returnurl))
            {
                return RedirectToAction("index");
            }
            return Redirect(VirtualPathUtility.ToAbsolute("~" + returnurl));
        }
        public ActionResult Property()
        {

            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 86400, VaryByParam = "*", Location = System.Web.UI.OutputCacheLocation.Any)]
        public JsonResult getarea(string Prefix)
        {
            var AreaName = db.Areas.Where(a => a.AreaOrder < 99).OrderBy(a => a.AreaOrder).Take(16).Select(a => new { name = a.AreaName, id = a.AreaID }).ToList();
            //Note : you can bind same list from database 
            if (!string.IsNullOrEmpty(Prefix))
            {
                var temp = db.Areas.Where(a => a.AreaOrder < 99 && a.AreaName.Contains(Prefix)).ToList();
                if (temp != null && temp.Count > 0)
                {
                    AreaName = temp.OrderBy(a => a.AreaOrder).Take(6).Select(a => new { name = a.AreaName, id = a.AreaID }).ToList();
                }
            }
            AreaName.Add(new { name = allregion, id = -1 });
            AreaName.Reverse();
            return Json(AreaName, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}