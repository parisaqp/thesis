using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public partial class Property
    {
        public string getName()
        {
            string theName = this.PropertyType.PropertyTypeName;
           
            if(this.Area!=null)
            {
                theName +=  " in " + Area.AreaName;
            }
            theName += " code " + this.PropertyID;
            return theName;
        }
        public string getURl()
        {
            string str = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia") + "/";
          return str+ "property/in-iran/tehran/" + this.PropertyID.ToString() + "/"+infrastracture.ConvertString.GetSlug(getName());
        }
    }
}