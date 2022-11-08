using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.PageViewModel
{
    public class BlogViewModel
    {
        public Blog theBlog { get; set; }
        public List<RentalAdmin.Models.AreaWhitRelated> regions { get; set; }
        public string LanguageDisplay { get; set; }

        public IEnumerable<Blog> RelatedBlogs { get; set; }
    }

}