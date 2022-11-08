using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models.PageViewModel
{
    public class SearchViewModel
    {
        public string AreaName { get; set; }
        public long? PropertyID { get; set; }
        public string PropertyName { get; set; }
        public long? PropertyPrice { get; set; }
        public string UserID { get; set; }
        public bool? IsExpired { get; set; }
        public System.DateTime? InsertDatetime { get; set; }
        public Nullable<int> PropertyTypeID { get; set; }
        public List<int?> PropertyTypeIDList { get; set; }
        public bool? IsSpecial { get; set; }
        public string WhyIsSpecial { get; set; }
        public List< int> AreaIDs { get; set; }
        public int? PropertySpace { get; set; }
        public byte? PropertyRoom { get; set; }
        public bool? PropertyPool { get; set; }
        public int? PropertyFloor { get; set; }
        public int? PropertyAllFloor { get; set; }
        public byte? PropertyAge { get; set; }
        public List<string> PropertyAgeStr { get; set; }
        public byte? PropertyUnitsPerFloor { get; set; }
        public byte? PropertyParkingNumber { get; set; }
        public bool? PropertySauna { get; set; }
        public bool? PropertyJacuzzi { get; set; }
        public bool? PropertyRoofGarden { get; set; }
        public bool? PropertyHasLobby { get; set; }
        public bool? PropertyHasLobbyMan { get; set; }
        public bool? PropertyHasGaurd { get; set; }
        public bool? PropertyHasGym { get; set; }
        public bool? PropertyIsShortTerm { get; set; }
        public List<byte> PropertyFurnisheType { get; set; }
        public List<byte?> PropertyBedroomList { get; set; }
        public int? PropertyOurCode { get; set; }
        public string Stxt { get;set; }
        public List< int?> pricemin { get; set; }
        public int? pricemax { get; set; }
        public IEnumerable<Property> properties { get; set; }

    }
}