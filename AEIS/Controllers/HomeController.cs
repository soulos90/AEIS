using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StateTemplateV5Beta.Controllers
{
    public class HomeController : Controller
    {
        public static Controllers.SecurityController active = null;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Justification(string name)
        {
            // if user is not logged in, redirect to index
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Inventory()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult InventoryGrid()
        {
            return View();
        }

        public ActionResult ChartAnalysis()
        {
            return View();
        }

        public ActionResult TextAnalysis()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Registration()
        {

            return View();
        }

        private void session()
        {
            if(active==null)
                active = new Controllers.SecurityController();
        }
    }
}