using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.PageViewModel
{
    public class PropertyViewModel
    {
        public long minprice { get; set; }
        public long maxprice { get; set; }
        public Property property { get; set; }
        public List<PropertyGallery> allPic { get; set; }
        public List<Metro> allMetro { get; set; }
        public List<Embassy> allEmbassy { get; set; }
        public Map map { get; set; }
        public Area region { get; set; }
        public AreaWhitRelated regionWithRelated { get; set; }
        public string getFirstPic()
        {
            if(allPic!=null&&allPic.Count>0)
            {
                return allPic.First().Upload.GetPublicUrl();
            }
            return "";
        }
        public PropertyType propertyType { get; set; }

        public IEnumerable<Property> sameProperties { get; set; }
        public IEnumerable<Property> samePropertiesNearNeighborhood { get; set; }
    }
}