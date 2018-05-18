using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using StateTemplateV5Beta.Models;

using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
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

        public ActionResult ForgotPassword(string userName = null)
        {
            Security active;
            if (userName == null)
                active = session("", "False", "False");
            active = session(userName, "False", "False");
            SecurityController Active = new SecurityController(active);
            IVM model = new SecurityVM(active);

            if (userName == null)
                return View("ForgotPassword",model);

            string randomPass = genPass().Trim();
            UsersController u = new UsersController();
            User user = u.GetU(userName);
            user.PassHash = u.HashPassword(randomPass, user.PassSalt);
            u.PutUser(user);
            sendEmail(randomPass, userName,user.FName);
            model = new LoginVM(false,active);
            return View("Index",model);
        }
        private void sendEmail(string randomPass,string userName,string FName)
        {
            SmtpClient email = new SmtpClient();
            string message = "Hello " + FName +","+
               "\nIn order to protect your information we have changed your password to: " + randomPass +
               "\nPlease use that password at the Department of Rehabilitation's AEIS website to login."+
               "\n\nThis address can not receive responses, please do not reply.";
            email.Port = 25;
            email.Host = "smtp.saclink.csus.edu";
            email.Send("NorthDelta@csus.edu", userName, "AEIS new password", message);
        }

        private string genPass()
        {
            string Value = "";
            Random n = new Random();
            for(int i = 0;i<8;++i)
            {
                Value += (char)((n.Next()%26)+65);
            }
            return Value.Trim();
        }

        public ActionResult Account(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();
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
        
        public ActionResult DeleteSurvey(string actives, string activeLog, string activeRem, int aId)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            AnswersController a = new AnswersController();
            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            string uId = Active.GetID();
            a.DeleteWholeAnswer(uId,aId);

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

        public ActionResult ChartAnalysis(string actives, string activeLog, string activeRem, string numOfSystems)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index");
            }

            // check that numOfSystems is a valid number
            int num;
            if (numOfSystems == null || !int.TryParse(numOfSystems, out num))
                num = 6;

            string uId = Active.GetID();

            Inventory inventory = new Inventory(uId);
            inventory.SortByTotalScore();
            inventory = inventory.GetTop(num);
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
                if(user.FName==null)
                {
                    user.FName = "";
                }
                if (user.LName == null)
                {
                    user.LName = "";
                }
                if (user.Organization == null)
                {
                    user.Organization = "";
                }
                SC.Login(user.ID);
                Login(SC);
                UController.PostUser(user);
                model = new InventoryVM(SC.GetID(), active);
                return View("Inventory", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Email already registered";
            }
            model = new SecurityVM(active);
            return View("Registration", model);
        }

        [HttpPost]
        public ActionResult PutUser(string FirstName, string LastName, string Organization, string PassHash, string actives, string activeLog, string activeRem, string CurrentPassword, string NewPassword)
        {
            Security active = session(actives, activeLog, activeRem);
            UsersController u = new UsersController();
            SecurityController SController = new SecurityController(active);
            IVM model;

            var getUser = u.GetU(SController.GetID().Trim());

            if (getUser.PassHash.Trim() == u.HashPassword(CurrentPassword, getUser.PassSalt).Trim())
            {
                if (FirstName == null)
                {
                    FirstName = "";
                }
                if (LastName == null)
                {
                    LastName = "";
                }
                if (Organization == null)
                {
                    Organization = "";
                }
                getUser.FName = FirstName;
                getUser.LName = LastName;
                getUser.Organization = Organization;

                getUser.PassHash = u.HashPassword(NewPassword, getUser.PassSalt);
                UController.PutUser(getUser.ID, getUser);

                ViewBag.ErrorMessage = "Account Info Updated";

                //return View("Account", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid Password";
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
                    return View("Inventory", model);
                }
                else
                    ViewBag.ErrorMessage = "Invalid Password";
            }
            else
                ViewBag.ErrorMessage = "Invalid User Name";

            return View("Index", model);      
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