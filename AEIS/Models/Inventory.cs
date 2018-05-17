using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace StateTemplateV5Beta.Models
{
    public class Inventory
    {
        private const int defaultNum = 6;
        public InventoryItem[] Systems { get; set; }
        public string[] SectionTitles { get; set; }
        public int DefaultNum { get { return defaultNum; } }

        private Inventory() {}

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

        // gets a specific system and stores it into Systems[0].
        public Inventory(string uId, string aId)
        {
            getSections();
            Systems = new InventoryItem[1];
            Systems[0] = new InventoryItem(uId, aId);
        }

        // returns a new inventory with only 0-num indexes (for chart / text analysis)
        public Inventory GetTop(int num)
        {
            if (num > Systems.Length)
                num = Systems.Length;
            else if (num < 0)
                num = 0;

            Inventory inventory = new Inventory();
            inventory.Systems = new InventoryItem[num];
            inventory.SectionTitles = SectionTitles;

            for (int i = 0; i < num; i++)
                inventory.Systems[i] = Systems[i];

            return inventory;
        }

        public void SortByName()
        {
            for (int i = 0; i < Systems.Length - 1; i++)
                for (int j = 0; j < Systems.Length - 1 - i; j++)
                {
                    if (Systems[j].Name.CompareTo(Systems[j + 1].Name) == 1)
                    {
                        InventoryItem temp = Systems[j];
                        Systems[j] = Systems[j + 1];
                        Systems[j + 1] = temp;
                    }
                }
        }

        public void SortBySectionScore(int sectionNum)
        {
            for (int i = 0; i < Systems.Length - 1; i++)
                for (int j = 0; j < Systems.Length - 1 - i; j++)
                {
                    if (Systems[j].SectionScores[sectionNum] < Systems[j + 1].SectionScores[sectionNum])
                    {
                        InventoryItem temp = Systems[j];
                        Systems[j] = Systems[j + 1];
                        Systems[j + 1] = temp;
                    }
                }
        }

        public void SortByTotalScore()
        {
            for (int i = 0; i < Systems.Length - 1; i++)           
                for (int j = 0; j < Systems.Length - 1 - i; j++)
                {
                    if (Systems[j].ScoreTotal < Systems[j + 1].ScoreTotal)
                    {
                        InventoryItem temp = Systems[j];
                        Systems[j] = Systems[j + 1];
                        Systems[j + 1] = temp;
                    }
                }            
        }

        public void SortByLastUsed()
        {
            for (int i = 0; i < Systems.Length - 1; i++)
                for (int j = 0; j < Systems.Length - 1 - i; j++)
                {
                    if (Systems[j].LastUsed < Systems[j + 1].LastUsed)
                    {
                        InventoryItem temp = Systems[j];
                        Systems[j] = Systems[j + 1];
                        Systems[j + 1] = temp;
                    }
                }
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