using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace StateTemplateV5Beta.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult GetChart()
        {
            var key = new Chart(width: 800, height: 600)
                .AddTitle("AEIS Inventory Analysis")
                .AddSeries(
                    chartType: "column",
                    legend: "AEIS Inventory Analysis",
                    xValue: new[] { "MyMedical 2.0", "Project CALculate", "MyCalTravel", "iTracker Online", "Contruction Manager", "DataShare Pub" },
                    yValues: new[] { "94", "81", "78", "74", "72", "66" })
                .Write();

            return null;
        }
    }
}