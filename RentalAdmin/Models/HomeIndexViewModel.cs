using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public class HomeIndexViewModel
    {
        public List<Property> TopProperties { get; set; }
        public List<Property> VillaProperties { get; set; }
        public List<Property> NewProperties { get; set; }
        public List<Property> FurinshedProperties { get; set; }
        public List<Property> ShortTermProperties { get; set; }
        public List<Property> LuxuryProperties { get; set; }
        public List<RentalAdmin.Models.ComplexBlog> TopTour { get; set; }
        public List<Models.Area> topRegions { get; set; }
        public Embassy BirthDayEmbassy { get; set; }
        public List<Blog> TopBlogs { get; set; }
    }
}