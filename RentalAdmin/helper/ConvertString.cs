using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.helper
{
    public class ConvertString
    {
        public static string MyTrimToLower(string txt)
        {
            string result = "";
            if (string.IsNullOrEmpty(txt))
                return txt;
            else
                result = System.Text.RegularExpressions.Regex.Replace(txt, @"\s+", " ").Trim();
            if (result == null)
                return result;
            else
            {
                return result.ToLower();
            }
        }
    }
}