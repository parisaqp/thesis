
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using System.Data;
using System.Data.Entity;
using System.Xml;
using RentalAdmin.Models;
using RentalAdmin.helper;

namespace RentalAdmin.Controllers
{
    public class SiteMapController : Controller
    {
        private RentalEntities db = new RentalEntities();
        /// <summary>
        /// for send error to Admin
        /// </summary>
        //byte sendFromToAdmin = 6;
        const string priceCurrency = "USD";

        private XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        //xmlns:video=
        private XNamespace xmlnsVideo = "http://www.google.com/schemas/sitemap-video/1.1";
        // GET: SiteMap
        //[OutputCache(Duration = 172800, Location = OutputCacheLocation.Server)]// 24 saati ast 
        //public ContentResult RSS()
        //{
        //    var items = GetRSSFeed();
        //    var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
        //      new XElement("rss",
        //        new XAttribute("version", "2.0"),
        //          new XElement("channel",
        //            new XElement("title", "جدیدترین های رسانه آموزشی لیموناد"),
        //            new XElement("link", StaticList.rssAddress),
        //            new XElement("description", "جدیدترین دوره های آموزشی رسانه آموزشی لیموناد"),
        //            new XElement("copyright", "(c)" + DateTime.Now.Year + ", رسانه آموزشی لیموناد. کلیه حقوق محفوظ است"),
        //          from item in items
        //          select
        //          new XElement("item",
        //            new XElement("title", item.title),
        //            new XElement("description",Uri.EscapeUriString( item.description)),
        //            new XElement("link", item.Url),
        //            new XElement("pubDate", item.publishDate.ToString())

