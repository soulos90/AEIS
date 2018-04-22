using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.Controllers
{
    
    public class SecurityController
    {
        private UsersController user = new UsersController();
        private static Security active;
        private HttpCookie activeCookie;
        public SecurityController()
        {
            active = new Security();
        }
        public string GetID()
        {
            string value = "";
            if (CheckLogin())
            {
                value = Security.ID;
            }
            return value;
        }

        public bool CheckLogin()
        {
            bool check = false;
            if (active==null)
            {
                active = new Security();
                Security.IsLoggedIn = check;
            }
            if(activeCookie == null)
                activeCookie = HttpContext.Current.Request.Cookies.Get("Status");
            if (activeCookie == null && Security.Cookie==null)
            {
                activeCookie = new HttpCookie("Status");
                if (Security.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                activeCookie["LoggedIn"] = "False";
                Security.Cookie = activeCookie;
                HttpContext.Current.Response.Cookies.Set(activeCookie);
            }
            else if(activeCookie == null)
            {
                activeCookie = Security.Cookie;
            }

            if (Security.IsLoggedIn == true)
            {
                check = true;
                activeCookie["ID"] = Security.ID;
                activeCookie["LoggedIn"] = "True";
                activeCookie["Hash"] = user.GetU(Security.ID).Passhash;
                if (Security.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                Security.Cookie = activeCookie;
                HttpContext.Current.Response.Cookies.Set(activeCookie);
            }
            else if (activeCookie["LoggedIn"].Equals("True"))
            {
                if (Security.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                Security.ID = activeCookie["ID"];
                Security.IsLoggedIn = true;
                Security.Cookie = activeCookie;
                HttpContext.Current.Response.Cookies.Set(activeCookie);
                check = true;
            }
            return check;
        }

        public HttpCookie Login(string ID)
        {
            if (active==null)
            {
                active = new Security();
            }
            Security.IsLoggedIn = true;
            if (activeCookie == null)
                activeCookie = HttpContext.Current.Request.Cookies.Get("Status");
            if (activeCookie == null && Security.Cookie == null)
            {
                activeCookie = new HttpCookie("Status");
                Security.Cookie = activeCookie;
            }
            else if (activeCookie == null)
            {
                activeCookie = Security.Cookie;
            }
            else
            {
                Security.Cookie = activeCookie = new HttpCookie("Status");
            }

            if (Security.Remember)
                activeCookie.Expires = DateTime.Now.AddMonths(4);
            else
                activeCookie.Expires = DateTime.Now.AddHours(8);
            activeCookie["LoggedIn"] = "True";
            activeCookie["ID"] = Security.ID = ID;
            HttpContext.Current.Response.Cookies.Set(activeCookie);
            return activeCookie;
        }
        public void Logout()
        {
            if (active==null)
            {
                 active = new Security();
            }
            Security.IsLoggedIn = false;
            if (activeCookie!=null)
            {
                activeCookie["LoggedIn"] = "False";
                if (Security.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                HttpContext.Current.Response.Cookies.Remove("Status");
            }
        }

    }
}