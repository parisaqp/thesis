using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalAdmin.Models
{
    public class StaticList
    {
        public static string PhoneNumber = "989034910011";//09034910011
        public static string PhoneNumberGoodShape = "98 903 491 0011";
        public static string EmailToShow = "agent@rentalir.com";
        public static string RegionStart = "rent-home/";
        public static string PropertyStart = "rent-in-tehran/";
        public static string EmbassyStart = "embassy/";
        public const string GoogleRecaptchaSecretKey = "6Ld8z1IaAAAAAJE653oOSJQ3ojhmnYa52RwR3LyY";//"6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";// for test
        public const string GoogleRecaptchaSiteKey = "6Ld8z1IaAAAAAF7TGQNnpZDIwetZba4K2ZFqJ0P9";//"6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI";// for test
        public static string GoogleRecaptchaInputName = "g-recaptcha-response";
        public static string GoogleApiClientID = "519681743608-nlideego7q6urqifnm7t5j4j65916hfc.apps.googleusercontent.com";
        public static string GoogleApiSecretID = "Hq8_i1N1By2IZpPYckZI23Mb";
        public static readonly string ErrorInRegisterByExternalLogin = "خطا در ساخت حساب کاربری. بار دیگر تلاش برای ساخت حساب کاربری کنید";
        public static readonly string AfterExternalLoginSuccessRegister1 = "با ایمیل " + "{0}" + " برای شما حساب کاربری ایجاد شد";
        public static readonly string AfterExternalLoginSuccessRegister2 = " در فرصت مناسب اقدام به ایجاد پسورد کنید و اطلاعات شخصی نظیر نام را در ویرایش اطلاعات تکمیل کنید ";

        public static readonly string ErrorInCreateNewUser = "خطا در ایجاد کاربر جدید";
        public static string GetPropertyType(int typeID)
        {
            string PropertyType = null;
            switch (typeID)
            {
                case 0:

                default:
                    break;
            }
            return PropertyType;
        }
        public readonly static List<SelectListItem> FurnishType = new[]
        {
             new SelectListItem{Value="0",Text="none furnished"}
            ,new SelectListItem{Value="1",Text="semi furnished"}
            ,new SelectListItem{Value="2",Text="full furnished"}

        }.ToList();
        public static PagedList.Mvc.PagedListRenderOptions pagedlist = new PagedList.Mvc.PagedListRenderOptions()
        {
            UlElementClasses = new[] { "pagination justify-content-center" },
            LiElementClasses=new[] { "page-item mx-1" },
            ContainerDivClasses=new[] { "col-md-auto" },
            DisplayLinkToFirstPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
            MaximumPageNumbersToDisplay = 5,
            DisplayLinkToLastPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            LinkToFirstPageFormat = "<i class=\"fa fa-angle-double-left\" aria-hidden=\"true\"></i>",
        LinkToPreviousPageFormat = "<i class=\"fa fa-angle-left\" aria-hidden=\"true\"></i>",
        LinkToNextPageFormat= "<i class=\"fa fa-angle-right\" aria-hidden=\"true\"></i>",
        LinkToLastPageFormat= "<i class=\"fa fa-angle-double-right\" aria-hidden=\"true\"></i>",
            //FunctionToDisplayEachPageNumber = ClassHelper.ConvertNumber.ConvertNumToPersianNum,
            DisplayLinkToNextPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            DisplayLinkToPreviousPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            EllipsesFormat = ""
            
        };
        public static PagedList.Mvc.PagedListRenderOptions BootstrapPagedlist = new PagedList.Mvc.PagedListRenderOptions()
        {

            UlElementClasses = new[] { "pagination justify-content-center" },
            LiElementClasses = new[] { "page-item" },
            ContainerDivClasses = new[] { "pagination page-link" },
            DisplayLinkToFirstPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,

            MaximumPageNumbersToDisplay = 5,
            DisplayLinkToLastPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            LinkToPreviousPageFormat = "<",
            LinkToNextPageFormat = ">",
            LinkToFirstPageFormat = "first",
            LinkToLastPageFormat = "last",
            DisplayLinkToNextPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            DisplayLinkToPreviousPage = PagedList.Mvc.PagedListDisplayMode.IfNeeded,
            EllipsesFormat = "..."
        };
    }
}