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
    public class HomeController : Controller
    {
        UsersController UController = new UsersController();
        public ActionResult Index(string actives, string activeLog, string activeRem)
        {

            Security active = session(actives, activeLog, activeRem);
            IVM model = new LoginVM(active.IsLoggedIn, active);
            SecurityController Active = new SecurityController(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model); //logged in    // change to redirect
            }
            //TODO: Logged in vs not logged in views probably involved making a IndexVM with a bool
            return View(model);//not logged in
        }

        public ActionResult Registration(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model);    // change to redirect
            }

            return View(model);
        }

        public ActionResult ForgotPassword(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model);    // change to redirect
            }

            return View(model);
        }

        public ActionResult Account(string actives, string activeLog, string activeRem)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            model = new AccountVM(uId, active);

            return View(model);
        }

        public ActionResult Inventory(string actives, string activeLog, string activeRem)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            model = new InventoryVM(uId, active);

            return View(model);
        }

        [HttpPost]
        public ActionResult Inventory(string sort, string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                LoginVM lmodel = new LoginVM(active.IsLoggedIn, active);
                return View("Index", lmodel);    // change to redirect
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

        // Not yet implemented
        public ActionResult DeleteSurvey(string actives, string activeLog, string activeRem, int aId)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();

            model = new InventoryVM(uId, active);
            return RedirectToAction("Inventory", model);
        }

        public ActionResult ChartAnalysis(string actives, string activeLog, string activeRem, int numOfSystems = 6)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(numOfSystems);
            model = new InventoryVM(inventory, active);

            return View(model);
        }

        public ActionResult TextAnalysis(string actives, string activeLog, string activeRem)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(6);
            model = new InventoryVM(inventory, active);

            return View(model);
        }

        public ActionResult Justification(string btnPrint, string actives, string activeLog, string activeRem)
        {
            IVM model;
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            string aId = Request.Form["btnJustification"];
            model = new JustificationVM(uId, aId, active);

            return View(model);
        }

        public ActionResult About(string btnPrint, string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            Active.CheckLogin();
            IVM model = new SecurityVM(active);
            return View(model);
        }

        private Security session(Security active)
        {
            if (active == null)
                active = new Security();
            return active;
        }

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
        public ActionResult PostUser(User user, Security active)
        {
            active = session(active);
            SecurityController SController = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser == null)
                {
                    SController.Login(user.ID);
                    UController.PostUser(user);
                    model = new LoginVM(active.IsLoggedIn, active);
                }

                return View("Index", model); // change to redirect
            }
        }

        [HttpPost]
        public ActionResult PutUser(User user, Security active)
        {
            active = session(active);
            SecurityController SController = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser != null)
                {
                    user.Created = getUser.Created;
                    SController.Login(user.ID);
                    model = new LoginVM(active.IsLoggedIn, SController.GetActive());
                    UController.PutUser(user.ID, user);
                }

                return View("Index", model); // change to redirect
            }
        }

        [HttpPost]
        public ActionResult LoginAuthentication(string userName, string password, bool RememberBox)
        {
            Security active = new Security();
            var UController = new UsersController();
            SecurityController SController = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var user = UController.GetU(userName);
                if (user != null)
                {
                    var saltHash = user.PassSalt;
                    var encodedPassword = UController.HashPassword(password, saltHash);
                    if (user.PassHash.Trim() == encodedPassword.Trim())
                    {
                        SController.Login(userName);
                        SController.SetRemember(RememberBox);
                        model = new InventoryVM(userName.Trim(), SController.GetActive());
                        return View("Inventory", model);    // change to redirect
                    }
                }

                ViewBag.ErrorMessage = "Invalid User Name or Password";
                return View("Index", model);    // change to redirect           
            }
        }//TODO: make a 404 page
    }
}