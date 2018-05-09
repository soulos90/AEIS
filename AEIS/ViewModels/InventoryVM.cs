using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class InventoryVM : IVM
    {
        public InventoryItem[] Systems { get; }
        public string[] SectionTitles { get; }
        public Security Active { get; }
        public int num { get;set; }

        public InventoryVM() { }

        // gets all systems from a uId
        public InventoryVM(string uId, Security active)
        {
            Inventory inventory = new Inventory(uId);

            Systems = inventory.Systems;
            SectionTitles = inventory.SectionTitles;
            Active = active;
        }

        // maps systems from inventory to VM
        public InventoryVM(Inventory inventory, Security active)
        {
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
            Active = active;
        }
        public InventoryVM(Inventory inventory, Security active, int Num)
        {
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
            Active = active;
            num = Num;
        }
    }
}