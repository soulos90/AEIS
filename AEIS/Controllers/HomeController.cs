using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StateTemplateV5Beta.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Justification()
        {
            return View();
        }

        public ActionResult Inventory()
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
        public ActionResult Questions()
        {
            return View();
        }

        #region Template Remnants
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Structure()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult serp()
        {
            return View();
        }
        #endregion
    }
}