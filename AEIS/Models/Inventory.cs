using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Controllers;

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
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT count(DISTINCT a.AId) FROM Answers a WHERE UId='" + uId + "';");          
            int numOfSystems = answers.Count();

            getSections();
            getSystems(uId, numOfSystems);
        }

        // gets a given number of systems from a given uId
        public Inventory(string uId, int numOfSystems)
        {
            DBAContext dBAContext = new DBAContext();
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT count(Distinct a.AID) FROM Answers a WHERE UId='" + uId + "';");

            if (numOfSystems > answers.Count())
                numOfSystems = answers.Count();

            getSections();
            getSystems(uId, numOfSystems);
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
            {
                SectionTitles[i] = e.GetSectionName(i);
            }
        }

        private void getSystems(string uId, int numOfSystems)
        {
            Systems = new InventoryItem[numOfSystems];

            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i] = new InventoryItem(uId, i.ToString());
            }
        }
    }
}