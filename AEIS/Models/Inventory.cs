using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            
            int numOfAnswers = dBAContext.Answers.SqlQuery("SELECT DISTINCT * FROM Answers WHERE UId='" + uId + "';").Count();
            int numOfSystems = numOfAnswers / Environment.NumQus;

            getSections();
            getSystems(uId, numOfSystems);
        }

        // gets a given number of systems from a given uId
        public Inventory(string uId, int numOfSystems)
        {
            DBAContext dBAContext = new DBAContext();
            int numOfAnswers = dBAContext.Answers.SqlQuery("SELECT DISTINCT * FROM Answers WHERE UId='" + uId + "';").Count();
            numOfAnswers /= Environment.NumQus;

            if (numOfSystems > numOfAnswers)
                numOfSystems = numOfAnswers;

            getSections();
            getSystems(uId, numOfSystems);
            // TODO: sort sections by ScoreTotal
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