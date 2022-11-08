using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalAdmin.Models;

namespace RentalAdmin.helper
{
    public class Logic
    {

        private RentalEntities db;
        public Logic(RentalEntities db)
        {
            this.db = db;
        }

    }
}