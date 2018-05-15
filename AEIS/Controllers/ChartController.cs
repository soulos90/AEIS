using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using StateTemplateV5Beta.Models;
using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult GetChart(string user,string num)
        {
            InventoryVM model = new
            InventoryVM("demo", new Models.Security());
            var key = new Chart(width: 800, height: 600);

            // display blank chart if there's no inventory entries
            if (model.Systems.Length == 0)
            {
                key.AddSeries(
                    chartType: "StackedColumn",
                    legend: "AEIS Inventory Analysis",
                    xValue: new []{""},
                    yValues: new[] {""});
            }

            for (int i = 0; i < model.SectionTitles.Length; i++)
            {
                string[] systemNames = new string[model.Systems.Length];
                int[] sectionScore = new int[model.Systems.Length];

                for (int j = 0; j < model.Systems.Length; j++)
                {
                    systemNames[j] = model.Systems[j].Name;
                    sectionScore[j] = model.Systems[j].SectionScores[i];
                }

                if (i == 0)
                {
                    key.AddSeries(
                         chartType: "StackedColumn",
                         name: model.SectionTitles[i],
                         xValue: systemNames,
                         yValues: sectionScore);

                }
                else
                {
                    key.AddSeries(
                        chartType: "StackedColumn",
                        name: model.SectionTitles[i],
                        yValues: sectionScore);
                }
            }

            key.SetXAxis("System", 0, (model.Systems.Length == 0) ? 6 : model.Systems.Length + 1);
            key.SetYAxis("Score", 0, 100);
            key.AddLegend();
            key.Write("png");
            return null;
        }


    }
}