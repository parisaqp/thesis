using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public class PropertySearchViewModel
    {
        [Display(Name = "کد ملک")]
        public long PropertyID { get; set; }
        [Display(Name = "نام ملک")]
        public string PropertyName { get; set; }
        [Display(Name = "قیمت ملک")]
        public long PropertyPrice { get; set; }
        public string UserID { get; set; }
        [Display(Name = "منقضی شده")]
        public bool IsExpired { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime InsertDatetime { get; set; }
        [Display(Name = "نوع ملک")]
        public Nullable<int> PropertyTypeID { get; set; }
        [Display(Name = "ویژه")]
        public bool IsSpecial { get; set; }
        [Display(Name = "توضیح ویژه")]
        public string WhyIsSpecial { get; set; }
        [Display(Name = "کد محله")]
        public int AreaID { get; set; }
        [Display(Name = "مساحت ملک")]
        public int PropertySpace { get; set; }
        [Display(Name = "تعداد خواب")]
        public byte PropertyRoom { get; set; }
        [Display(Name = "استخر دارد")]
        public bool PropertyPool { get; set; }
        [Display(Name = "طبقه چندم است")]
        public int PropertyFloor { get; set; }
        [Display(Name = "ساختمان چند طبقه است")]
        public int PropertyAllFloor { get; set; }
        [Display(Name = "سن ملک")]
        public byte PropertyAge { get; set; }
        [Display(Name = "تعداد واحد در هر طبقه")]
        public byte PropertyUnitsPerFloor { get; set; }
        [Display(Name = "تعداد پارکینگ")]
        public byte PropertyParkingNumber { get; set; }
        [Display(Name = "سونا دارد")]
        public bool PropertySauna { get; set; }
        [Display(Name = "جکوزی دارد")]
        public bool PropertyJacuzzi { get; set; }
        public bool PropertyRoofGarden { get; set; }
        public bool PropertyHasLobby { get; set; }
        public bool PropertyHasLobbyMan { get; set; }
        public bool PropertyHasGaurd { get; set; }
        public bool PropertyHasGym { get; set; }
        [Display(Name = "کد نقشه")]
        public Nullable<long> MapID { get; set; }


        public Nullable<long> PropertyPriceMax { get; set; }

        public Nullable<System.DateTime> InsertDatetimeMax { get; set; }

        public Nullable<int >PropertySpaceMax { get; set; }
        public Nullable<byte> PropertyRoomMax { get; set; }
        public Nullable<int> PropertyAllFloorMax { get; set; }
        public Nullable<byte> PropertyAgeMax { get; set; }
        public Nullable<byte> PropertyUnitsPerFloorMax { get; set; }
        public Nullable<byte> PropertyParkingNumberMax { get; set; }

    }
}