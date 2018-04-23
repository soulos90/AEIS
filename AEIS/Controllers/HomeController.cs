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
            return View();
        }

        public ActionResult Registration()
        {

            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Account()
        {
            /*
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            */

            session();
            string uId = active.GetID();
            if (uId == "")
                uId = "mswart";

            AccountVM model = new AccountVM(uId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Inventory()
        {
            /*
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            */

            session();
            string uId = active.GetID();
            if (uId == "")
                uId = "mswart";

            InventoryVM model = new InventoryVM(uId);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChartAnalysis()
        {
            /*
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            */

            session();
            string uId = active.GetID();
            if (uId == "")
                uId = "mswart";

            InventoryVM model = new InventoryVM(uId, 6);
            return View(model);
        }

        [HttpGet]
        public ActionResult TextAnalysis()
        {
            /*
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            */

            session();
            string uId = active.GetID();
            if (uId == "")
                uId = "mswart";

            InventoryVM model = new InventoryVM(uId, 6);
            return View(model);
        }

        [HttpGet]
        public ActionResult Justification(string btnPrint)
        {
            /*
            if (!active.CheckLogin())
            {
                return RedirectToAction("Index");
            }
            */

            session();
            string uId = active.GetID();
            if (uId == "")
                uId = "mswart";

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