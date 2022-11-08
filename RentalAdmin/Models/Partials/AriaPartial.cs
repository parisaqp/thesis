using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalAdmin.Models
{

    [MetadataType(typeof(AreaViewModel))]
    public partial class Area
    {
        //Read more about Elahiyeh in Tehran and see rental Properties.
        //Weather of Elahiyeh is nice and ten embassies are located in this area which represents it's quality.
        //The number of offices and villas in this neighborhood is high.

        public string getName()
        {
            string theName = AreaName + " in Tehran";

            return theName;
        }
        public string getAbout()
        {
            string tagp = "<p class='region_page_context_comment' >";
            string tagpend = "</p>";
            string theName = AreaDescription;
            if(!string.IsNullOrEmpty(theName))
            {
                if (!theName.StartsWith("<p"))
                {
                    theName = tagp + AreaDescription + tagpend;
                }
                theName.Replace("<p>", tagp);
            }
          

            return theName;
        }

        public string getTitle()
        {
            return "Read more about " + getName() + " and see rental Properties.";
        }
        public string getTitleShort()
        {
            return "Read more about " + this.AreaName + " and see rental Properties";
        }
        public string getAboutMeta()
        {
            string theName = "";
            if (!string.IsNullOrEmpty(AreaDescription))
            {
                var temp = helper.StringNumberConvertot.StripHTML(AreaDescription);
                theName += temp;
            }
            else
            {
                theName = GetAboutFromDatabase();
            }

            if (theName.Length > 130)
            {
                theName = theName.Substring(0, 128);
            }
            return theName;
        }
        public string GetAboutFromDatabase()
        {
            string res = getTitle();
            if (Climate == "clean")
            {
                res += " Weather of "+ AreaName + " is nice and ";
            }
            if (Embassies.Count>2)
            {
                res +=helper.IntegerToWrittenNumber.IntegerToWritten( Embassies.Count) + " embassies are located in this area which represents it's quality.";
            }
            //else
            //{
            //    res += " " + AreaName;
            //}
            bool isVillaOrOffice = false;
            if(Office == "many"|| Villa == "many")
            {
                res += " The number of ";
                isVillaOrOffice = true;
            }
            bool needand = false;
            if (Office == "many")
            {
                res +=  " offices ";
                needand = true;
            }
            if(Villa=="many")
            {
                if(needand)
                {
                    res += " and ";
                }

                res += " villas ";
            }
            if(isVillaOrOffice)
            {
                res += " in this neighborhood is high.";
            }
            return res;
        }
        public string getURl()
        {
            //string str = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia") + "/";str;// +
            return "/" + Models.StaticList.RegionStart + AreaID.ToString() + "/" + infrastracture.ConvertString.GetSlug(getName());
        }
        public string getClass(string forwhat,string value)
        {
            string theClass = "";
            if (value == null)
                return theClass;
            switch (forwhat)
            {
                case "TrafficCongestion":
                    theClass = "back";
                    
                    if(value.Contains( "heavy"))
                    {
                        theClass += "red";
                    }
                    if(value == "traffic")
                    {
                        theClass += "yellow";//
                    }
                    if (value == "good"||value.Contains( "smooth"))
                    {
                        theClass += "green";
                    }
                    break;
                case "Climate":
                    theClass = "back";
                    if (value.Contains("polluted")|| value == "bad")
                    {
                        theClass += "red";
                    }
                    if (value == "normal")
                    {
                        theClass += "yellow";//
                    }
                    if (value == "good"|| value == "clean")
                    {
                        theClass += "green";
                    }
                    break;
                case "Embassies":
                    theClass = "back";
                    int a = 0;
                    int.TryParse(value, out a);
                    if(a==0)
                    {
                        theClass += "red";
                    }
                    if(a==1)
                    {
                        theClass += "yellow";//
                    }
                    if(a>1)
                    {
                        theClass += "green";
                    }
                    break;
                default:
                    break;
            }
            return theClass;
        }

    }
    public class AreaViewModel
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public int AreaOrder { get; set; }
        [System.Web.Mvc.AllowHtml]
        public string AreaDescription { get; set; }
        public string AreaSlug { get; set; }
        public string AreaNameInPersian { get; set; }
        public string AreaSeoTitle { get; set; }
        public Nullable<long> MapID { get; set; }
        public string ImageSmall { get; set; }
        public string ImageBig { get; set; }
        public Nullable<long> ImageSmallUploadID { get; set; }
        public Nullable<long> ImageBigUploadID { get; set; }
    }

    public class AreaWhitRelated: Area
    {
        public AreaWhitRelated(Area ob)
        {
            this.AreaDescription = ob.AreaDescription;
            this.AreaID = ob.AreaID;
            this.AreaName = ob.AreaName;
            this.AreaNameInPersian = ob.AreaNameInPersian;
            this.AreaOrder = ob.AreaOrder;
            this.AreaRelations = ob.AreaRelations;
            this.AreaRelations1 = ob.AreaRelations1;
            this.AreaSeoTitle = ob.AreaSeoTitle;
            this.AreaSlug = ob.AreaSlug;
            this.ArticleRelations = ob.ArticleRelations;
            this.Climate = ob.Climate;
            this.Embassies = ob.Embassies;
            this.FixedPropertyPrice = ob.FixedPropertyPrice;
            this.ImageBig = ob.ImageBig;
            this.ImageBigUploadID = ob.ImageBigUploadID;
            this.ImageSmall = ob.ImageSmall;
            this.ImageSmallUploadID = ob.ImageSmallUploadID;
            this.Map = ob.Map;
            this.MapID = ob.MapID;
            this.Metroes = ob.Metroes;
            this.Office = ob.Office;
            this.Properties = ob.Properties;
            this.TrafficCongestion = ob.TrafficCongestion;
            this.Villa = ob.Villa;
            this.WelfareAmenities = ob.WelfareAmenities;
        }
        public string AreaRelated { get; set; }
        public List< Area> AreaRelatedList { get; set; }
    }
}