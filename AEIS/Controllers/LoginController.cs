﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using StateTemplateV5Beta.Models;

//namespace StateTemplateV5Beta.Controllers
//{
//    public class LoginController : Controller
//    {
//        UsersController UController = new UsersController();
//        SecurityController SController = new SecurityController();
//        [HttpPost]
//        public ActionResult PostUser(User user)
//        {
//            HttpCookie pass = SController.Login(user.ID);
//            UController.PostUser(user);

//            return View("LoggedIn");
//        }
//        // GET: Login
//        public ActionResult LogIn(User user)
//        {
//            SController.Login(user.ID);
//            return View("LoggedIn");
//        }

//        [HttpPost]
//        public ActionResult LoginCheck(string userName, string password)
//        {
//            using (var context = new DBUContext())
//            {
//                var getUser = (from s in context.Users where s.ID == userName select s).FirstOrDefault();
//                if (getUser != null)
//                {
//                    var saltHash = getUser.PassSalt;
//                    var encodedPassword = new UsersController().HashPassword(password, saltHash);

//                    var query = (from s in context.Users where s.ID == userName && s.PassHash.Equals(encodedPassword) select s).FirstOrDefault();
//                    if (query != null)
//                    {
//                        return View("LoggedIn");
//                    }
//                    ViewBag.ErrorMessage = "Invalid User Name or Password";
//                    return View("LoginFail");
//                }
//                ViewBag.ErrorMessage = "Invalid User Name or Password";
//                return View("LoginFail");
//            }
//        }
//    }
//}