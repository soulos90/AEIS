using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.Controllers
{
    public class EnvironmentController : Controller
    {
        private static Models.Environment Env;
        // GET: Environment
        public EnvironmentController()
        {
            Env = new Models.Environment();
        }
        public int GetSectionCount()
        {
            return Models.Environment.NumSec;
        }
        public string GetQuestionCount()
        {
            return Models.Environment.NumQus;
        }
        public string GetDBSource()
        {
            return Models.Environment.DBSource;
        }
        public string GetInitCatalog()
        {
            return Models.Environment.InitCat;
        }
        public string GetDBUser()
        {
            return Models.Environment.User;
        }
        public string GetDBPassword()
        {
            return Models.Environment.Password;
        }
        public string GetSectionName(int i)
        {
            return Env.GetSectionName(i-1);
        }
        public string GetSectionFirst(int i)
        {
            return Env.GetSectionFirst(i-1);
        }
        public string GetSectionLast(int i)
        {
            return Env.GetSectionLast(i-1);
        }
        public string GetQuestionText(int i)
        {
            return Env.GetQuestionText(i - 1);
        }
        public string GetQuestionYesVal(int i)
        {
            return Env.GetQuestionYV(i-1);
        }
        public string GetQuestionNoVal(int i)
        {
            return Env.GetQuestionNV(i-1);
        }
        public string GetQuestionReliesOn(int i)
        {
            return Env.GetQuestionRO(i-1);
        }
    }
}