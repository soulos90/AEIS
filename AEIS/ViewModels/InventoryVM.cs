using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class InventoryVM
    {
        #region Properties
        public string[] SectionTitles { get; }
        public InventoryItem[] Systems { get; }
        #endregion

        #region Constructors
        // gets all systems from a uId
        public InventoryVM(string uId)
        {
            Inventory inventory = new Inventory(uId);
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
        }

        // gets 'numOfSystems' number of systems from a uId
        public InventoryVM(string uId, int numOfSystems)
        {
            Inventory inventory = new Inventory(uId, numOfSystems);
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
        }
        #endregion
    }
}