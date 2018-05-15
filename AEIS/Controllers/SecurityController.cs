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
           
            return active.IsLoggedIn;
        }

        public void Login(string ID)
        {
            if (active==null)
            {
                active = new Security();
            }
            active.ID = ID;
            active.IsLoggedIn = true;
        }
        public void Login(Security ID)
        {
            active = ID;
            active.IsLoggedIn = true;
        }
        public DateTime GetEX()
        {
            if (active.Remember)
                return DateTime.Now.AddMonths(4);
            else
                return DateTime.Now.AddHours(8);
        }
    }
}