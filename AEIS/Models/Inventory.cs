using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace StateTemplateV5Beta.Models
{
    public class Inventory
    {
        public string[] SectionTitles { get; set; }
        public InventoryItem[] Systems { get; set; }

        // gets ALL systems from a given uId
        public Inventory(string uId)
        {
            DBAContext dBAContext = new DBAContext();

            var result = (from Answers in dBAContext.Answers
                          orderby Answers.AId
                          where Answers.UId == uId
                          select Answers.AId).Distinct().ToList();

                int numOfSystems = result.Count();

            getSections();
            getSystems(uId, numOfSystems);
        }

        // gets a given number of systems from a given uId
        public Inventory(string uId, int numOfSystems)
        {
            DBAContext dBAContext = new DBAContext();
            var result = (from Answers in dBAContext.Answers
                          orderby Answers.AId
                          where Answers.UId == uId
                          select Answers.AId).Distinct().ToList();

            if (numOfSystems > result.Count())
                numOfSystems = result.Count();
            else if (numOfSystems <= 0)
                numOfSystems = 1;
                
            getSections();
            getSystems(uId, numOfSystems);
            // TODO: sort systems by ScoreTotal
        }

        // gets a specific system and stores it into Systems[0].
        public Inventory(string uId, string aId)
        {
            getSections();
            Systems = new InventoryItem[1];
            Systems[0] = new InventoryItem(uId, aId);
        }

        private void getSections()
        {
            Environment e = new Environment();
            SectionTitles = new string[Environment.NumSec];

            for (int i = 0; i < SectionTitles.Length; i++)
                SectionTitles[i] = e.GetSectionName(i);
        }

        private void getSystems(string uId, int numOfSystems)
        {
            Systems = new InventoryItem[numOfSystems];

            for (int i = 0; i < Systems.Length; i++)
                Systems[i] = new InventoryItem(uId, i.ToString());
        }
    }
}