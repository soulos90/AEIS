using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class AccountVM : VMP
    {
        #region Properties
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Organization { get; }
        #endregion

        #region Constructor
        public AccountVM(string uId, Security active) : base(active)
        {
            AccountInfo accountInfo = new AccountInfo(uId);
            Email = accountInfo.Email;
            FirstName = accountInfo.FirstName;
            LastName = accountInfo.LastName;
            Organization = accountInfo.Organization;
        }
        #endregion
    }
}