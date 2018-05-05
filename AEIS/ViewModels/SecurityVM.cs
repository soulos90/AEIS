using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class SecurityVM : IVM
    {
        public Security Active { get; }
        
        public SecurityVM(Security active)
        {
            if (active == null)
                active = new Security();

            Active = active;
        }
    }
}