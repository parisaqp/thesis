using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    [MetadataType(typeof(PropertyViewModel))]
    public partial class Property
    { }
    public  class PropertyViewModel
    {
        [Display(Name ="کد ملک")]
        public long PropertyID { get; set; }
        [Display(Name = "نام ملک")]
        public string PropertyName { get; set; }
        [Display(Name = "قیمت ملک به دلار")]
        public long PropertyPrice { get; set; }
        public string UserID { get; set; }
        [Display(Name = "موقت است")]
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
        [Display(Name = "روف گاردن دارد")]
        public bool PropertyRoofGarden { get; set; }
        [Display(Name = "لابی دارد")]
        public bool PropertyHasLobby { get; set; }
        [Display(Name = "لابی من دارد")]
        public bool PropertyHasLobbyMan { get; set; }
        [Display(Name = "نگهبان دارد")]
        public bool PropertyHasGaurd { get; set; }
        [Display(Name = "سالن ورزش دارد")]
        public bool PropertyHasGym { get; set; }

        [Display(Name = "وضعیت مبله")]
        public byte PropertyIsFernished { get; set; }

        [Display(Name = "کد ملک خودمان")]
        public int PropertyOurCode { get; set; }
        [Display(Name = "ارزش ملک از دید ما از 100")]

        [Range(0, 100, ErrorMessage = "امتیاز از 0 تا 100 می باشد")]
        public int PropertyOurPoint { get; set; }
        [Display(Name = "کد نقشه")]
        public Nullable<long> MapID { get; set; }
        [Display(Name = "توضیح متنی")]
        public string PropertyDescription { get; set; }
        [Display(Name = "تعداد حمام")]
        public int PropetryBathRooms { get; set; }

        [Display(Name = "اینترنت")]
        public string PropertyWiFi { get; set; }
        [Display(Name = "کوتاه مدت است")]
        public bool PropertyIsShortTerm { get; set; }
        [Display(Name = "قیمت روزانه")]
        public int PropertyPriceShortDaily { get; set; }
        [Display(Name = "قیمت ماهانه")]
        public int PropertyPriceShortMonthly { get; set; }
        [Display(Name = "نوع ملک")]
        public virtual PropertyType PropertyType { get; set; }
        [Display(Name = "محله")]
        public virtual Area Area { get; set; }
    }
}