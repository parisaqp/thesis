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
    [Authorize(Roles = "admin")]
    public class MetroesController : Controller
    {
        private RentalEntities db = new RentalEntities();

        // GET: Metroes
        public ActionResult Index()
        {
            var metroes = db.Metroes.Include(m => m.Area).Include(m => m.Map);
            return View(metroes.OrderBy(a=>a.Line).ThenBy(a=>a.MetroName).ToList());
        }

        // GET: Metroes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metro metro = db.Metroes.Find(id);
            if (metro == null)
            {
                return HttpNotFound();
            }
            return View(metro);
        }

        // GET: Metroes/Create
        public ActionResult Create()
        {
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName");
            return View();
        }

        // POST: Metroes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Metro metro)
        {
            if (ModelState.IsValid)
            {
                db.Metroes.Add(metro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName", metro.AreaID);
            return View(metro);
        }

        // GET: Metroes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metro metro = db.Metroes.Find(id);
            if (metro == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName", metro.AreaID);
            return View(metro);
        }

        // POST: Metroes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MetroID,MetroName,AreaID,Line,MapID")] Metro metro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "AreaName", metro.AreaID);
            ViewBag.MapID = new SelectList(db.Maps, "MapID", "MapGoogleLink", metro.MapID);
            return View(metro);
        }

        // GET: Metroes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metro metro = db.Metroes.Find(id);
            if (metro == null)
            {
                return HttpNotFound();
            }
            return View(metro);
        }

        // POST: Metroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Metro metro = db.Metroes.Find(id);
            db.Metroes.Remove(metro);
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
