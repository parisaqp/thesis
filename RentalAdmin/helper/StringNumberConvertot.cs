using System;
using System.Text.RegularExpressions;

namespace RentalAdmin.helper
{
    public static class StringNumberConvertot
    {
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static string PreperSlug(string str)
        {
            if (!string.IsNullOrEmpty(str))
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
        public static double doubleToOneDesimal(double num)
        {
            double theNum = num;
            double rounded = ((int)(theNum * 10 + 5) / 10.0);
            return theNum;
        }

        public static string getUrlWihtoutPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            if (url.Contains("page="))
            {
                url = url.Replace("page=", "bpage=");
            }
            return url;
        }
        public static double maxAndMin(double num, double max, double min)
        {
            if (num > max)
                return max;
            if (num < min)
                return min;
            return num;
        }
        public static string getwhatsapptxt(string str)
        {
            //str=str.Replace
            return str;
        }

        public static string GetPlural(string str,long num)
        {
            if(num>1)
            {
                switch (str.ToLower())
                {
                    case "embassy":
                        return "embassies";
                    default:
                        return str + "s";
                }
            }
            return str;
        }
    }
}