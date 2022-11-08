using System.Data;
using System.Linq;

namespace RentalAdmin.Models
{
    public partial class Property
    {
        public string getName()
        {
            string theName = PropertyType.PropertyTypeName;

            if (Area != null)
            {
                theName += " in " + Area.AreaName;
            }
            theName += " ID " + PropertyOurCode;
            if (IsSpecial)
            {
                if (!string.IsNullOrEmpty(WhyIsSpecial))
                {
                    string tempSpecial = WhyIsSpecial;
                    if (WhyIsSpecial.Length > 16)
                    {
                        tempSpecial = WhyIsSpecial.Substring(0, 15);
                    }
                    theName += " - " + WhyIsSpecial;
                }
            }
            if (PropertyIsFernished == 2)
            {
                theName = "Furnished " + theName;
            }

            return theName;
        }

        public string getNameMax()
        {
            string theName = getName();
            if(theName.Length>39)
            {
                theName= theName.Replace("Apartment", "Apt");
            }

            return theName;
        }

        public System.Collections.Generic.List<string> getTops()
        {
            System.Collections.Generic.List<string> res = new System.Collections.Generic.List<string>();
            //if (IsSpecial)
            //{
            //    string whyIsSpecial = WhyIsSpecial;
            //    if (!string.IsNullOrEmpty(whyIsSpecial))
            //    {
            //        if (whyIsSpecial.Length > 16)
            //        {
            //            whyIsSpecial = whyIsSpecial.Substring(0, 15);
            //        }
            //        res.Add(WhyIsSpecial);
            //    }
            //}
            res.Add(PropertyRoom.ToString() + " " + "Rooms");

            if (PropertyRoofGarden)
            {
                res.Add("Roof Garden");
            }
            if(PropertyPool)
            {
                res.Add("Pool");
            }
            //this.PropertyMetroes.
            return res;
        }
        public string getURl()
        {
            // string str = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia") + "/";
            // str +
            return "/" + Models.StaticList.PropertyStart + PropertyID.ToString() + "/" + infrastracture.ConvertString.GetSlug(getName().Replace("ID",""));
        }
        public string getPrice()
        {
           return string.Format("{0:n0}", PropertyPrice);
        }
        public string getPriceShort()
        {
            return string.Format("{0:n0}", PropertyPriceShortMonthly);
        }
        public string getPriceShortDaily()
        {
            return string.Format("{0:n0}", PropertyPriceShortDaily);
        }
        public string getDescription()
        {
            string res = "The ";
            if (PropertyIsFernished > 0)
            {
                res += RentalAdmin.Models.StaticList.FurnishType.Where(a => a.Value == PropertyIsFernished.ToString()).First().Text.ToLower();
            }
            res+=" "+ PropertyType.PropertyTypeName.ToLower() + " has " + PropertyRoom +" "+ helper.StringNumberConvertot.GetPlural("bedroom", PropertyRoom) ;
            res += " and the rent is $" + getPrice() + " per month";
            //res += " (" + Area.AreaName + ")";
            if (IsSpecial && !string.IsNullOrEmpty(WhyIsSpecial))
            {
                res += " (" + WhyIsSpecial + ")";
            }
            res += ".";
            return res;
        }
        public string getDescriptionMeta()
        {
            string res = getDescription();
            res += ". this property " + PropertyType.PropertyTypeName + " is located in the " +this.Area.AreaName + " neighborhood";
            res += "(";
            if (PropertyHasGaurd)
            {
                res += "Gaurd,";
            }
            if (PropertyHasGym)
            {
                res += "Gym,";
            }
            if (PropertyPool)
            {
                res += "Pool,";
            }
            if (PropertySauna)
            {
                res += "Sauna,";
            }
            res += ")";
            return res;
        }
        public string getFloor()
        {
            if(PropertyFloor==PropertyAllFloor&&PropertyAllFloor>2)
            {
                return "top";
            }
            if(PropertyFloor==1)
            {
                return "first";
            }
            if(PropertyFloor==2)
            {
                return "second";
            }
            if (PropertyFloor == 3)
            {
                return "third";
            }
            return PropertyFloor.ToString() + "th";
        }
    }
}