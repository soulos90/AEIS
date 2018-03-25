﻿using System;
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