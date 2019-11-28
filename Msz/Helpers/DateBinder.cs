using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Helpers
{
    public static class DateBinder
    {
        public static DateTime? FromUrl(string paramName)
        {
            var startDateStr = AppContext.Current.Request.Query[paramName].ToString();
            if (DateTime.TryParse(startDateStr, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime startDate))
            {
                return startDate;
            }
            return null;
        }
    }
}
