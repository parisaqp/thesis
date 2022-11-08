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
    public class RequestApartmentsController : Controller
    {
        private RentalEntities db = new RentalEntities();

        // GET: RequestApartments
        public ActionResult Index()
        {
            return View(db.RequestApartments.ToList());
        }

        // GET: RequestApartments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestApartment requestApartment = db.RequestApartments.Find(id);
            if (requestApartment == null)
            {
                return HttpNotFound();
            }
            return View(requestApartment);
        }
        [AllowAnonymous]
        // GET: RequestApartments/Create
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        // POST: RequestApartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestApartmentID,FullName,Beds,AreaName,PersonsCount,MounthCount,DaysCount,WhenCome,Nation,Email,PhoneNumber")] RequestApartment requestApartment)
        {
            if (ModelState.IsValid)
            {
                db.RequestApartments.Add(requestApartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestApartment);
        }

        // GET: RequestApartments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestApartment requestApartment = db.RequestApartments.Find(id);
            if (requestApartment == null)
            {
                return HttpNotFound();
            }
            return View(requestApartment);
        }

        // POST: RequestApartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestApartmentID,FullName,Beds,AreaName,PersonsCount,MounthCount,DaysCount,WhenCome,Nation,Email,PhoneNumber")] RequestApartment requestApartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestApartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestApartment);
        }

        // GET: RequestApartments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestApartment requestApartment = db.RequestApartments.Find(id);
            if (requestApartment == null)
            {
                return HttpNotFound();
            }
            return View(requestApartment);
        }

        // POST: RequestApartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            RequestApartment requestApartment = db.RequestApartments.Find(id);
            db.RequestApartments.Remove(requestApartment);
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
