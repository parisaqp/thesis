using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;
using PagedList;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CustomerBuysController : Controller
    {
        private RentalEntities db = new RentalEntities();

        // GET: CustomerBuys
        public ActionResult Index(int page = 1)
        {
            var data = db.CustomerBuys.OrderByDescending(a => a.FirstCallDate).ToPagedList(page,12);
            return View(data);
        }

        // GET: CustomerBuys/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBuy customerBuy = db.CustomerBuys.Find(id);
            if (customerBuy == null)
            {
                return HttpNotFound();
            }
            return View(customerBuy);
        }

        // GET: CustomerBuys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerBuys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( CustomerBuy customerBuy)
        {
            if (ModelState.IsValid)
            {
                db.CustomerBuys.Add(customerBuy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerBuy);
        }

        // GET: CustomerBuys/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBuy customerBuy = db.CustomerBuys.Find(id);
            if (customerBuy == null)
            {
                return HttpNotFound();
            }
            return View(customerBuy);
        }

        // POST: CustomerBuys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CustomerBuy customerBuy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerBuy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerBuy);
        }

        // GET: CustomerBuys/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBuy customerBuy = db.CustomerBuys.Find(id);
            if (customerBuy == null)
            {
                return HttpNotFound();
            }
            return View(customerBuy);
        }

        // POST: CustomerBuys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CustomerBuy customerBuy = db.CustomerBuys.Find(id);
            db.CustomerBuys.Remove(customerBuy);
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
