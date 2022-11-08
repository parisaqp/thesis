using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles ="admin")]
    public class AreasController : Controller
    {
        private RentalEntities db = new RentalEntities();
        #region general
        //private void SetViewBagsForDropDownLists(int? EmailLayoutID = null, int? emailStatus = null, int? EmailCreateStatus = null)
        //{
        //    // **************************************************

        //    var varParents =
        //                   db.EmailLayouts
        //        .Select(current => new
        //        {
        //            Id = current.EmailLayoutID,
        //            Name = current.EmailLayoutName,
        //        });
        //    //ViewBag.ToturialCompanyID =
        //    ViewBag.EmailLayoutIDData =
        //        new System.Web.Mvc.SelectList
        //            (varParents, "Id", "Name", EmailLayoutID);

        //    ViewBag.EmailStatus =
        //       new System.Web.Mvc.SelectList
        //           (limoonad.Models.StaticList.EmailStatus.Select(current => new
        //           {
        //               Id = current.Value,
        //               Name = current.Text,
        //           }), "Id", "Name", emailStatus);

        //    ViewBag.EmailCreateStatus =
        //  new System.Web.Mvc.SelectList
        //      (limoonad.Models.StaticList.EmailCreateStatus.Select(current => new
        //      {
        //          Id = current.Value,
        //          Name = current.Text,
        //      }), "Id", "Name", EmailCreateStatus);
        //    // **************************************************
        //}

        #endregion /general
        // GET: Areas
        public ActionResult Index()
        {
            var areas = db.Areas.Include(a => a.Map);
            return View(areas.OrderBy(a=>a.AreaName).ToList());
        }

        // GET: Areas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Areas/Create
        public ActionResult Create()
        {
            //ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapID");
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Area area)
        {
            if (ModelState.IsValid)
            {
                area.AreaSlug = helper.StringNumberConvertot.PreperSlug(area.AreaSlug);
                db.Areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapID", area.MapID);
            return View(area);
        }

        public ActionResult SetImage(long id)
        {
           var theArea= db.Areas.Where(a => a.AreaID == id).FirstOrDefault();
            //ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapID");
            return View(theArea);
        }

        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetImage(long id, HttpPostedFileBase smallimage = null, HttpPostedFileBase bigimage = null)
        {
            //var theArea = db.Areas.Where(a => a.AreaID == id).FirstOrDefault();

            //if (smallimage != null)
            //{
            //    Upload up = RentalAdmin.helper.filemanager.saveFile(smallimage, "area");
            //    db.Uploads.Add(up);
            //    db.SaveChanges();
            //    theArea.ImageSmallUploadID = up.UploadID;
            //    //souldsavemapagain = true;
            //}
            //if (bigimage != null)
            //{
            //    Upload up = RentalAdmin.helper.filemanager.saveFile(bigimage, "area");
            //    db.Uploads.Add(up);
            //    db.SaveChanges();
            //    theArea.ImageBigUploadID = up.UploadID;
            //  //  souldsavemapagain = true;
            //}

            //area.AreaSlug = helper.StringNumberConvertot.PreperSlug(area.AreaSlug);
            //db.Areas.Add(area);
            //db.SaveChanges();
            return RedirectToAction("Index");


            //ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapID", area.MapID);
            //return RedirectToAction("SetImage", new { id = id });
        }

        // GET: Areas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Area area)
        {
            if (ModelState.IsValid)
            {
                area.AreaSlug = helper.StringNumberConvertot.PreperSlug(area.AreaSlug);
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapID", area.MapID);
            return View(area);
        }

        // GET: Areas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Areas.Find(id);
            db.Areas.Remove(area);
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
