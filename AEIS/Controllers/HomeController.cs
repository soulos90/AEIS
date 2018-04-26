using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index(Security active)
        {
            IVM model = new LoginVM(active.IsLoggedIn, active);
            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model); //logged in    // change to redirect
            }
            //TODO: Logged in vs not logged in views probably involved making a IndexVM with a bool
            return View(model);//not logged in
        }

        public ActionResult Registration(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model);    // change to redirect
            }

            return View(model);
        }

        public ActionResult ForgotPassword(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (Active.CheckLogin())
            {
                model = new InventoryVM(Active.GetID(), Active.GetActive());
                return View("Inventory", model);    // change to redirect
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Account(Security active)
        {
            IVM model;
            active = session(active);
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

        [HttpGet]
        public ActionResult Inventory(Security active)
        {
            IVM model;
            active = session(active);
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

        [HttpGet]
        public ActionResult ChartAnalysis(Security active)
        {
            IVM model;
            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            model = new InventoryVM(uId, 6, active);

            return View(model);           
        }

        [HttpPost]
        public ActionResult ChartAnalysis(int numOfSystems, Security active)
        {
            IVM model;
            
            string uId = "Demo";
            model = new InventoryVM(uId, numOfSystems, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis(Security active)
        {
            IVM model;
            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            model = new InventoryVM(uId, 6, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(Security active)
        {
            IVM model;
            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new LoginVM(active.IsLoggedIn, active);
                return View("Index", model);    // change to redirect
            }

            string uId = Active.GetID();
            model = new JustificationVM(uId, "0", active);

            return View(model);
        }

        [HttpPost]
        public ActionResult Justification(string btnPrint, Security active)
        {
            IVM model;
            active = session(active);
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

        public ActionResult About(Security active)
        {
            IVM model = new SecurityVM(session(active));
            return View(model);
        }

        private Security session(Security active)
        {
            if(active==null)
                active = new Security();
            return active;
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
                    HttpCookie pass = SController.Login(user.ID);
                    UController.PostUser(user);
                    model = new LoginVM(active.IsLoggedIn, active);
                }

                return View("Index",model); // change to redirect
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
                    HttpCookie pass = SController.Login(user.ID);
                    model = new LoginVM(active.IsLoggedIn, SController.GetActive());
                    UController.PutUser(user.ID, user);
                }

                return View("Index",model); // change to redirect
            }
        }

        [HttpPost]
        public ActionResult LoginAuthentication(string userName, string password, bool RememberBox, Security active)
        {
            active = session(active);
            var UC = new UsersController();
            SecurityController SController = new SecurityController(active);
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var user = UC.GetU(userName);
                if (user != null)
                {
                    var saltHash = user.PassSalt;
                    var encodedPassword = UC.HashPassword(password, saltHash);
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
        }
    }
}