        //          )
        //        )
        //      )
        //    );
        //    return Content(rss.ToString(), "text/xml");
        //}
        [UpdateSiteMapIndex]
        [OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        public ContentResult properies()
        {
            var sitemapNodes = GetPropertiesSitemapNodes();
            return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        }
        //[UpdateSiteMapIndex]

        [UpdateSiteMapIndex]
        [OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        public ContentResult embassies()
        {
            var sitemapNodes = GetEmbassies();
            return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        }
        [UpdateSiteMapIndex]
        [OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        public ContentResult regions()
        {
            var sitemapNodes = GetRegionsSitemapNodes();
            UpdateSiteMapIndex();
            return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        }
        [OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        public ContentResult blogs()
        {
            var sitemapNodes = GetBlogsSitemapNodes();
            UpdateSiteMapIndex();
            return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        }
        //[UpdateSiteMapIndex]
        //[OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        //public ContentResult uppcategory()
        //{
        //    var sitemapNodes = GetUppcategorySitemapNodes();
        //    UpdateSiteMapIndex();
        //    return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        //}
        //[OutputCache(Duration = 43200, Location = OutputCacheLocation.Server)]// 6 saati ast 
        //public ContentResult Pages()
        //{
        //    var sitemapNodes = GetPagesSitemapNodes();
        //    UpdateSiteMapIndex();
        //    return getXmlSiteMapFromNodes(sitemapNodes.ToList());

        //}
        //[UpdateSiteMapIndex]
        //[OutputCache(Duration = 86400, Location = OutputCacheLocation.Server)]// 6 saati ast 
        //public ContentResult Videos()
        //{
        //    var sitemapNodesWithVideos = GetSitemapVideosNodes();
        //    XElement root = new XElement(xmlns + "urlset",
        //        new XAttribute(XNamespace.Xmlns + "video", xmlnsVideo));
        //    foreach (SitemapWithVideosNode NodeWhitVideo in sitemapNodesWithVideos)
        //    {//first create video tag then create url tag and add video to url and all add to root
        //        //preparing tags element
        //        List<XElement> videoElement = new List<XElement>();
        //        foreach (var sitemapNode in NodeWhitVideo.videosNode)
        //        {
        //            XElement[] tags = new XElement[0];
        //            if (sitemapNode.tag.Count() > 0)
        //            {
        //                tags = new XElement[sitemapNode.tag.Count()];
        //                for (int i = 0; i < sitemapNode.tag.Count(); i++)
        //                {
        //                    tags[i] = new XElement(xmlnsVideo + "tag", Uri.EscapeUriString(sitemapNode.tag[i]));
        //                }
        //            }
        //            videoElement.Add(new XElement(
        //               xmlnsVideo + "video",
        //               new XElement(xmlnsVideo + "thumbnail_loc", Uri.EscapeUriString(sitemapNode.thumbnail_loc)),
        //                new XElement(xmlnsVideo + "title", Uri.EscapeUriString(sitemapNode.title)),
        //                new XElement(xmlnsVideo + "description", Uri.EscapeUriString(sitemapNode.description)),
        //                new XElement(xmlnsVideo + "content_loc", sitemapNode.content_loc),
        //                new XElement(xmlnsVideo + "duration", sitemapNode.duration),
        //                sitemapNode.rating < 2 ? null :
        //                new XElement(xmlnsVideo + "rating", sitemapNode.rating),
        //                 new XElement(xmlnsVideo + "publication_date", MyDateTimeExtensions.ConvertDateTimeToString(sitemapNode.publication_date)),
        //                 new XElement(xmlnsVideo + "family_friendly", sitemapNode.family_friendly),
        //                 // new XElement(xmlnsVideo + "gallery_loc", sitemapNode.gallery_loc),
        //                 tags[0] == null ? null : tags,
        //                 new XElement(xmlnsVideo + "category", Uri.EscapeUriString(sitemapNode.category)),
        //                 sitemapNode.price > 0 ?
        //                 new XElement(xmlnsVideo + "price", new XAttribute("currency", priceCurrency), sitemapNode.price * 10)//XNamespace.Xmlns + 
        //                 : null
        //                   ));
        //        }
        //        XElement urlElement = new XElement(
        //            xmlns + "url",
        //            new XElement(xmlns + "loc", NodeWhitVideo.loc), videoElement);
        //        root.Add(urlElement);
        //    }
        //    XDocument document = new XDocument(root);
        //    string documentString = document.ToString();
        //    //bayad in warning haye zir email ham shavand.
        //    //var AdminUserID = db.Settings.Find(18).SettingTask;
        //    //Message warningMazLimit = new Message()
        //    //{
        //    //    MessageDate = DateTime.UtcNow,
        //    //    MessageNotification = false,
        //    //    MessageSeen = false,
        //    //    MessageTitle = "سایت مپ ویدیوها در حال پر شدن است",
        //    //    MessageSendFrom = sendFromToAdmin,
        //    //    MessageReceiverID = AdminUserID,
        //    //    MessageSenderID = AdminUserID
        //    //};
        //    //bool dbMustSave = false;
        //    //if (documentString.Length > 9437184) //max is 10 mb   9437184=9 mb
        //    //{
        //    //    warningMazLimit.MessageBody = "سایز سایت مپ ویدیو ها " +
        //    //        "~/sitemap/Videos" + " به مقدار 9 مگابایت رسیده است حداکثر این سایز 10 مگابایت است ";
        //    //    db.Messages.Add(warningMazLimit); dbMustSave = true;
        //    //    //message to admin that sitemap is max size
        //    //}
        //    //if (sitemapNodesWithVideos.Count > 49000) //max is 50000
        //    //{
        //    //    warningMazLimit.MessageBody = "تعداد آدرس ها از سایت در سایت مپ ویدیو ها  " + "~/sitemap/Videos"
        //    //        + "تا نزدیک حداکثر یعنی 49000 تا رسیده است" + " حداکثر 50000 تا است";
        //    //    db.Messages.Add(warningMazLimit); dbMustSave = true;
        //    //    //message to admin that sitemap is max url
        //    //}
        //    //if (dbMustSave)
        //    //    db.SaveChanges();
        //    //End Message
        //    return Content(documentString, "text/xml", Encoding.UTF8); ;
        //}
        private ContentResult getXmlSiteMapFromNodes(List<SitemapNode> sitemapNodes)
        {
            XElement root = new XElement(xmlns + "urlset");
            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", sitemapNode.Url),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                      MyDateTimeExtensions.ConvertDateTimeToString(sitemapNode.LastModified.Value)),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return Content("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + document.ToString(), "text/xml", Encoding.UTF8);
        }
        //private IReadOnlyCollection<RSS> GetRSSFeed()//UrlHelper urlHelper
        //{
        //    List<RSS> nodes = new List<RSS>();
        //    var CourseNodes = db.Courses.Where(a => a.CourseStatus == 1).OrderByDescending(a => a.CoursePublishDate)
        //       .Take(20).ToList();
        //    foreach (var item in CourseNodes)
        //    {
        //        if (item.CoursePublishDate != null)
        //        {
        //            string courseBody = item.CourseBody;
        //            if (item.CourseBody.Length > 1000)
        //                item.CourseBody = item.CourseBody.Substring(0, 1000) + " ..."; ;
        //            nodes.Add(
        //               new RSS()
        //               {
        //                   Url = string.Format(StaticList.urlToCourse, item.CourseID, item.CourseSlug),//urlHelper.Action("Training", "Course", new { id = item.CourseID, FriendlyUrl = item.CourseSlug }),//limoonad.ClassHelper.LinkGenrator.AbsoluteRouteUrl(urlHelper, "ProductGetProduct", new { id = item.CourseID }),
        //                   title = item.CourseTitle,
        //                   description = courseBody,
        //                   publishDate = (DateTime)item.CoursePublishDate
        //               });
        //        }
        //    }
        //    return nodes;
        //}

        //private IReadOnlyCollection<SitemapNode> GetSingelCourseSitemapNodes()//UrlHelper urlHelper
        //{
        //    List<SitemapNode> nodes = new List<SitemapNode>();
        //    var CourseNodes = db.Courses.Where(a => a.CourseStatus == 1).ToList();

        //    foreach (var item in CourseNodes)
        //    {
        //        SitemapFrequency frequency = SitemapFrequency.Weekly;
        //        TimeSpan stabality = new TimeSpan();
        //        if (item.CourseLastModified != null)
        //        {
        //            stabality = (DateTime)item.CourseLastModified - (DateTime)item.CoursePublishDate;

        //            int days = (int)stabality.TotalDays;
        //            if (days > 30 && days < 300)
        //                frequency = SitemapFrequency.Monthly;
        //            if (days > 300)
        //                frequency = SitemapFrequency.Yearly;
        //        }
        //        else
        //        {

        //        }
        //        nodes.Add(
        //           new SitemapNode()
        //           {
        //               Url = Url.Action("training", "mcourse", new { id = item.CourseID, FriendlyUrl = item.CourseSlug }, "https"),
        //               Frequency = frequency,
        //               Priority = item.CoursePriority,
        //               LastModified = item.CourseLastModified
        //           });
        //    }
        //    return nodes;
        //}
        //private IReadOnlyCollection<SitemapNode> GetStaticPages()//UrlHelper urlHelper
        //{
        //    List<SitemapNode> nodes = new List<SitemapNode>();
        //    nodes.Add(
        //       new SitemapNode()
        //       {
        //           Url = "https://www.limoonad.com",
        //           Frequency = SitemapFrequency.Daily,
        //           Priority = 1,
        //           LastModified = db.Courses.Where(a => a.CourseStatus == 1).Max(a => a.CoursePublishDate)
        //       });
        //    return nodes;
        //}

        //private IReadOnlyCollection<SitemapWithVideosNode> GetPropertiesSitemapNodes()//UrlHelper urlHelper
        //{
        //    List<SitemapWithVideosNode> result = new List<SitemapWithVideosNode>();
        //    var PropertiesNodes = db.Properties.Include(a=>a.Area).Where(a => a.IsExpired == false).ToList();
        //    foreach (var item in PropertiesNodes)
        //    {
        //        //if (item.CourseInterveiwVideoID == null || item.CourseImageFileID == null || item.CourseChapterCount < 1 || item.CourseLearningCount < 1)
        //        //{
        //        //    //email this bad course to admin
        //        //}
        //        //else
        //        //{
        //        SitemapWithVideosNode sitemapWithVideosNode = new SitemapWithVideosNode()
        //        {
        //            loc = item.getURl(),
        //            videosNode = new List<SitemapVideoNode>()
        //        };
        //        List<SitemapVideoNode> nodes = new List<SitemapVideoNode>();
        //        //category from course category table
        //        var uploader = db.AspNetUsers.Find(item.UserId);
        //        var deepTags = item.getTops().ToArray();
        //        if (deepTags != null )
        //        {
        //            //first Add preview video from Course
        //            var previewVideo = db.Uploads.Find(item.CourseInterveiwVideoID);
        //            string courseBody = item.CourseBody;
        //            if (item.CourseBody.Length > 2040)
        //                item.CourseBody = item.CourseBody.Substring(0, 2040) + " ...";
        //            if (string.IsNullOrEmpty(courseBody))
        //                courseBody = item.CourseTitle;
        //            sitemapWithVideosNode.videosNode.Add(new SitemapVideoNode()
        //            {
        //                tag = deepTags,
        //                category = item.Area.getName(),
        //                //content_loc= 
        //                description = item.g,
        //                duration = previewVideo.UploadTime,
        //                family_friendly = "yes",
        //                // gallery_loc = Url.Action("profile", "appuser", new { id = uploader.UserName }, "https"),
        //                live = "no",
        //                //platform
        //                content_loc = Url.Action("getclipbeforeinter", "lmedia", new { id = previewVideo.UploadID }, "https"),
        //                price = item.CoursePrice,
        //                publication_date = (DateTime)item.CoursePublishDate,
        //                rating = item.CourseRate,
        //                requires_subscription = "no",
        //                //restriction
        //                thumbnail_loc = item.CourseImage,
        //                title = item.CourseSubheading,
        //                uploader = uploader.UserFName + " " + uploader.UserLName
        //            });
        //            //End add preview course
        //            //List<Learning> learningVideos = null;
        //            //if (item.CoursePrice == 0 || item.CourseIsSingle == true)
        //            //{
        //            //    learningVideos = db.Learnings.Include(a => a.Chapter.Course).Where(a => a.Chapter.CourseID == item.CourseID).ToList();
        //            //}
        //            //else
        //            //{
        //            //    learningVideos = db.Learnings.Include(a => a.Chapter.Course).Where(a => a.Chapter.CourseID == item.CourseID && a.LearningPreview == true).ToList();
        //            //}
        //            //foreach (var learningV in learningVideos)// to in foreach course single single nemiad
        //            //{
        //            //    Upload Video = db.Uploads.Find(learningV.VideoID);
        //            //    if (Video != null)
        //            //    {
        //            //        string description = learningV.LearningBody;
        //            //        if (description == null)
        //            //            description = learningV.LearningTitle;
        //            //        if (description.Length > 2000)
        //            //            description = description.Substring(0, 2048);
        //            //        sitemapWithVideosNode.videosNode.Add(
        //            //                  new SitemapVideoNode()
        //            //                  {
        //            //                      tag = deepTags,
        //            //                      category = uppCategory.CategoryName,
        //            //                      //content_loc= 
        //            //                      description = learningV.Chapter.ChapterTitle + " - " + learningV.LearningTitle + " - " + description,// Uri.EscapeUriString(description),
        //            //                      duration = Video.UploadTime,
        //            //                      family_friendly = "yes",
        //            //                      // gallery_loc = Url.Action("profile", "appuser", new { id = uploader.UserName }, "https"),
        //            //                      live = "no",
        //            //                      //platform
        //            //                      content_loc = learningV.LearningPreview == true ? Url.Action("getclipbeforeinter", "lmedia", new { id = previewVideo.UploadID }, "https")
        //            //                      : Url.Action("getclip", "lmedia", new { id = previewVideo.UploadID }, "https"),
        //            //                      price = item.CoursePrice,
        //            //                      publication_date = (DateTime)item.CoursePublishDate,
        //            //                      rating = item.CourseRate,
        //            //                      requires_subscription = "no",
        //            //                      //restriction
        //            //                      thumbnail_loc = item.CourseImage,
        //            //                      title = learningV.Chapter.ChapterTitle + " - " + learningV.LearningTitle,
        //            //                      uploader = uploader.UserFName + " " + uploader.UserLName
        //            //                  });
        //            //    }
        //            //}
        //            result.Add(sitemapWithVideosNode);
        //        }
        //        //}

        //    }
        //    return result;
        //}
        private IReadOnlyCollection<SitemapNode> GetPropertiesSitemapNodes()//UrlHelper urlHelper
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var properties = db.Properties.Include(a => a.Area)
                .Where(a => a.IsExpired == false).ToList();
            foreach (var item in properties)
            {
                    //var lastPublishedCourseDate = db.Courses.Where(a => a.CourseStatus == 1 && a.UserId == item.UserID).Select(a => a.CourseLastModified).OrderByDescending(a => a.Value).FirstOrDefault();
                    //if (lastPublishedCourseDate != null)
                    //{
                        var lasttempDate = new DateTime(2020, 01, 12);
                        if (item.InsertDatetime < lasttempDate)
                        {
                    item.InsertDatetime = lasttempDate;
                        }
                double pr = 0.5 +(item.PropertyOurPoint / 250.0);
                pr = helper.StringNumberConvertot.maxAndMin(pr, 0.9, 0.5);
                pr = helper.StringNumberConvertot.doubleToOneDesimal(pr);
                        nodes.Add(
                         new SitemapNode()
                         {
                             Url = "https://www.rentalir.com/"+ item.getURl(),
                             Frequency = SitemapFrequency.Yearly,
                             Priority = pr,
                             LastModified = item.InsertDatetime
                         });
                    //}
                
            }
            return nodes;
        }
        private IReadOnlyCollection<SitemapNode> GetEmbassies()//UrlHelper urlHelper
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var obts = db.Embassies.Include(a => a.Area)
                .ToList();
            foreach (var item in obts)
            {
                //var lastPublishedCourseDate = db.Courses.Where(a => a.CourseStatus == 1 && a.UserId == item.UserID).Select(a => a.CourseLastModified).OrderByDescending(a => a.Value).FirstOrDefault();
                //if (lastPublishedCourseDate != null)
                //{
                var lasttempDate = new DateTime(2020, 01, 12);
             
                nodes.Add(
                 new SitemapNode()
                 {
                     Url ="https://www.rentalir.com/"+ item.getURl(),
                     Frequency = SitemapFrequency.Never,
                     Priority = 0.7,
                     LastModified = lasttempDate
                 });
                //}

            }
            return nodes;
        }
        private IReadOnlyCollection<SitemapNode> GetRegionsSitemapNodes()//UrlHelper urlHelper
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var obts = db.Areas.ToList();
            foreach (var item in obts)
            {
               var tempProperty= db.Properties.Where(a => a.IsExpired == false && a.AreaID == item.AreaID)
                    .OrderByDescending(a => a.InsertDatetime).FirstOrDefault();

                var lasttempDate = new DateTime(2020, 01, 12);
                if(tempProperty!=null&&tempProperty.InsertDatetime > lasttempDate)
                {
                    lasttempDate = tempProperty.InsertDatetime;
                }
                //if (lastPublishedCourseDate != null)
                //{
                double pr = 0.5 + (1.5 / (item.AreaOrder + 1));
                pr = helper.StringNumberConvertot.maxAndMin(pr, 0.9, 0.5);
                pr = helper.StringNumberConvertot.doubleToOneDesimal(pr);

               
                nodes.Add(
                 new SitemapNode()
                 {
                     Url = "https://www.rentalir.com/"+ item.getURl(),
                     Frequency = SitemapFrequency.Daily,
                     Priority = pr,
                     LastModified = lasttempDate
                 });
                //}

            }
            return nodes;
        }
        private IReadOnlyCollection<SitemapNode> GetBlogsSitemapNodes()//UrlHelper urlHelper
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var obts = db.Blogs.ToList();
            foreach (var item in obts)
            {
            
                var lasttempDate =  DateTime.Now;
                if (item.InsertDateTime != null )
                {
                    lasttempDate =(DateTime) item.InsertDateTime;
                }

                //if (lastPublishedCourseDate != null)
                //{
                double pr = 0.9;
                pr = helper.StringNumberConvertot.maxAndMin(pr, 0.9, 0.5);
                pr = helper.StringNumberConvertot.doubleToOneDesimal(pr);


                nodes.Add(
                 new SitemapNode()
                 {
                     Url = "https://www.rentalir.com/" + item.getURl(),
                     Frequency = SitemapFrequency.Daily,
                     Priority = pr,
                     LastModified = lasttempDate
                 });
                //}

            }
            return nodes;
        }

