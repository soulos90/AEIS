using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class InventoryVM : IVM
    {
        public string[] SectionTitles { get; }
        public InventoryItem[] Systems { get; }
        public Security Active { get; }

        // gets all systems from a uId
        public InventoryVM(string uId, Security active)
        {
            Inventory inventory = new Inventory(uId);
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
            Active = active;
        }

        // gets 'numOfSystems' number of systems from a uId
        public InventoryVM(string uId, int numOfSystems, Security active)
        {
            Inventory inventory = new Inventory(uId, numOfSystems);
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
            Active = active;
        }
    }
}