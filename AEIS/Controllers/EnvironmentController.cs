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
        public int GetQuestionCount()
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
            return Models.Environment.Sections[i].name;
        }
        public string GetSectionFirst(int i)
        {
            return Models.Environment.Sections[i].first;
        }
        public string GetSectionLast(int i)
        {
            return Models.Environment.Sections[i].last;
        }
        public string GetQuestionText(int i)
        {
            return Models.Environment.Questions[i].text;
        }
        public string GetQuestionYesVal(int i)
        {
            return Models.Environment.Questions[i].YesVal;
        }
        public string GetQuestionNoVal(int i)
        {
            return Models.Environment.Questions[i].NoVal;
        }
        public string GetQuestionReliesOn(int i)
        {
            return Models.Environment.Questions[i].ReliesOn;
        }
    }
}