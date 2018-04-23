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
            // TODO: fix db query... currently gets all answers with given aId. I just want to know how many different answers there are.
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT DISTINCT * FROM Answers WHERE UId='" + uId + "';");
            
            int numOfSystems = answers.Count();
            string userId = HomeController.active.GetID();

            Environment e = new Environment();

            // put section titles from settings into array
            SectionTitles = new string[Models.Environment.NumSec];
            for (int i = 0; i < SectionTitles.Length; i++)
            {
                SectionTitles[i] = e.GetSectionName(i);
            }

            // get systems from userId
            Systems = new InventoryItem[numOfSystems];
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i] = new InventoryItem(uId, i.ToString());
            }
        }

        // gets a certain number of systems from a given uId
        public Inventory(string uId, int numOfSystems)
        {
            DBAContext dBAContext = new DBAContext();
            // TODO: fix db query... currently gets all answers with given aId. I just want to know how many different answers there are.
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT DISTINCT * FROM Answers WHERE UId='" + uId + "';");

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