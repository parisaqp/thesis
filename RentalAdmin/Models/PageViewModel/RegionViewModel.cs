using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.PageViewModel
{
    public class RegionViewModel
    {
        public Models.Area region { get; set; }
        public Models.AreaWhitRelated regionWithRelated { get; set; }
        public IEnumerable<Models.Property> properties { get; set; }

        public List<Embassy> allEmbassy { get; set; }
        public int propertiesCount { get; set; }
        public int metroCount { get; set; }
        public int embassyCount { get; set; }
    }
}