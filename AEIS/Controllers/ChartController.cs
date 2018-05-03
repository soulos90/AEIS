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
            Inventory model = new Inventory("user");
            model.GetTop(int.Parse(num));

            var key = new Chart(width: 800, height: 600);
            key.AddTitle("AEIS Inventory Analysis");

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
                         legend: "AEIS Inventory Analysis",
                         xValue: systemNames,
                         yValues: sectionScore);
                }
                else
                {
                    key.AddSeries(
                        chartType: "StackedColumn",
                        legend: "AEIS Inventory Analysis",
                        yValues: sectionScore);
                }
            }

            key.Write();
            return null;
        }
    }
}