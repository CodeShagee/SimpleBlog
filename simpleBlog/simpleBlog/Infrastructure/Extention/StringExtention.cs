using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace simpleBlog.Infrastructure.Extention
{
    public static class StringExtention
    {
        public static string Slugify(this string that)
        {
            that = Regex.Replace(that, @"[^a-zA-Z0-9\s]", "");
            that = that.ToLower();
            that = Regex.Replace(that, @"\s", "-");
            return that;
        }
    }
}