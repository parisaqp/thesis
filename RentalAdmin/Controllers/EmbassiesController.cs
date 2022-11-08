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
    public class EmbassiesController : Controller
    {
        private RentalEntities db = new RentalEntities();
        private int pageSize = 15;
        private  void setviewbag(int? AreaID=null)
        {
            ViewBag.AreaID = new SelectList(db.Areas.OrderBy(a => a.AreaOrder), "AreaID", "AreaName", AreaID);
        }
        // GET: Embassies
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var embassies = db.Embassies.Include(e => e.Map).OrderBy(a=>a.EmbassyName).ToPagedList(pageNumber, pageSize);
            return View(embassies);
        }

        // GET: Embassies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embassy embassy = db.Embassies.Find(id);
            if (embassy == null)
            {
                return HttpNotFound();
            }
            return View(embassy);
        }

        // GET: Embassies/Create
        public ActionResult Create()
        {
            setviewbag();
            //ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapGoogleLink");
            return View();
        }

        // POST: Embassies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmbassyID,EmbassyName,EmbassyOrginalName,EmbassyAddress,AreaID,EmbassyWebsite,EmbassyBirthDate,EmbassyEmail,EbmbassyPhone,EmbassySlug,MapID,ImageSmall,ImageBig")] Embassy embassy)
        {
            if (ModelState.IsValid)
            {
                db.Embassies.Add(embassy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            setviewbag();
            return View(embassy);
        }

        // GET: Embassies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embassy embassy = db.Embassies.Find(id);
            if (embassy == null)
            {
                return HttpNotFound();
            }
            setviewbag(embassy.AreaID);
            return View(embassy);
        }

        // POST: Embassies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Embassy embassy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(embassy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            setviewbag(embassy.AreaID);
            return View(embassy);
        }

        // GET: Embassies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Embassy embassy = db.Embassies.Find(id);
            if (embassy == null)
            {
                return HttpNotFound();
            }
            return View(embassy);
        }

        // POST: Embassies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Embassy embassy = db.Embassies.Find(id);
            db.Embassies.Remove(embassy);
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
