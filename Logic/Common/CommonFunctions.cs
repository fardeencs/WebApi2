using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Common
{
    public static class CommonFunctions
    {
        public static DateTime ConvertDateTimeFromString(string dateTime,string culture)
        {
          return Convert.ToDateTime(dateTime, System.Globalization.CultureInfo.GetCultureInfo(culture).DateTimeFormat);
        }

        public static DateTime GetDateInHiINCultre(this string dateTime)
        {
            return Convert.ToDateTime(dateTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
        }
    }
}
