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
            if (isLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new LoginVM(active.IsLoggedIn, active);

            return View(model);//not logged in
        }

        public ActionResult Registration(Security active)
        {
            if (isLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new SecurityVM(active);

            return View(model);
        }

        public ActionResult ForgotPassword(Security active)
        {
            if (isLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new SecurityVM(active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Account(Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];

            string uId = cookie.Values["ID"];
            IVM model = new AccountVM(uId, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Inventory(Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];

            string uId = cookie.Values["ID"];
            IVM model = new InventoryVM(uId, active);

            return View(model);
        }

        // Not implemented yet
        [HttpGet]
        public ActionResult DeleteSurvey(int aId)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            AnswersController aController = new AnswersController();

            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public ActionResult ChartAnalysis(Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            IVM model = new InventoryVM(uId, 6, active);

            return View(model);
        }

        [HttpPost]
        public ActionResult ChartAnalysis(int numOfSystems, Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            IVM model = new InventoryVM(uId, numOfSystems, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis(Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            IVM model = new InventoryVM(uId, 6, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            IVM model = new JustificationVM(uId, "0", active);

            return View(model);
        }

        [HttpPost]
        public ActionResult Justification(string btnPrint, Security active)
        {
            if (!isLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];
            string aId = Request.Form["btnJustification"];
            IVM model = new JustificationVM(uId, aId, active);

            return View(model);
        }

        public ActionResult About(Security active)
        {
            IVM model = new LoginVM(active.IsLoggedIn, active);
            return View();
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("UserInfo");
            cookie.Values["LoggedIn"] = "False";
            cookie.Values["ID"] = null;
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostUser(User user, Security active)
        {
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser == null)
                {
                    login(user.ID);
                    UController.PostUser(user);
                    model = new LoginVM(active.IsLoggedIn, active);
                }

                return RedirectToAction("Index"); // change to redirect
            }
        }

        [HttpPost]
        public ActionResult PutUser(User user, Security active)
        {
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser != null)
                {
                    user.Created = getUser.Created;
                    login(user.ID);
                    model = new LoginVM(active.IsLoggedIn, active);
                    UController.PutUser(user.ID, user);
                }

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult LoginAuthentication(string userName, string password, bool RememberBox, Security active)
        {
            IVM model;

            using (var context = new DBUContext())
            {
                var user = UController.GetU(userName);
                if (user != null)
                {
                    var saltHash = user.PassSalt;
                    var encodedPassword = UController.HashPassword(password, saltHash);
                    if (user.PassHash.Trim() == encodedPassword.Trim())
                    {
                        login(userName);
                        active.Remember = RememberBox;
                        model = new InventoryVM(userName.Trim(), active);
                        return RedirectToAction("Inventory");
                    }
                }

                model = new LoginVM(false, active);
                ViewBag.ErrorMessage = "Invalid User Name or Password";
                return RedirectToAction("Index");
            }
        }

        private void login(string ID)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];

            if (cookie == null || cookie.Values["LoggedIn"] != "True")
                cookie = new HttpCookie("UserInfo");

            cookie.Values["LoggedIn"] = "True";
            cookie.Values["ID"] = ID;
            cookie.Expires = DateTime.Now.AddHours(8);
            Response.Cookies.Add(cookie);
        }

        public bool isLoggedIn()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            bool isLoggedIn = true;

            if (cookie == null || cookie.Values["LoggedIn"] != "True" ||
                cookie.Values["ID"] == null)
                isLoggedIn = false;

            return isLoggedIn;
        }
    }
}