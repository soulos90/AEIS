﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.Controllers
{
    public class LoginController : Controller
    {
        UsersController UController = new UsersController();
        SecurityController SController = new SecurityController();
        [HttpPost]
        public ActionResult PostUser(User user)
        {
            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser == null)
                {
                    HttpCookie pass = SController.Login(user.ID);
                    UController.PostUser(user);
                    //TODO: redirect in a way that creates a security token
                    return View("LoggedIn");
                }
                return View("LoginFail");
            }
                
        }
        [HttpPost]
        public ActionResult PutUser(User user)
        {
            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser != null)
                {
                    FillBlanks(user, getUser);
                    HttpCookie pass = SController.Login(user.ID);
                    UController.PutUser(user.ID,user);

                    return View("LoggedIn");
                }
                return View("LoginFail");
            }

        }
        [HttpPost]
		public ActionResult LoginCheck(string userName, string password, bool RememberBox)
		{
			using (var context = new DBUContext())
			{				
					var getUser = (from s in context.Users where s.ID == userName select s).FirstOrDefault();
					if (getUser != null)
					{ 
						var saltHash = getUser.PassSalt;
						var encodedPassword = new UsersController().HashPassword(password, saltHash);

						var query = (from s in context.Users where s.ID == userName && s.PassHash.Equals(encodedPassword) select s).FirstOrDefault();
						if(query != null)
						{
                            
                            SController.Login(userName);
                            SController.SetRemember(RememberBox);
                            return View("LoggedIn");
						}
						ViewBag.ErrorMessage = "Invalid User Name or Password";
						return View("LoginFail");
					}
					ViewBag.ErrorMessage = "Invalid User Name or Password";
					return View("LoginFail");
			}
		}
        private void FillBlanks(User New, User Old)
        {
            New.Created = Old.Created;

        }
    }
}