using System;
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
            HttpCookie pass = SController.Login(user.ID);
            UController.PostUser(user);
            
            return View("LoggedIn");
        }
        // GET: Login
        public ActionResult LogIn(User user)
        {
            SController.Login(user.ID);
            return View("LoggedIn");
        }
    }
}