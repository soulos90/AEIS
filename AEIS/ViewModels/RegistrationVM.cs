using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class RegistrationVM : VMP
    {
        #region Properties
        public User user { get; }
        #endregion

        #region Constructor
        public RegistrationVM(User u, Security active) : base(active)
        {
            user = u;
        }
        #endregion
    }
} 