using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Controllers;

namespace StateTemplateV5Beta.Models
{
    public class SecurityCombined
    {
        public string ID { get; set; }
        public HttpCookie Cookie { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool Remember { get; set; }

        public SecurityCombined()
        {
            SetDefaultValues();
        }

        public void SetDefaultValues()
        {
            ID = null;
            Cookie = null;
            IsLoggedIn = false;
            Remember = false;
        }

        public bool CheckLogin()
        {
            if (Cookie == null)
                Cookie = HttpContext.Current.Request.Cookies.Get("Status");

            if (Cookie == null)
            {
                Cookie = new HttpCookie("Status");
                Cookie["LoggedIn"] = "False";
                SetCookieExpiration();
            }

            if (IsLoggedIn == true)
            {
                Cookie["LoggedIn"] = "True";
                Cookie["ID"] = ID;
                SetCookieExpiration();
            }
            else if (Cookie["LoggedIn"].Equals("True"))
            {
                IsLoggedIn = true;
                ID = Cookie["ID"];
                SetCookieExpiration();
            }

            return IsLoggedIn;
        }

        public HttpCookie Login(string ID)
        {
            IsLoggedIn = true;

            if (Cookie == null)
                Cookie = HttpContext.Current.Request.Cookies.Get("Status");

            Cookie = new HttpCookie("Status");
            Cookie["LoggedIn"] = "True";
            Cookie["ID"] = this.ID = ID;
            SetCookieExpiration();

            return Cookie;
        }

        private void SetCookieExpiration()
        {
            HttpContext.Current.Response.Cookies.Set(Cookie);
            if (Remember)
                Cookie.Expires = DateTime.Now.AddMonths(4);
            else
                Cookie.Expires = DateTime.Now.AddHours(8);
        }

        public void Logout()
        {
            if (Cookie != null)
            {
                Cookie = new HttpCookie("Status");
                HttpContext.Current.Response.Cookies.Remove("Status");
            }

            SetDefaultValues();
        }
    }
}