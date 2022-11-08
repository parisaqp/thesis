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
    public class UploadsController : Controller
    {
        private RentalEntities db = new RentalEntities();

        // GET: Uploads
        public ActionResult Index()
        {
            return View(db.Uploads.ToList());
        }
        public ActionResult MyUpload()
        {
            return View(db.Uploads.Where(a=>a.UserName==User.Identity.Name).OrderByDescending(a=>a.UploadDate).ToList());
        }

        // GET: Uploads/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // GET: Uploads/Create
        [HttpGet]
        public ActionResult create(string fromsource, string returnurl, long id = 0)
        {
            ViewBag.fromsource = fromsource;
            ViewBag.id = id;
            ViewBag.returnurl = returnurl;

            return View();
        }

        [HttpPost]
        public ActionResult createpost(string fromsource, long id, string returnurl, string sumbit)
        {
            if (sumbit == "delete")
            {

            }
            else
            if (Request.Files.Count > 0)
            {
                if(string.IsNullOrEmpty(fromsource))
                {
                    fromsource = "user";
                }
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var user= db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                    Upload up = helper.filemanager.saveFile(file, fromsource,null, user.Id);
                    up.UserName = user.UserName;
                    db.Uploads.Add(up);
                    db.SaveChanges();

                }
            }
            return RedirectToAction("myupload", "uploads");
            //if(returnurl==null)
            //{
            //    return RedirectToAction("myupload", "uploads");
            //}
            //return Redirect(returnurl);
        }

        [HttpGet]
        public ActionResult gallery(long id)
        {
            var data = db.PropertyGalleries.Include(a => a.Upload).Where(a => a.PropertyID == id)
                .OrderBy(a => a.PropertyGalleryOrder).ToList();
            ViewBag.id = id;
            return View(data);
        }

        [HttpPost]
        public ActionResult gallery(List<HttpPostedFileBase> newImage,List<int> newImageOrder, List<HttpPostedFileBase> replaceImage
            , List<long> uploadID, List<int> imageOrder, List<long> deleteimage, long id)
        {
            byte order = 0;
            int i = 0;
           //update order of exist
            if(uploadID!=null && uploadID.Count>0&& imageOrder!=null&& imageOrder.Count>0)
            {
                foreach (var item in imageOrder)
                {
                    long theUploadID = uploadID[i];
                    byte theOrder = (byte)item;
                    var propertyGallery = db.PropertyGalleries.Where(a => a.UploadID == theUploadID
                          && a.PropertyID == id).FirstOrDefault();
                    propertyGallery.PropertyGalleryOrder = theOrder;
                    db.Entry(propertyGallery).State = EntityState.Modified;
                    db.SaveChanges();
                    i++;
                }
               
            }
            // delete image
            i = 0;
            if (deleteimage != null && deleteimage.Count > 0)
            {
                foreach (var item in deleteimage)
                {
                    //long theUploadID = uploadID[i];
                    var propertyGallery = db.PropertyGalleries.Where(a => a.PropertyGalleryID == item
                      //&& a.PropertyID == id
                    ).Include(a => a.Upload).FirstOrDefault();
                    var res = RentalAdmin.helper.filemanager.deleteFile(propertyGallery.Upload);
                    if (res)
                    {
                        db.PropertyGalleries.Remove(propertyGallery);
                        db.SaveChanges();
                    }
                    i++;
                }
            }

            if (replaceImage != null)
            {
                i = 0;
                foreach (var item in replaceImage)
                {

                    if (item != null)
                    {
                        long theUploadID = uploadID[i];
                        Upload up = db.Uploads.Where(a => a.UploadID == theUploadID).FirstOrDefault();
                        up = helper.filemanager.replaceFile(item, up);
                        db.Entry(up).State = EntityState.Modified;
                        db.SaveChanges();
                        
                    }
                    i++;
                }
            }
            if (newImage != null)
            {
                i = 0;
                foreach (var item in newImage)
                {
                    if (item != null)
                    {
                        Upload up = RentalAdmin.helper.filemanager.saveFile(item, "property",id);
                        db.Uploads.Add(up);
                        db.SaveChanges();
                        var temp = new PropertyGallery()
                        {
                            PropertyGalleryInsertDate = DateTime.UtcNow,
                            PropertyGalleryOrder =(Byte) newImageOrder[i],
                            PropertyID = id,
                            UploadID = up.UploadID
                        };
                        db.PropertyGalleries.Add(temp);
                        db.SaveChanges();
                        order++;
                    }
                    i++;
                }
            }
            // set property preview image
            var preViewImage= db.PropertyGalleries.Include(a => a.Upload)
                .Where(a => a.PropertyID == id && a.PropertyGalleryOrder == 99).FirstOrDefault();
            if(preViewImage==null)
            {
                preViewImage= db.PropertyGalleries.Include(a => a.Upload)
                                .Where(a => a.PropertyID == id && a.PropertyGalleryOrder == 1).FirstOrDefault();
            }
            if(preViewImage!=null)
            {
                var theProperty = db.Properties.Where(a => a.PropertyID == id).FirstOrDefault();
                theProperty.PropertyImageAddress = preViewImage.Upload.GetPublicUrl();
                db.Entry(theProperty).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("gallery", new { id = id });
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult uplaodfile()
        {
            //if (ModelState.IsValid)
            //{
            foreach (string fileName in Request.Files)
            {
            }
            //        db.Uploads.Add(upload);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(upload);
            return View();
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UploadID,UserName,UploadName,UploadType,UploadContentType,BasicAddressPhysical,BasicAddressVirtual,UploadAddressOffSet,UploadDate,UploadWidth,UploadHeight,UploadTime,UploadSize,UploadDownloadCount,UploadSourceIsOut,UploadSetLogo,UploadSetStartVid,UploadLogLeft,UploadOverWriteRendered")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(upload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(upload);
        }

        // GET: Uploads/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Uploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Upload upload = db.Uploads.Find(id);
            db.Uploads.Remove(upload);
            db.SaveChanges();
            return RedirectToAction("myupload", "uploads");
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
