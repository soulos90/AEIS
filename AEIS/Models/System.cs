using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Controllers;

namespace StateTemplateV5Beta.Models
{
    public class System
    {
        public string SystemName;
        public int[] SectionScores { get; }

        public System(string systemNum)
        {
            string userId = HomeController.active.GetID();
            AnswersController aController = new AnswersController();
            Environment e = new Environment();
            
            int numQuestions = int.Parse(Environment.NumQus);

            for (int curQuestion = 1; curQuestion < numQuestions; curQuestion++)
            {
                
            }
        }
    }
}