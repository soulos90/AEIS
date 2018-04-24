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

        public ActionResult Index(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (Active.CheckLogin())
            {
                return View(active);//loggedin
            }
            return View(active);//not logged in//TODO: Logged in vs not logged in views
        }

        public ActionResult Registration(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            return View();
        }

        public ActionResult ForgotPassword(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            return View(active);
        }

        [HttpGet]
        public ActionResult Account(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            
            string uId = Active.GetID();

            AccountVM model = new AccountVM(uId,active);
            return View(model);
        }

        [HttpGet]
        public ActionResult Inventory(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            
            string uId = Active.GetID();

            InventoryVM model = new InventoryVM(uId,active);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChartAnalysis(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            string uId = Active.GetID();

            InventoryVM model = new InventoryVM(uId, 6,active);
            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis(Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            string uId = Active.GetID();

            InventoryVM model = new InventoryVM(uId, 6,active);
            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(string btnPrint,Security active)
        {
            SecurityController Active = new SecurityController(session(active));
            if (!Active.CheckLogin())
            {
                return RedirectToAction("Index",active);
            }
            string uId = Active.GetID();

            JustificationVM model = new JustificationVM(uId, btnPrint,active);
            return View(model);
        }

        public ActionResult About(Security active)
        {
            return View(active);
        }

        private Security session(Security active)
        {
            if(active==null)
                active = new Security();
            return active;
        }
    }
}