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
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Organization { get; }
        public string PassHash { get; }
        public string PassSalt { get; }
        public Security Active { get; }

        public AccountVM(string uId, Security active)
        {
            UsersController usersController = new UsersController();
            User user = usersController.GetU(uId);

            if (user != null)
            {
                Email = user.ID;
                FirstName = user.FName;
                LastName = user.LName;
                Organization = user.Organization;
                PassHash = user.PassHash;
                PassSalt = user.PassSalt;
            }
            else
            {
                Email = "NULL USER";
                FirstName = "NULL USER";
                LastName = "NULL USER";
                Organization = "NULL USER";
            }

            Active = active;
        }

        public bool CheckPassword(string password)
        {
            string combined = password + PassSalt;
            string hashedPass;
            using (SHA512CryptoServiceProvider sha = new SHA512CryptoServiceProvider())
            {
                byte[] dataToHash = Encoding.UTF8.GetBytes(combined);
                byte[] hashed = sha.ComputeHash(dataToHash);
                hashedPass = Convert.ToBase64String(hashed);
            }

            return hashedPass == PassHash;
        }
    }
}