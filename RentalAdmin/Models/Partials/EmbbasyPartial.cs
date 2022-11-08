using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public partial class Embassy
    {
        public string getName()
        {
            string theName = "The Embassy Of "+ this.EmbassyName ;

            return theName;
        }
        public string getTitle()
        {
            string theName = "The Embassy Of " + this.EmbassyName + " in Tehran";

            return theName;
        }
        public string getURl()
        {
            // string str = System.Configuration.ConfigurationManager.AppSettings.Get("websitenamemedia") + "/";
            // str +
            return "/"+ Models.StaticList.EmbassyStart+ this.EmbassyID.ToString() + "/" + infrastracture.ConvertString.GetSlug(getName());
        }
    }
}