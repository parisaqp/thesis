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
    public class BlogAdminController : Controller
    {
        private RentalEntities db = new RentalEntities();

private void setViewBag(int? DisplayLanguageID = null, int? BlogTypeID = null)
        {
            ViewBag.DisplayLanguageID = new SelectList(db.DisplayLanguages, "DisplayLanguageID", "DisplayLanguageName", DisplayLanguageID);
            ViewBag.BlogTypeID = new SelectList(db.BlogTypes, "BlogTypeID", "TypeName", BlogTypeID);
        }
        // GET: Blog
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.DisplayLanguage);
            return View(blogs.OrderByDescending(a=>a.InsertDateTime).ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            setViewBag();
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog,int? BlogTypeID)
        {
            blog.BlogID = db.Blogs.Max(a => a.BlogID) + 1;
            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                if(BlogTypeID!=null&& BlogTypeID >0)
                {
                    ComplexBlog complexBlog = new ComplexBlog() {
                        BlogID=blog.BlogID,
                        BlogTypeID=(int) BlogTypeID,
                        
                    };
                    db.ComplexBlogs.Add(complexBlog);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            setViewBag(blog.DisplayLanguageID, BlogTypeID);
            return View(blog);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            int? BlogTypeID = null;
            ComplexBlog complexBlog = db.ComplexBlogs.Where(a => a.BlogID == blog.BlogID
                       ).FirstOrDefault();
            if(complexBlog!=null)
            {
                BlogTypeID = complexBlog.BlogTypeID;
            }
            setViewBag(blog.DisplayLanguageID, BlogTypeID);
            return View(blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Blog blog,int? BlogTypeID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                if (BlogTypeID != null && BlogTypeID > 0)
                {
                    int tempTypeID = (int)BlogTypeID;
                    ComplexBlog complexBlog = db.ComplexBlogs.Where(a => a.BlogID == blog.BlogID
                       ).FirstOrDefault();
                    if(complexBlog==null)
                    {
                        complexBlog = new ComplexBlog()
                        {
                            BlogID = blog.BlogID,
                            BlogTypeID = (int)BlogTypeID,

                        };
                        db.ComplexBlogs.Add(complexBlog);
                        db.SaveChanges();
                    }
                    else if(complexBlog.BlogTypeID!= tempTypeID)
                    {
                        complexBlog.BlogTypeID = tempTypeID;
                        db.Entry(complexBlog).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                   
                }
                return RedirectToAction("Index");
            }
            setViewBag(blog.DisplayLanguageID, BlogTypeID);
            return View(blog);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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
