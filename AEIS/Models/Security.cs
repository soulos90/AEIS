using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StateTemplateV5Beta.Models
{
    public class Security
    {
        public static string ID { get; set; }
        public static HttpCookie Cookie { get; set; }
        public static bool IsLoggedIn { get; set; }
        public static bool Remember { get; set; }
        public Security()
        {
            IsLoggedIn = false;
            ID = null;
            Cookie = null;
            Remember = false;
        }

    }
}