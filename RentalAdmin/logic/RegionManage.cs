using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalAdmin.Models;

namespace RentalAdmin.logic
{
    public class RegionManage
    {

        private RentalEntities db = new RentalEntities();
        public RegionManage (RentalEntities database)
        {
            db = database;
        }
        public AreaWhitRelated GetAreaWhitRelated(Area theArea)
        {
            AreaWhitRelated temp = new AreaWhitRelated(theArea);
            var allrelated = db.AreaRelations.Include(a => a.Area).Include(a => a.Area1)
                .Where(a => a.AreaID1 == theArea.AreaID || a.AreaID2 == theArea.AreaID).ToList();
            var listTemp = new HashSet<Area>();
            temp.AreaRelated = "";
            if (allrelated != null && allrelated.Count > 0)
            {
                System.Collections.Generic.HashSet<string> areaname = new System.Collections.Generic.HashSet<string>();
                foreach (var therelated in allrelated)
                {
                    listTemp.Add(therelated.Area);
                    listTemp.Add(therelated.Area1);
                    areaname.Add(therelated.Area.AreaName);
                    areaname.Add(therelated.Area1.AreaName);
                }
                if (areaname.Any(a => a == theArea.AreaName))
                {
                    areaname.Remove(theArea.AreaName);
                    listTemp.Remove(theArea);
                }
                temp.AreaRelatedList = listTemp.ToList();
                temp.AreaRelated = string.Join(", ", areaname.ToList());
            }
            return temp;
        }
    }
}