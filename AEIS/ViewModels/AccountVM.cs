using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class AccountVM
    {
        #region Properties
        public string FirstName { get; }
        public string LastName { get; }
        public string Organization { get; }
        #endregion

        #region Constructor
        public AccountVM(string uId)
        {
            AccountInfo accountInfo = new AccountInfo(uId);
            FirstName = accountInfo.FirstName;
            LastName = accountInfo.LastName;
            Organization = accountInfo.Organization;
        }
        #endregion
    }
}