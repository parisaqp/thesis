using RentalAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace RentalAdmin.helper
{
    public class UpdateSiteMapIndex : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(filterContext.HttpContext.Server.MapPath("~/sitemapindex.xml"));
                XmlNode node;
                node = myXmlDocument.DocumentElement;
                foreach (XmlNode node1 in node.ChildNodes)
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name == "lastmod")
                        {
                            node2.InnerText = MyDateTimeExtensions.ConvertDateTimeToString(DateTime.UtcNow);
                        }
                myXmlDocument.Save(filterContext.HttpContext.Server.MapPath("~/sitemapindex.xml"));
            }
            catch 
            {
            }
        }
    }
}