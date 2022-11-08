using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class MapsController : Controller
    {
        private RentalEntities db = new RentalEntities();

        // GET: Maps
        public ActionResult Index()
        {
            var maps = db.Maps.Include(m => m.UploadBig).Include(m => m.UploadSmall);
            return View(maps.ToList());
        }

        // GET: Maps/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // GET: Maps/Create
        public ActionResult Create(string source, long forienid, string redirect)
        {
            ViewBag.source = source;
            ViewBag.forienid = forienid;
            ViewBag.redirect = redirect;

            switch (source.ToLower())
            {
                case "area":
                    var theArea = db.Areas.Where(a => a.AreaID == forienid && a.MapID != null).FirstOrDefault();
                    if (theArea != null)
                    {
                        return RedirectToAction("edit", new { id = theArea.MapID, redirect = redirect });
                    }
                    break;
                case "property":
                    var theproperty = db.Properties.Where(a => a.PropertyID == forienid && a.MapID != null).FirstOrDefault();
                    if (theproperty != null)
                    {
                        return RedirectToAction("edit", new { id = theproperty.MapID, redirect = redirect });
                    }
                    break;
                case "metro":
                    var theMetro = db.Metroes.Where(a => a.MetroID == forienid && a.MapID != null).FirstOrDefault();
                    if (theMetro != null)
                    {
                        return RedirectToAction("edit", new { id = theMetro.MapID, redirect = redirect });
                    }
                    break;
                case "embassy":
                    var theembassy = db.Embassies.Where(a => a.EmbassyID == forienid && a.MapID != null).FirstOrDefault();
                    if (theembassy != null)
                    {
                        return RedirectToAction("edit", new { id = theembassy.MapID, redirect = redirect });
                    }
                    break;
                default:
                    break;
            }

            //ViewBag.MapImageBigID = new SelectList(db.Uploads, "UploadID", "UserName");
            //ViewBag.MapImageID = new SelectList(db.Uploads, "UploadID", "UserName");
            return View(new Map());
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Map map, string source, long forienid, string redirect,
            HttpPostedFileBase smallimage = null, HttpPostedFileBase bigimage = null)
        {
            if (ModelState.IsValid)
            {
                db.Maps.Add(map);
                db.SaveChanges();
                switch (source.ToLower())
                {
                    case "area":
                        var theArea = db.Areas.Where(a => a.AreaID == forienid).FirstOrDefault();
                        theArea.MapID = map.MapID;
                        db.Entry(theArea).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "property":
                        var theproperty = db.Properties.Where(a => a.PropertyID == forienid).FirstOrDefault();
                        theproperty.MapID = map.MapID;
                        db.Entry(theproperty).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "metro":
                        var theMetro = db.Metroes.Where(a => a.MetroID == forienid).FirstOrDefault();
                        theMetro.MapID = map.MapID;
                        db.Entry(theMetro).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "embassy":
                        var theembassy = db.Embassies.Where(a => a.EmbassyID == forienid).FirstOrDefault();
                        theembassy.MapID = map.MapID;
                        db.Entry(theembassy).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    default:
                        break;
                }
                bool souldsavemapagain = false;
                if (smallimage != null)
                {
                    Upload up = RentalAdmin.helper.filemanager.saveFile(smallimage, "map");
                    db.Uploads.Add(up);
                    db.SaveChanges();
                    map.MapImageID = up.UploadID;
                    souldsavemapagain = true;
                }
                if (bigimage != null)
                {
                    Upload up = RentalAdmin.helper.filemanager.saveFile(bigimage, "map");
                    db.Uploads.Add(up);
                    db.SaveChanges();
                    map.MapImageBigID = up.UploadID;
                    souldsavemapagain = true;
                }
                if (souldsavemapagain)
                {
                    db.Entry(map).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // create upload.
                return Redirect(redirect);
            }
            return View(map);
        }

        // GET: Maps/Edit/5
        public ActionResult Edit(long? id, string redirect)
        {
            ViewBag.redirect = redirect;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Include(a => a.UploadBig).Include(a => a.UploadSmall).Where(a => a.MapID == id).FirstOrDefault();
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Map map, string redirect,
            HttpPostedFileBase smallimage = null, HttpPostedFileBase bigimage = null)
        {
            Upload uptodelete1 = null;
            Upload uptodelete2 = null;
            if (ModelState.IsValid)
            {
                db.Entry(map).State = EntityState.Modified;
                db.SaveChanges();
                //bool souldsavemapagain = false;
                if (smallimage != null)
                {
                    if (map.MapImageID != null)
                    {
                         uptodelete2 = helper.filemanager.saveFile(smallimage, "map");
                        db.Uploads.Add(uptodelete2);
                        db.SaveChanges();
                    }

                    Upload up = db.Uploads.Where(a => a.UploadID == map.MapImageID).FirstOrDefault();
                    up = helper.filemanager.replaceFile(smallimage, up);
                    db.Entry(up).State = EntityState.Modified;
                    db.SaveChanges();
                    map.MapImageID = up.UploadID;
                    db.Entry(map).State = EntityState.Modified;
                    db.SaveChanges();
                    //souldsavemapagain = true;

                }
                if (bigimage != null)
                {
                    if (map.MapImageBigID != null)
                    {
                        uptodelete1 = db.Uploads.Where(a => a.UploadID == map.MapImageBigID).FirstOrDefault();

                    }

                    Upload up = helper.filemanager.saveFile(bigimage, "map");
                    db.Uploads.Add(up);
                    db.SaveChanges();
                    map.MapImageBigID = up.UploadID;
                    db.Entry(map).State = EntityState.Modified;
                    db.SaveChanges();
                    //souldsavemapagain = true;
                }
                if (uptodelete1 != null)
                {
                    RentalAdmin.helper.filemanager.deleteFile(uptodelete1);
                    db.Uploads.Remove(uptodelete1);
                    db.SaveChanges();
                }
                if (uptodelete2 != null)
                {
                    RentalAdmin.helper.filemanager.deleteFile(uptodelete2);
                    db.Uploads.Remove(uptodelete2);
                    db.SaveChanges();
                }
                db.Entry(map).State = EntityState.Modified;
                db.SaveChanges();



                return Redirect(redirect);
            }
            return View(map);
        }

        // GET: Maps/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Include(a => a.UploadBig).Include(a => a.UploadSmall).Where(a => a.MapID == id).FirstOrDefault();
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Map map = db.Maps.Find(id);
            db.Maps.Remove(map);
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
