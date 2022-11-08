
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
//using Persia;
//using MD.PersianDateTime;

namespace RentalAdmin.helper
{
    // Minimum Persian Calendar date (Gregorian Calendar):  Friday, March 22, 0622
    //    Minimum Persian Calendar date (Persian Calendar):  Friday, 1/1/1 0:0:0
    //
    //    Maximum Persian Calendar date (Gregorian Calendar):  Friday, December 31, 9999
    //    Maximum Persian Calendar date (Persian Calendar):  Friday, 10/13/9378 0:59:59
    public static class MyDateTimeExtensions
    {
        /// <summary>
        /// دو تاریخ را از نوع datetime می گیرد و نشان می دهد اختلافشان را بصورت مثلا یک روز قبل
        /// date time avali zamane dynamic ast va dovomi zamane hal ast
        /// </summary>
        /// <param name="date">زمان اولیه</param>
        /// <param name="comparedTo">زمان ثانویه</param>
        /// <returns></returns>

        //public static string GetRelativeDateValue(DateTime date, DateTime comparedTo, string TimeZoneStandardName)
        //{

        //    TimeSpan diff = comparedTo.Subtract(date);

        //    if (diff.Days >= 7)

        //        return GetPersianDateInShortString(ConvertUtcDateToTimeZoneDate(TimeZoneStandardName, date));

        //    else if (diff.Days > 1)

        //        return ConvertNumber.convertDigitToPersianString(diff.Days) + " روز پیش";

        //    else if (diff.Days == 1)

        //        return "دیروز";

        //    else
        //        return GetPersianTimeInShortString(ConvertUtcDateToTimeZoneDate(TimeZoneStandardName, date));

        //    //    return string.Concat(diff.Hours, " ساعت پیش");

        //    //else if (diff.Minutes >= 60)

        //    //    return "بیش از یک ساعت قبل";

        //    //else if (diff.Minutes >= 5)

        //    //    return string.Concat(diff.Minutes, " دقیقه پیش");

        //    //if (diff.Minutes >= 1)

        //    //    return "دقایقی قبل";

        //    //else

        //    //    return "به تازگی";

        //}
        ///// <summary>
        /// این تابع زمان را بر حسب مرجع می گیرد و سپس زمان متناسب با تایم زون ایران را بر  می گرداند
        /// </summary>
        /// <param name="time">زمان برای تبدیل با نوع DateTime</param>
        /// <returns>زمان مناسب برای ایران را بر می گرداند</returns>
        public static DateTime ConvertUtcDateToTimeZoneDate(DateTime time)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            return TimeZoneInfo.ConvertTime(time, timeZoneInfo);
        }
        /// <summary>
        /// این تابع زمان را بر حسب مرجع می گیرد و سپس زمان متناسب با تایم زون کشور درخواستی را بر  می گرداند
        /// </summary>
        /// <param name="CountryID">آیدی کشور که می خواهد زمان متناسب آن بشود</param>
        /// <param name="time"زمان برای تبدیل با نوع DateTime</param>
        /// <returns></returns>
        //public static DateTime ConvertUtcDateToTimeZoneDate(int? CountryID, DateTime time)
        //{
        //    DateTime result = DateTime.UtcNow;
        //    if (CountryID == null)
        //        return time;
        //    string countryZone = new limonadeEntities().Countries.Where(a => a.CountryID == CountryID).Select(a => a.CountryTimeZone).FirstOrDefault();
        //    try
        //    {

        //        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(countryZone);
        //        TimeSpan utcOffset = timeZoneInfo.GetUtcOffset(time);
        //        result = new DateTime(time.Ticks + utcOffset.Ticks, DateTimeKind.Local);
        //    }
        //    catch
        //    {

        //    }
        //    return result;
        //}
        public static DateTime ConvertUtcDateToTimeZoneDate(string TimeZoneStandardName, DateTime time)
        {
            DateTime result = DateTime.UtcNow;
            if (TimeZoneStandardName == null)
                return time;
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneStandardName);
                TimeSpan utcOffset = timeZoneInfo.GetUtcOffset(time);
                result = new DateTime(time.Ticks + utcOffset.Ticks, DateTimeKind.Local);

            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// convert from time zone to utc
        /// </summary>
        /// <param name="CountryID"></param>
        /// <param name="time"></param>
        /// <returns></returns>
   
        
        /// <summary>
        /// convert from time zone to utc
        /// </summary>
        /// <param name="CountryID"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeZoneDateToUtcDate( DateTime time)
        {
            DateTime result = DateTime.UtcNow;
            try
            {

                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"); 
                TimeSpan utcOffset = timeZoneInfo.GetUtcOffset(time);
                result = new DateTime(time.Ticks - utcOffset.Ticks, DateTimeKind.Local);
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// ForSiteMap
        /// 2005-01-01 or 2004-10-01T18:23:17+00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToString(DateTime date)
        {
            
            //return date.Year.ToString() + "-" + date.Month.ToString() +
            //    "-" + date.Day.ToString() + "T" + date.Hour.ToString() + ":"
            //    + date.Minute.ToString() + ":" + date.Second.ToString() + "+00:00";
            return string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}",   date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second ) + "+00:00";
        }
        /// <summary>
        /// ForSiteMap
        /// 2005-01-01 or 2004-10-01T18:23:17
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToString2(DateTime date)
        {

            //return date.Year.ToString() + "-" + date.Month.ToString() +
            //    "-" + date.Day.ToString() + "T" + date.Hour.ToString() + ":"
            //    + date.Minute.ToString() + ":" + date.Second.ToString() + "+00:00";
            return string.Format("{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second)+".000Z";
        }
        /// <summary>
        /// convert datetime to 10/13 mah/rooz
        /// </summary>
        /// <param name="date"></param>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"> datetime</param>
        /// <returns>مثال: 2 اردبهشت 1395</returns>


        /// <param name="date"></param>
        /// <returns> 15 om az mahe ghabl ra bar mi gardanad.</returns>
        public static DateTime GetPersianDatePreviousMonth(DateTime date)
        {
            var result = DateTime.UtcNow;
            if (date.Year > 623 && date.Year < 9999)
            {
                PersianCalendar pc = new PersianCalendar();
                //string pcDate = pc.GetYear(date).ToString() + "/" + pc.GetMonth(date).ToString() + "/" + pc.GetDayOfMonth(date).ToString();
                var NowMonth = pc.GetMonth(date);
                var NowYear=pc.GetYear(date);
                if (NowMonth==1)
                {
                    NowMonth = 12;
                    NowYear--;
                }
                else
                {
                    NowMonth--;
                }
                result = new DateTime(NowYear, NowMonth, 15, 1, 1, 1);
                return result;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// این ۱۳۹۵/۰۲/۱۸ رو می گیرد زمان  بر می گردوند
        ///وقتی که برعکس فعال باشد، این ۱۸/۰۲/۱۳۹۵ رو می گیرد زمان بر می گردوند
        /// </summary>
        /// <param name="date"></param>
        /// <returns>datetime</returns>
       
        public static string GetNameOfPersianMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }
        


        public static string BeTowDigit(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "00";
            if (s.Length == 1)
            {
                return "0" + s;
            }
            return s;
        }

    }
}