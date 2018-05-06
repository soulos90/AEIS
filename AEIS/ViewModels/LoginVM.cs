using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class LoginVM : IVM
    {
        public bool LoggedIn { get; set; }
        public Security Active { get; }

        public LoginVM(bool loggedIn, Security active)
        {
            LoggedIn = loggedIn;
            Active = active;
        }
    }
}