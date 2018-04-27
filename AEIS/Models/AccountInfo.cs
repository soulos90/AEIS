using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Controllers;

namespace StateTemplateV5Beta.Models
{
    public class AccountInfo
    {
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Organization { get; }

        public AccountInfo(string uId)
        { 
            UsersController usersController = new UsersController();
            User user = usersController.GetU(uId);

            if (user != null)
            {
                Email = user.ID;
                FirstName = user.FName;
                LastName = user.LName;
                Organization = user.Organization;
            }
            else
            {
                Email = "NULL USER";
                FirstName = "NULL USER";
                LastName = "NULL USER";
                Organization = "NULL USER";
            }
        }
    }
}