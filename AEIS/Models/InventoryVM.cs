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

        // gets all systems from a uId
        public InventoryVM(string uId, Security active)
        {
            InventoryVM inventory = new InventoryVM(uId);

            Systems = inventory.Systems;
            SectionTitles = inventory.SectionTitles;
            Active = active;
        }

        // maps systems from inventory to VM
        public InventoryVM(InventoryVM inventory, Security active)
        {
            SectionTitles = inventory.SectionTitles;
            Systems = inventory.Systems;
            Active = active;
        }
    }

    
        
 
        }
    

    public class InventoryItem
    {
    }

    public class IVM
    {
    }
