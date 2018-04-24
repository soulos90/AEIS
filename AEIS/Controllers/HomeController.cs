using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StateTemplateV5Beta.Models;

using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
    public class HomeController : Controller
    {
        UsersController UController = new UsersController();
        public ActionResult Index(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            VMP model = new VMP(active);
            if (Active.CheckLogin())
            {
                return View(model);//loggedin
            }
            return View(model);//not logged in//TODO: Logged in vs not logged in views probably involved making a IndexVM with a bool
        }

        public ActionResult Registration(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            VMP model = new VMP(active);
            if (Active.CheckLogin())
            {
                return View("Index",model);
            }
            return View(model);
        }

        public ActionResult ForgotPassword(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            VMP model = new VMP(active);
            if (!Active.CheckLogin())
            {
                return View("Index",model);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Account(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            if (!Active.CheckLogin())
            {
                VMP model = new VMP(active);
                return View("Index", active);
            }
            else
            {
                string uId = Active.GetID();

                AccountVM model = new AccountVM(uId, active);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Inventory(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            if (!Active.CheckLogin())
            {
                VMP model = new VMP(active);
                return View("Index", active);
            }
            else
            {
                string uId = Active.GetID();

                InventoryVM model = new InventoryVM(uId, active);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ChartAnalysis(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            if (!Active.CheckLogin())
            {
                VMP model = new VMP(active);
                return View("Index", active);
            }
            else
            {

                string uId = Active.GetID();

                InventoryVM model = new InventoryVM(uId, 6, active);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult TextAnalysis(Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            if (!Active.CheckLogin())
            {
                VMP model = new VMP(active);
                return View("Index", active);
            }
            else
            {
                string uId = Active.GetID();

                InventoryVM model = new InventoryVM(uId, 6, active);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Justification(string btnPrint,Security active)
        {
            active = session(active);
            SecurityController Active = new SecurityController(active);
            if (!Active.CheckLogin())
            {
                VMP model = new VMP(active);
                return View("Index",model);
            }
            else
            {
                string uId = Active.GetID();

                JustificationVM model = new JustificationVM(uId, btnPrint, active);
                return View(model);
            }
        }

        public ActionResult About(Security active)
        {
            VMP model = new VMP(session(active));
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
            VMP model = new VMP(active);
            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser == null)
                {
                    HttpCookie pass = SController.Login(user.ID);
                    UController.PostUser(user);
                    model = new VMP(SController.GetActive());
                    return View("Index",model);
                }
                return View("Index",model);
            }

        }
        [HttpPost]
        public ActionResult PutUser(User user, Security active)
        {
            active = session(active);
            SecurityController SController = new SecurityController(active);
            VMP model = new VMP(active);
            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser != null)
                {
                    FillBlanks(user, getUser);
                    HttpCookie pass = SController.Login(user.ID);
                    model = new VMP(SController.GetActive());
                    UController.PutUser(user.ID, user);

                    return View("Index",model);
                }
                return View("Index",model);
            }

        }
        [HttpPost]
        public ActionResult LoginCheck(string userName, string password, bool RememberBox, Security active)
        {
            active = session(active);
            SecurityController SController = new SecurityController(active);
            VMP model = new VMP(active);
            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == userName select s).FirstOrDefault();
                if (getUser != null)
                {
                    var saltHash = getUser.PassSalt;
                    var encodedPassword = new UsersController().HashPassword(password, saltHash);

                    var query = (from s in context.Users where s.ID == userName && s.PassHash.Equals(encodedPassword) select s).FirstOrDefault();
                    if (query != null)
                    {

                        SController.Login(userName);
                        SController.SetRemember(RememberBox);
                        model = new VMP(SController.GetActive());
                        return View("Index", model);//TODO: deal with security token
                    }
                    ViewBag.ErrorMessage = "Invalid Password";
                    return View("Index", model);
                }
                ViewBag.ErrorMessage = "Invalid User Name";
                return View("Index", model);
            }
        }
        private void FillBlanks(User New, User Old)
        {
            New.Created = Old.Created;

        }
    }
}