using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;

namespace RentalAdmin.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private RentalEntities db = new RentalEntities();
        private void setviewbag(int? AreaID = null, int? PropertyTypeID = null, byte? PropertyIsFernished = null)
        {
            ViewBag.AreaID = new SelectList(db.Areas.OrderBy(a => a.AreaOrder), "AreaID", "AreaName", AreaID);
            ViewBag.PropertyTypeID = new SelectList(db.PropertyTypes, "PropertyTypeID", "PropertyTypeName", PropertyTypeID);
            ViewBag.PropertyIsFernishedvalue = new SelectList(StaticList.FurnishType.Select(a => new { PropertyIsFernished = a.Value, PropertyIsFernishedName = a.Text }).ToList(), "PropertyIsFernished", "PropertyIsFernishedName", PropertyIsFernished);
        }
        // GET: Agent
        public ActionResult dashboard()
        {
            return View();
        }
        public ActionResult myProperties()
        {
            var user = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
            var data= db.Properties.Where(a => a.UserID == user.Id).Include(a=>a.Area).Include(a => a.PropertyType).ToList();
            return View(data);
        }
        public ActionResult support()
        {
            return View();
        }

     
        [HttpGet]
        public ActionResult SendProperty()
        {
            setviewbag();
            return View();
        }
        [HttpPost]
        [ActionName("SendProperty")]
        public ActionResult SendPropertyPost(Property property, int PropertyIsFernishedvalue)
        {
            if (ModelState.IsValid)
            {
                property.IsExpired = true;
                property.PropertyIsFernished = (byte)PropertyIsFernishedvalue;
                property.InsertDatetime = DateTime.UtcNow;
                property.UserID = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First().Id;
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("SendProperty2",new { id= property.PropertyID});
            }
            setviewbag(property.AreaID, property.PropertyTypeID);
            return View(property);
        }
        [HttpGet]
        public ActionResult SendProperty2(long id)
        {
            var data = db.PropertyGalleries.Include(a => a.Upload).Where(a => a.PropertyID == id)
              .OrderBy(a => a.PropertyGalleryOrder).ToList();
            ViewBag.id = id;
            return View(data);
        }
        [HttpPost]
        [ActionName("SendProperty2")]
        public ActionResult SendPropertyPost2(List<HttpPostedFileBase> newImage, List<int> newImageOrder, List<HttpPostedFileBase> replaceImage
            , List<long> uploadID, List<int> imageOrder, List<long> deleteimage, long id)
        {
           var theProperty= db.Properties.Where(a => a.PropertyID == id).FirstOrDefault();
            if(!CanEditPropertyBy(theProperty, User))
            {
                return RedirectToAction("SendProperty2", new { id = id });
            }
            byte order = 0;
            int i = 0;
            //update order of exist
            if (uploadID != null && uploadID.Count > 0 && imageOrder != null && imageOrder.Count > 0)
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
                        Upload up = RentalAdmin.helper.filemanager.saveFile(item, "property", id);
                        db.Uploads.Add(up);
                        db.SaveChanges();
                        var temp = new PropertyGallery()
                        {
                            PropertyGalleryInsertDate = DateTime.UtcNow,
                            PropertyGalleryOrder = (Byte)newImageOrder[i],
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
            var preViewImage = db.PropertyGalleries.Include(a => a.Upload)
                .Where(a => a.PropertyID == id && a.PropertyGalleryOrder == 99).FirstOrDefault();
            if (preViewImage == null)
            {
                preViewImage = db.PropertyGalleries.Include(a => a.Upload)
                                .Where(a => a.PropertyID == id && a.PropertyGalleryOrder == 1).FirstOrDefault();
            }
            if (preViewImage != null)
            {
                theProperty.PropertyImageAddress = preViewImage.Upload.GetPublicUrl();
                db.Entry(theProperty).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("myProperties");
        }

        // GET: Properties/Edit/5
        public ActionResult EditProperty(long? id)
        {
            var user = User;
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }

            setviewbag();
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProperty(
        //[Bind(Include = "PropertyID,PropertyName,PropertyPrice,UserID,IsExpired,InsertDatetime,PropertyTypeID,IsSpecial,WhyIsSpecial,AreaID,PropertySpace,PropertyRoom,PropertyPool,PropertyFloor,PropertyAllFloor,PropertyAge,PropertyUnitsPerFloor,PropertyParkingNumber,PropertySauna,PropertyJacuzzi,PropertyRoofGarden,PropertyHasLobby,PropertyHasLobbyMan,PropertyHasGaurd,PropertyHasGym,MapID")]
        Property property, int PropertyIsFernishedvalue)
        {
          
            if (ModelState.IsValid&& CanEditPropertyBy(property, User))
            {
                var oldProperty = db.Properties.Where(a => a.PropertyID == property.PropertyID).FirstOrDefault();
                oldProperty.PropertyPrice = property.PropertyPrice;
                oldProperty.PropertyTypeID = property.PropertyTypeID;
                oldProperty.IsSpecial = property.IsSpecial;
                oldProperty.WhyIsSpecial = property.WhyIsSpecial;
                oldProperty.AreaID = property.AreaID;
                oldProperty.PropertySpace = property.PropertySpace;
                oldProperty.PropertyRoom = property.PropertyRoom;
                oldProperty.PropetryBathRooms = property.PropetryBathRooms;
                oldProperty.PropertyFloor = property.PropertyFloor;
                oldProperty.PropertyAllFloor = property.PropertyAllFloor;
                oldProperty.PropertyAge = property.PropertyAge;
                oldProperty.PropertyUnitsPerFloor = property.PropertyUnitsPerFloor;
                oldProperty.PropertyParkingNumber = property.PropertyParkingNumber;
                oldProperty.PropertyPool = property.PropertyPool;
                oldProperty.PropertySauna = property.PropertySauna;
                oldProperty.PropertyJacuzzi = property.PropertyJacuzzi;
                oldProperty.PropertyRoofGarden = property.PropertyRoofGarden;
                oldProperty.PropertyHasLobby = property.PropertyHasLobby;
                oldProperty.PropertyHasLobbyMan = property.PropertyHasLobbyMan;
                oldProperty.PropertyHasGaurd = property.PropertyHasGaurd;
                oldProperty.PropertyHasGym = property.PropertyHasGym;
                oldProperty.PropertyIsFernished = property.PropertyIsFernished;
                oldProperty.PropertyDescription = property.PropertyDescription;

                //oldProperty.PropertyIsFernished = (byte)PropertyIsFernishedvalue;
                db.Entry(oldProperty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SendProperty2", new { id = property.PropertyID });
            }
            setviewbag(property.AreaID, property.PropertyTypeID);
            return View(property);
        }


        public bool CanEditPropertyBy(Property theProperty,System.Security.Principal.IPrincipal User)
        {
            if(User.IsInRole("admin"))
            {
                return true;
            }
            if(theProperty.IsExpired==true)
            {
                return true;
            }
            return false;
        }
    }
}