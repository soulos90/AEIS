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
            return View();//TODO: logged in vs not logged in
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
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult TextAnalysis()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Account()
        {
            session();
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
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

        private void session()
        {
            if(active==null)
                active = new Controllers.SecurityController();
        }
    }
}