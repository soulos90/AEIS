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
                    chartType: "StackedColumn",
                    legend: "AEIS Inventory Analysis",
                    xValue: new[] { "MyMedical 2.0", "Project CALculate", "MyCalTravel", "iTracker Online", "Contruction Manager", "DataShare Pub" },
                    yValues: new[] { "33", "33", "17", "33", "33", "27" })
               
                    .AddSeries(
                    chartType: "StackedColumn",
                    legend: "AEIS Inventory Analysis",
                    yValues: new[] { "11", "11", "11", "11", "11", "11" })

                    .AddSeries(
                    chartType: "StackedColumn",
                    legend: "AEIS Inventory Analysis",
                    yValues: new[] { "22", "9", "22", "2", "22", "22" })

                    .AddSeries(
                    chartType: "StackedColumn",
                    legend: "AEIS Inventory Analysis",
                    yValues: new[] { "28", "28", "28", "28", "6", "6" })
                .Write();
            return null;
        }

        private Object TextBoxes(
    Object Index
)
    }
}