using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentalAdmin.Models;

namespace RentalAdmin.logic
{
    public class PropertyManager
    {
        private RentalEntities db = new RentalEntities();
        public PropertyManager(RentalEntities database)
        {
            db = database;
        }
        public System.Collections.Generic.List<Property> getSameProperty(Property theProperty,out long modelMinprice, out long modelMaxprice
            ,out int modelminbed,out int modelmaxbed)
        {
            int zarib = 1;
            if(theProperty.PropertyIsShortTerm==true)
            {
                zarib = 2;
            }
            //same property
            System.Collections.Generic.HashSet<Property> uniqueList = new System.Collections.Generic.HashSet<Property>();
            var temp1 = db.Properties.Where(a => a.PropertyRoom == theProperty.PropertyRoom && a.AreaID == theProperty.AreaID
            &&a.PropertyIsShortTerm== theProperty.PropertyIsShortTerm)
                .Take(4).ToList();
            if (temp1 != null && temp1.Count > 0)
            {
                foreach (var item in temp1)
                {
                    uniqueList.Add(item);
                }
            }
            int percent = 26*zarib; // 1800 - 1200
            if (theProperty.PropertyPrice < 1200)
            {
                percent = 40 * zarib;
            }
            if (theProperty.PropertyPrice > 1800)
            {
                percent = 28 * zarib;
            }
            if (theProperty.PropertyPrice > 2500)
            {
                percent = 28 * zarib;
            }
            if (theProperty.PropertyPrice > 3500)
            {
                percent = 32 * zarib;
            }
            if (theProperty.PropertyPrice > 5000)
            {
                percent = 40 * zarib;
            }
            if (theProperty.PropertyPrice > 7500)
            {
                percent = 50 * zarib;
            }
            long minprice = theProperty.PropertyPrice - ((theProperty.PropertyPrice * percent) / 100);
            long maxprice = theProperty.PropertyPrice + ((theProperty.PropertyPrice * percent) / 100);
            int minbed = theProperty.PropertyRoom;
            int maxbed = theProperty.PropertyRoom;
            if (theProperty.PropertyType.PropertyTypeName.Contains("villa"))
            {
                minbed -= 4;
                maxbed += 4;
            }
            modelMinprice = minprice;
            modelMaxprice = maxprice;
            temp1 = db.Properties.Where(a => a.IsExpired == false &&
            a.PropertyTypeID == theProperty.PropertyTypeID &&
            a.PropertyPrice > minprice && a.PropertyPrice < maxprice && a.AreaID == theProperty.AreaID
            &&a.PropertyIsShortTerm==theProperty.PropertyIsShortTerm)
              .Take(6).ToList();
            if (temp1 != null && temp1.Count > 0)
            {
                foreach (var item in temp1)
                {
                    uniqueList.Add(item);
                }
            }
            if (uniqueList.Count < 5)
            {
                temp1 = db.Properties.
                    Where(a => a.IsExpired == false &&
                    a.PropertyTypeID == theProperty.PropertyTypeID 
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
                   && (a.PropertyRoom >= minbed - 1 && a.PropertyRoom <= maxbed + 1) && a.AreaID == theProperty.AreaID)
                .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueList.Add(item);
                    }
                }
            }
            if (uniqueList.Count < 4)
            {
                minprice = theProperty.PropertyPrice - ((theProperty.PropertyPrice * (percent + 10)) / 100);
                maxprice = theProperty.PropertyPrice + ((theProperty.PropertyPrice * (percent + 10)) / 100);
                modelMinprice = minprice;
                modelMaxprice = maxprice;
                temp1 = db.Properties.Where(a => a.IsExpired == false &&
                a.PropertyTypeID == theProperty.PropertyTypeID
                && a.PropertyPrice > minprice && a.PropertyPrice < maxprice && a.AreaID == theProperty.AreaID
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
                    )
                  .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueList.Add(item);
                    }
                }
            }


            if (uniqueList.Count < 2 &&
                (theProperty.PropertyType.PropertyTypeName.Contains("villa") || theProperty.PropertyType.PropertyTypeName.Contains("office")))
            {
                temp1 = db.Properties.Where(a => a.PropertyTypeID == theProperty.PropertyTypeID
                && a.AreaID == theProperty.AreaID

                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm)
              .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueList.Add(item);
                    }
                }
            }
            var itself = uniqueList.Where(a => a.PropertyID == theProperty.PropertyID).FirstOrDefault();
            if (itself != null)
            {
                uniqueList.Remove(itself);
            }
            modelminbed = minbed;
            modelmaxbed = maxbed;
            return uniqueList.OrderByDescending(a => a.PropertyOurPoint).Take(6).ToList();
            

        }
        public System.Collections.Generic.List<Property> getSamePropertyNearNeighborhood(Property theProperty, AreaWhitRelated  regionWithRelated
            ,  long minprice,  long maxprice,int minbed,int maxbed)
        {
            System.Collections.Generic.HashSet<Property> uniqueListNearNeighborhood = new System.Collections.Generic.HashSet<Property>();
            // similar property nearby neighborhood

            System.Collections.Generic.List<int> AreaList = new System.Collections.Generic.List<int>();

            if (regionWithRelated != null &&
                regionWithRelated.AreaRelatedList != null &&
                regionWithRelated.AreaRelatedList.Count > 0)
            {
                AreaList.AddRange(regionWithRelated.AreaRelatedList.Select(a => a.AreaID).ToList());
            }

            //minprice = theProperty.PropertyPrice - theProperty.PropertyPrice / percent;
            //    maxprice = theProperty.PropertyPrice + theProperty.PropertyPrice / percent;
           var  temp1 = db.Properties.Where(a => a.IsExpired == false
            && AreaList.Any(c => c == a.AreaID)
           && a.PropertyTypeID == theProperty.PropertyTypeID
            && a.PropertyPrice > minprice && a.PropertyPrice < maxprice
           && (a.PropertyRoom >= minbed && a.PropertyRoom <= maxbed)
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
            )
              .Take(6).ToList();
            if (temp1 != null && temp1.Count > 0)
            {
                foreach (var item in temp1)
                {
                    uniqueListNearNeighborhood.Add(item);
                }
            }
            if (uniqueListNearNeighborhood.Count < 5)
            {
                temp1 = db.Properties.
                    Where(a => a.IsExpired == false &&
                     AreaList.Any(c => c == a.AreaID)
                   && a.PropertyTypeID == theProperty.PropertyTypeID &&
                    a.PropertyRoom == theProperty.PropertyRoom
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm)
                .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueListNearNeighborhood.Add(item);
                    }
                }
            }
            if (uniqueListNearNeighborhood.Count < 6)
            {
                temp1 = db.Properties.
                    Where(a => a.IsExpired == false &&
                     AreaList.Any(c => c == a.AreaID)
                   && a.PropertyTypeID == theProperty.PropertyTypeID &&
                    (a.PropertyRoom >= minbed - 1 && a.PropertyRoom <= maxbed + 1)
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
                    )
                .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueListNearNeighborhood.Add(item);
                    }
                }
            }


            if (uniqueListNearNeighborhood.Count < 2 &&
                (theProperty.PropertyType.PropertyTypeName.Contains("villa") || theProperty.PropertyType.PropertyTypeName.Contains("office")))
            {
                temp1 = db.Properties.Where(a => a.PropertyTypeID == theProperty.PropertyTypeID
                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
                    )
              .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueListNearNeighborhood.Add(item);
                    }
                }
            }
            if(uniqueListNearNeighborhood.Count<2)
            {
                temp1 = db.Properties.
                   Where(a => a.IsExpired == false 
                  && a.PropertyTypeID == theProperty.PropertyTypeID &&
                   (a.PropertyRoom >= minbed - 1 && a.PropertyRoom <= maxbed + 1)
                   && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm
                   )
               .Take(6).ToList();
                if (temp1 != null && temp1.Count > 0)
                {
                    foreach (var item in temp1)
                    {
                        uniqueListNearNeighborhood.Add(item);
                    }
                }
            }
            var itself = uniqueListNearNeighborhood.Where(a => a.PropertyID == theProperty.PropertyID

                    && a.PropertyIsShortTerm == theProperty.PropertyIsShortTerm).FirstOrDefault();
            if (itself != null)
            {
                uniqueListNearNeighborhood.Remove(itself);
            }
           
            return uniqueListNearNeighborhood.OrderByDescending(a => a.PropertyOurPoint).Take(6).ToList();
        }
    }
}