        //private IReadOnlyCollection<SitemapNode> GetPropertiesSitemapNodes()//UrlHelper urlHelper
        //{
        //    List<SitemapNode> nodes = new List<SitemapNode>();
        //    nodes.Add(
        //                 new SitemapNode()
        //                 {
        //                     Url = "https://www.limoonad.com/courses/all",// Url.Action("cat", "search", new { id = item.CategorySlug }, "https"),
        //                     Frequency = SitemapFrequency.Hourly,
        //                     Priority = 1,
        //                     LastModified = db.Properties.Where(a => a.IsExpired == false).Select(a => a.InsertDatetime).OrderByDescending(a => a.Date).FirstOrDefault()
        //                 });
        //    nodes.Add(
        //                 new SitemapNode()
        //                 {
        //                     Url = "https://www.limoonad.com",// Url.Action("cat", "search", new { id = item.CategorySlug }, "https"),
        //                     Frequency = SitemapFrequency.Hourly,
        //                     Priority = 1,
        //                     LastModified = db.Courses.Where(a => a.CourseStatus == 1).Select(a => a.CoursePublishDate).OrderByDescending(a => a.Value).FirstOrDefault()
        //                 });
        //    var Uppcats = db.Categories.Where(a => a.CategoryLevel == 1 && a.CategoryIsShow == true).ToList();
        //    foreach (var item in Uppcats)
        //    {
        //        if (db.Courses.Any(a => a.CategoryId == item.CategoryID))
        //        {
        //            var lastPublishedCourseDate = db.Courses.Where(a => a.CourseStatus == 1 && a.CategoryId == item.CategoryID).Select(a => a.CoursePublishDate).OrderByDescending(a => a.Value).FirstOrDefault();
        //            if (lastPublishedCourseDate != null)
        //            {
        //                nodes.Add(
        //                 new SitemapNode()
        //                 {
        //                     Url = Url.Action("cat", "search", new { id = item.CategorySlug }, "https"),
        //                     Frequency = SitemapFrequency.Daily,
        //                     Priority = 0.8,
        //                     LastModified = lastPublishedCourseDate
        //                 });
        //            }
        //        }
        //    }

        //    return nodes;
        //}
        private void UpdateSiteMapIndex()
        {
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(Server.MapPath("~/sitemapindex.xml"));
                XmlNode node;
                node = myXmlDocument.DocumentElement;
                foreach (XmlNode node1 in node.ChildNodes)
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name == "lastmod")
                        {

                            node2.InnerText = MyDateTimeExtensions.ConvertDateTimeToString(DateTime.UtcNow);
                        }
               
                    myXmlDocument.Save(Server.MapPath("~/sitemapindex.xml"));
               
            }
            catch 
            {
                
            }
           
        }
    }
}