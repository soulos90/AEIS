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
            Inventory model = new Inventory(user);
            model.SortByTotalScore();
            model = model.GetTop(int.Parse(num));

            var key = new Chart(width: 1280, height: 720);
            key.AddTitle("AEIS Inventory Analysis");

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

        private Object TextBoxes()
    Object Index

    }
}