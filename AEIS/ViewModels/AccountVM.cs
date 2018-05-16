using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;
using StateTemplateV5Beta.Controllers;
using System.Security.Cryptography;
using System.Text;

namespace StateTemplateV5Beta.ViewModels
{
    public class AccountVM : IVM
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Organization { get; }
        public Security Active { get; }

        public AccountVM(string uId, Security active)
        {
            UsersController usersController = new UsersController();
            User user = usersController.GetU(uId);

            if (user != null)
            {
                FirstName = user.FName.TrimEnd();
                LastName = user.LName.TrimEnd();
                Organization = user.Organization.TrimEnd();
            }
            else
            {
                FirstName = "NULL USER";
                LastName = "NULL USER";
                Organization = "NULL USER";
            }

            Active = active;
        }
    }
}