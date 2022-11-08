using PagedList;
using RentalAdmin.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PropertiesController : Controller
    {
        private RentalEntities db = new RentalEntities();

        private void setviewbag(int? AreaID = null, int? PropertyTypeID = null, byte? PropertyIsFernished = null)
        {
            ViewBag.AreaID = new SelectList(db.Areas.OrderBy(a => a.AreaOrder), "AreaID", "AreaName", AreaID);
            ViewBag.PropertyTypeID = new SelectList(db.PropertyTypes, "PropertyTypeID", "PropertyTypeName", PropertyTypeID);
            ViewBag.PropertyIsFernishedvalue = new SelectList(StaticList.FurnishType.Select(a => new { PropertyIsFernished = a.Value, PropertyIsFernishedName = a.Text }).ToList(), "PropertyIsFernished", "PropertyIsFernishedName", PropertyIsFernished);
        }
        // GET: Properties
        public ActionResult Index(Models.PageViewModel.SearchViewModel model, int page = 1)
        {
            int pagechunk = 50;
            bool isOurCode = false;
            string allregion = "All Areas";
            var query = db.Properties.Include(a=>a.Area).Include(a => a.AspNetUser)
                .Include(a => a.Map).Include(a => a.PropertyType).Where(a => a.IsExpired == false|| a.IsExpired == true);

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
            model.properties = query.OrderByDescending(a => a.InsertDatetime).ToPagedList(page, pagechunk);
            return View(model);
            //return View(model);
        }

        // GET: Properties/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: Properties/Create
        [HttpGet]
        public ActionResult Create(string UserID)
        {
            setviewbag();
            ViewBag.UserID = UserID;
            //ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName");
            //ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.PropertyTypeID = new SelectList(db.PropertyTypes, "PropertyTypeID", "PropertyTypeName");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Property property, int PropertyIsFernishedvalue)
        {
            if (ModelState.IsValid)
            {
                property.PropertyIsFernished = (byte)PropertyIsFernishedvalue;
                property.InsertDatetime = DateTime.UtcNow;

                property.PropertyOurCode = db.Properties.Max(a => a.PropertyOurCode) + 1;
                if (property.UserID == null)
                {
                    property.UserID = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First().Id;
                }
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            setviewbag(property.AreaID, property.PropertyTypeID);
            return View(property);
        }

        public ActionResult search()
        {
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName");
            //ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult search(PropertySearchViewModel sproperty)
        {
            var searchRes = db.Properties.Where(a => 1 == 1);
            //if (sproperty.AreaID != null && sproperty.AreaID.Count > 0)
            //{
            //    //int[] asdasd = sproperty.AreaID.ToArray();
            //    searchRes = searchRes.Where(a => sproperty.AreaID.Contains(a.AreaID));
            //}
            if (sproperty.InsertDatetime != null)
            {
                searchRes = searchRes.Where(a => a.InsertDatetime > sproperty.InsertDatetime);
            }
            if (sproperty.InsertDatetimeMax != null)
            {
                searchRes = searchRes.Where(a => a.InsertDatetime < sproperty.InsertDatetimeMax);
            }

            if (sproperty.IsExpired != null)
            {
                searchRes = searchRes.Where(a => a.InsertDatetime > sproperty.InsertDatetime);
            }

            if (sproperty.IsSpecial != null)
            {
                searchRes = searchRes.Where(a => a.IsSpecial == sproperty.IsSpecial);
            }

            if (sproperty.PropertyAge != null)
            {
                searchRes = searchRes.Where(a => a.PropertyAge > sproperty.PropertyAge);
            }
            if (sproperty.PropertyAgeMax != null)
            {
                searchRes = searchRes.Where(a => a.PropertyAge < sproperty.PropertyAgeMax);
            }



            if (sproperty.PropertyFloor != null)
            {
                searchRes = searchRes.Where(a => a.PropertyFloor > sproperty.PropertyFloor);
            }
            //if (sproperty.PropertyFloorMax != null)
            //{
            //    searchRes = searchRes.Where(a => a.PropertyFloor < sproperty.PropertyFloorMax);
            //}

            if (sproperty.PropertyHasGaurd != null)
            {
                searchRes = searchRes.Where(a => a.PropertyHasGaurd == sproperty.PropertyHasGaurd);
            }

            if (sproperty.PropertyHasGaurd != null)
            {
                searchRes = searchRes.Where(a => a.PropertyHasGaurd == sproperty.PropertyHasGaurd);
            }
            searchRes.OrderByDescending(a => a.InsertDatetime);

            setviewbag();
            ViewBag.PropertyTypeID = new SelectList(db.PropertyTypes, "PropertyTypeID", "PropertyTypeName", sproperty.PropertyTypeID);
            return View(sproperty);
        }

        // GET: Properties/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }

            setviewbag(property.AreaID, property.PropertyTypeID, property.PropertyIsFernished);
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
        //[Bind(Include = "PropertyID,PropertyName,PropertyPrice,UserID,IsExpired,InsertDatetime,PropertyTypeID,IsSpecial,WhyIsSpecial,AreaID,PropertySpace,PropertyRoom,PropertyPool,PropertyFloor,PropertyAllFloor,PropertyAge,PropertyUnitsPerFloor,PropertyParkingNumber,PropertySauna,PropertyJacuzzi,PropertyRoofGarden,PropertyHasLobby,PropertyHasLobbyMan,PropertyHasGaurd,PropertyHasGym,MapID")]
        Property property, int PropertyIsFernishedvalue)
        {
            if (ModelState.IsValid)
            {
                property.PropertyIsFernished = (byte)PropertyIsFernishedvalue;
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            setviewbag(property.AreaID, property.PropertyTypeID);
            return View(property);
        }

        // GET: Properties/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Property property = db.Properties.Find(id);
            property.IsExpired = true;
            db.Entry(property).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
