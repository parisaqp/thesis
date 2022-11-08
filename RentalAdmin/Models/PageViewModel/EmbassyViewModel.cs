using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.PageViewModel
{
    public class EmbassyViewModel
    {
        public Embassy embassy { get; set; }
        public Area area { get; set; }
        public AreaWhitRelated areaWhitRelated { get; set; }
        public Map map { get; set; }
        public IEnumerable<Property> properties{get;set;}
        public bool isCelebrate { get; set; }
    }
}