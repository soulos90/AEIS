using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class VMP
    {
        public Security Active { get; }
        public VMP(Security active)
        {
            Active = active;
        }
    }
}