using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (IsLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new LoginVM(active.IsLoggedIn, active);

            return View(model);//not logged in
        }

        public ActionResult Registration(Security active)
        {
            if (IsLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new SecurityVM(active);

            return View(model);
        }

        public ActionResult ForgotPassword(Security active)
        {
            if (IsLoggedIn())
                return RedirectToAction("Inventory");

            IVM model = new SecurityVM(active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Account(Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            IVM model = new AccountVM(uId, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Inventory(Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            Inventory inventory = new Inventory(uId);
            inventory.SortByLastUsed();
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        [HttpPost]
        public ActionResult Inventory(string sort, Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            InventoryVM model = new InventoryVM();
            Inventory inventory = new Inventory(uId);
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

            model = new InventoryVM(inventory, active);

            return View(model);
        }

        // Not implemented yet
        [HttpGet]
        public ActionResult DeleteSurvey(int aId)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public ActionResult ChartAnalysis(Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(6);
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        [HttpPost]
        public ActionResult ChartAnalysis(int numOfSystems, Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(int.Parse(Request.Form["numOfSystems"]));
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis(Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(6);
            InventoryVM model = new InventoryVM(inventory, active);

            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(Security active)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index");

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string uId = cookie.Values["ID"];

            IVM model = new JustificationVM(uId, "0", active);

            return View(model);
        }

        [HttpPost]
        public ActionResult Justification(string btnPrint, Security active)
        {
            if (!IsLoggedIn())
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
            active = new Security();
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser == null)
                {
                    UController.PostUser(user);
                    Login(user.ID);
                    model = new LoginVM(active.IsLoggedIn, active);
                }

                return RedirectToAction("Index"); // change to redirect
            }
        }

        [HttpPost]
        public ActionResult PutUser(User user, Security active)
        {
            active = new Security();
            IVM model = new LoginVM(active.IsLoggedIn, active);

            using (var context = new DBUContext())
            {
                var getUser = (from s in context.Users where s.ID == user.ID select s).FirstOrDefault();
                if (getUser != null)
                {
                    user.Created = getUser.Created;
                    Login(user.ID);
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
                        Login(userName);
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

        private void Login(string uID)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            UsersController usersController = new UsersController();
            User user = usersController.GetU(uID);

            if (cookie == null || cookie.Values["LoggedIn"] != "True")
                cookie = new HttpCookie("UserInfo");

            cookie.Values["LoggedIn"] = "True";
            cookie.Values["ID"] = uID;
            cookie.Values["Hash"] = user.PassHash;
            cookie.Expires = DateTime.Now.AddHours(8);
            Response.Cookies.Add(cookie);
        }

        public bool IsLoggedIn()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            UsersController usersController = new UsersController();
            User user = null;
            string decodedHash = null;

            if (cookie != null)
            {
                string decodedUser = HttpUtility.HtmlDecode(cookie.Values["ID"]);
                user = usersController.GetU(decodedUser);
                decodedHash = HttpUtility.HtmlDecode(cookie.Values["Hash"]);
            }

            if (user == null || cookie == null || cookie.Values["LoggedIn"] != "True" ||
                cookie.Values["ID"] == null || decodedHash != user.PassHash.Trim())
                return false;

            return true;
        }
    }
}