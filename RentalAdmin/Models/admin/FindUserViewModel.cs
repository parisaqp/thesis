using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.admin
{
    public class FindUserViewModel
    {
        public IEnumerable<RentalAdmin.Models.AspNetUser> users { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string lname { get; set; }
        public string fname { get; set; }
        public string phonenumber { get; set; }
        
    }
}