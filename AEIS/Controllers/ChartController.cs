﻿using System;
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
            InventoryVM model = new
            InventoryVM("demo", 6, new Models.Security());
            var key = new Chart(width: 800, height: 600)


                key.AddTitle("AEIS Inventory Analysis");
                for (int i=0; i<model.SectionTitles.Length; i++)
                    {
                string[] systemNames = new string[model.Systems.Length];
                int[] sectionScore = new int[model.Systems.Length];

                for (int j = 0; j < model.System.Length; j++)
                {
                    systemNames[j] = model.Systems[j].Name;
                    sectionScore[j] = medel.Systems[j].SectionScores[i];
                }
                if (i == 0)
                {
                    key.AddSeries(
                        chartType: "StackedColumn",
                        legend: "AEIS Inventory Analysis",
                        xValue: systemNames,
                        yValues: sectionScore);
                }else
                {
                    key.AddSeries(
                        chartType: "stackedColumn",
                        legend: "AEIS Inventory Analysis",
                        yValues: sectionScore);
                }
                }
                    
            return null;
        }

        private Object TextBoxes()
    Object Index

    }
}