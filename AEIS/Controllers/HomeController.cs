using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
    public class HomeController : Controller
    {
        public static SecurityController active = null;

        public ActionResult Index()
        {
            return View();//TODO: Logged in vs not logged in views
        }

        public ActionResult Registration()
        {
            session();
            if (active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Account()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            
            string uId = active.GetID();

            AccountVM model = new AccountVM(uId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Inventory()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            
            string uId = active.GetID();

            InventoryVM model = new InventoryVM(uId);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChartAnalysis()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            string uId = active.GetID();

            InventoryVM model = new InventoryVM(uId, 6);
            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            string uId = active.GetID();

            InventoryVM model = new InventoryVM(uId, 6);
            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(string btnPrint)
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            string uId = active.GetID();

            JustificationVM model = new JustificationVM(uId, btnPrint);
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        private void session()
        {
            if(active==null)
                active = new SecurityController();
        }
    }
}