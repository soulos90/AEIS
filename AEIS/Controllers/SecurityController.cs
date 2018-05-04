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
        private Security active;
        private HttpCookie activeCookie;
        public SecurityController()
        {
            active = new Security();
        }
        public SecurityController(Security Active)
        {
            active = Active;
        }
        public string GetID()
        {
            string value = "";
            if (CheckLogin())
            {
                value = active.ID;
            }
            return value;
        }
        public void SetRemember(bool RB)
        {
            active.Remember = RB;
        }
        public bool GetRemember()
        {
            return active.Remember;
        }
        public Security GetActive()
        {
            return active;
        }
        public bool CheckLogin()
        {
            bool check = false;
            if (active==null)
            {
                active = new Security();
                active.IsLoggedIn = check;
            }
            if(activeCookie == null)
                activeCookie = HttpContext.Current.Request.Cookies.Get("Status");
            if (activeCookie == null && active.Cookie==null)
            {
                activeCookie = new HttpCookie("Status");
                if (active.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                activeCookie["LoggedIn"] = "False";
                active.Cookie = activeCookie;
                HttpContext.Current.Response.Cookies.Set(activeCookie);
            }
            else if(activeCookie == null)
            {
                activeCookie = active.Cookie;
            }

            if (active.IsLoggedIn == true)
            {
                check = true;
                activeCookie["ID"] = active.ID;
                activeCookie["LoggedIn"] = "True";
                activeCookie["Hash"] = user.GetU(active.ID).PassHash;
                if (active.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                active.Cookie = activeCookie;
                HttpContext.Current.Response.Cookies.Set(activeCookie);
            }
            else if (activeCookie["LoggedIn"].Equals("True"))
            {
                if (active.Remember)
                    activeCookie.Expires = DateTime.Now.AddMonths(4);
                else
                    activeCookie.Expires = DateTime.Now.AddHours(8);
                active.ID = activeCookie["ID"];
                active.IsLoggedIn = true;
                active.Cookie = activeCookie;
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
            active.IsLoggedIn = true;
            if (activeCookie == null)
                activeCookie = HttpContext.Current.Request.Cookies.Get("Status");
            if (activeCookie == null && active.Cookie == null)
            {
                activeCookie = new HttpCookie("Status");
                active.Cookie = activeCookie;
            }
            else if (activeCookie == null)
            {
                activeCookie = active.Cookie;
            }
            else
            {
                active.Cookie = activeCookie = new HttpCookie("Status");
            }

            if (active.Remember)
                activeCookie.Expires = DateTime.Now.AddMonths(4);
            else
                activeCookie.Expires = DateTime.Now.AddHours(8);
            activeCookie["LoggedIn"] = "True";
            activeCookie["ID"] = active.ID = ID;
            HttpContext.Current.Response.Cookies.Set(activeCookie);
            return activeCookie;
        }
        public void Logout()
        {
            if (active==null)
            {
                 active = new Security();
            }
            active.IsLoggedIn = false;
            if (activeCookie!=null)
            {
                activeCookie["LoggedIn"] = "False";
                HttpContext.Current.Response.Cookies.Remove("Status");
            }
        }

    }
}