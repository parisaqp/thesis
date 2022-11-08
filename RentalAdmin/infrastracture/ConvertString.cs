using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.infrastracture
{
    public static class ConvertString
    {
        public static string GetSlug(string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                str = str.Replace(" ", "-");
                str = str.Replace("(", "-");
                str = str.Replace(")", "-");
                str = str.Replace("'", "");
                str = str.Replace("|", "");
                str = str.Replace("_", "");

                while (str.Contains("--"))
                {
                    str = str.Replace("--", "-");
                }
                if (str.EndsWith("-"))
                {
                    str = str.Remove(str.Length - 1, 1);
                }
            }
            return str;
        }
    }
}