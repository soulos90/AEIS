﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using StateTemplateV5Beta.Models;

using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
    // TODO: HOME CONTROLLER - add return redirects where needed
    // TODO: HOME CONTROLLER - update user 'lastUsed' field when they log in
    // TODO: HOME CONTROLLER - make a 404 page

    public class HomeController : Controller
    {
        UsersController UController = new UsersController();
        public ActionResult Index(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            IVM model = new LoginVM(active.IsLoggedIn, active);
            SecurityController Active = new SecurityController(active);

            if ((IsLoggedIn(Active).CheckLogin()))
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model); 
            }

            return View(model);
        }

        public ActionResult Registration(string actives, string activeLog, string activeRem)
        {

            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (IsLoggedIn(Active).CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model); 
            }

            return View(model);
        }

        // TODO: HOME CONTROLLER - implement ForgotPassword
        public ActionResult ForgotPassword(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (IsLoggedIn(Active).CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model); 
            }

            return View(model);
        }

        public ActionResult Account(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives.Trim(), activeLog.Trim(), activeRem.Trim());
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID().Trim();
            AccountVM model = new AccountVM(uId, active);

            return View(model);
        }

        public ActionResult Inventory(string sort, string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            Inventory inventory = new Inventory(Active.GetID());
            inventory.SortByLastUsed();
            int section;

            if (sort == "name")
                inventory.SortByName();
            else if (sort == "lastUsed")
                inventory.SortByLastUsed();
            else if (sort == "totalScore")
                inventory.SortByTotalScore();
            else if (int.TryParse(sort, out section))
                inventory.SortBySectionScore(section);

            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        // TODO: HOME CONTROLLER - implement DeleteSurvey
        public ActionResult DeleteSurvey(string actives, string activeLog, string activeRem, int aId)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();

            // add delete logic

            InventoryVM model = new InventoryVM(uId, active);
            return RedirectToAction("Inventory", model);

        }

        [HttpGet]
        public ActionResult ChartAnalysis(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(inventory.DefaultNum);
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        public ActionResult ChartAnalysis(string actives, string activeLog, string activeRem, int numOfSystems)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(numOfSystems);
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        public ActionResult TextAnalysis(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();
            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(6);
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        public ActionResult Justification(string actives, string activeLog, string activeRem, string aId)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();
            JustificationVM model = new JustificationVM(uId, aId, active);

            return View(model);
        }

        public ActionResult About(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            Active.CheckLogin();
            IVM model = new SecurityVM(active);
            return View(model);
        }

        //public ActionResult Logout()
        //{
        //    if (active == null)
        //        active = new Security();
        //    return active;
        //}

        private Security session(string active, string activeLog, string rem)
        {
            Security Active;
            if (active == null)
            {
                active = "";
            }

            if (activeLog == null)
            {
                activeLog = "False";
            }

            if (rem == null)
            {
                rem = "False";
            }
            Active = new Security(active, activeLog.Equals("True"), rem.Equals("True"));

            return Active;
        }

        [HttpPost]
        public ActionResult PostUser(User user)
        {
            Security active = session(user.ID, "False", "False");
            UsersController u = new UsersController();
            SecurityController SC = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            var getUser = u.GetU(user.ID);
            if (getUser == null)
            {
                SC.Login(user.ID);
                Login(SC);
                UController.PostUser(user);
                model = new LoginVM(SC.CheckLogin(), SC.GetActive());
                return View("Index", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Email already registered";
            }
            model = new SecurityVM(active);
            return View("Registration", model);
        }

        [HttpPost]
        public ActionResult PutUser(User user, string actives, string activeLog, string activeRem, string currentPassword)
        {
            Security active = session(actives, activeLog, activeRem);
            UsersController u = new UsersController();
            SecurityController SController = new SecurityController(active);
            IVM model;

            var getUser = u.GetU(SController.GetID().Trim());
            if (getUser.ID != user.ID && u.GetU(user.ID.Trim()) == null)
            {
                user.PassSalt = getUser.PassSalt;
                if (getUser.PassHash.Trim() == u.HashPassword(currentPassword, user.PassSalt).Trim())
                {
                    user.Created = getUser.Created;
                    SController.Login(user.ID);
                    Login(SController);

                    user.PassHash = u.HashPassword(user.PassHash, user.PassSalt);
                    model = new LoginVM(active.IsLoggedIn, SController.GetActive());
                    UController.PutUser(user.ID, user);
                    return View("Index", model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Current Password";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid New Email";
            }

            model = new AccountVM(SController.GetID(), SController.GetActive());

            return View("Account", model);
        }

        [HttpPost]
        public ActionResult LoginAuthentication(string userName, string password, bool RememberBox)
        {
            Security active = new Security();
            var UController = new UsersController();
            SecurityController SController = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            var user = UController.GetU(userName);
            if (user != null)
            {
                var saltHash = user.PassSalt;
                var encodedPassword = UController.HashPassword(password, saltHash);
                if (user.PassHash.Trim() == encodedPassword.Trim())
                {
                    SController.Login(userName);
                    SController.SetRemember(RememberBox);
                    Login(SController);
                    model = new InventoryVM(userName.Trim(), SController.GetActive());
                    return View("Inventory", model);    // change to redirect
                }
                else
                    ViewBag.ErrorMessage = "Invalid Password";
            }
            else
                ViewBag.ErrorMessage = "Invalid User Name";

            return View("Index", model);    // change to redirect           
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("UserInfo");
            cookie.Values["LoggedIn"] = "False";
            cookie.Values["ID"] = null;
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
            Session.Clear();

            return RedirectToAction("Index");
        }

        private void Login(SecurityController active)
        {
            //Session["loggedin"] = true;
            HttpCookie cookie = Request.Cookies["UserInfo"];

            if (cookie == null || cookie.Values["LoggedIn"] != "True")
                cookie = new HttpCookie("UserInfo");

            cookie.Values["LoggedIn"] = "True";
            cookie.Values["ID"] = active.GetID();
            cookie.Values["Remember"] = active.GetRemember().ToString();
            cookie.Expires = active.GetEX();
            Response.Cookies.Add(cookie);
        }

        public SecurityController IsLoggedIn(SecurityController active)
        {

            bool value = false;
            string decodedUser = "";
            bool remember = false;
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                decodedUser = HttpUtility.HtmlDecode(cookie.Values["ID"]);
                value = HttpUtility.HtmlDecode(cookie.Values["LoggedIn"]).Equals("True");
                remember = HttpUtility.HtmlDecode(cookie.Values["Remember"]).Equals("True");
            }

            if (value)
            {
                active.Login(decodedUser);
                active.SetRemember(remember);
            }

            return active;
        }
    }
}