using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StateTemplateV5Beta.Models;
/*
namespace StateTemplateV5Beta.Controllers
{
    public class CalculationController
    {
        #region Fields
       // private QuestionsController qController = new QuestionsController();
       private EnvironmentController qController = StateTemplateV5Beta.MvcApplication.environment;
        private AnswersController aController = new AnswersController();
        private const int MAX_SCORE = 900;
        #endregion

        #region Constructor
        public CalculationController()
        {

        }
        #endregion

        #region Methods
        // uses answers from questions 1-4
        public int GetPublicServicesScore(string entryId)
        {
            int publicServicesScore = 0;
            Answer a = aController.GetA(entryId);

            //Question q = qController.GetQ(1);
            if (a.q1 == "YES")
                publicServicesScore += qController.GetQuestionYesVal(1);
            else if (a.q1 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(2);
            if (a.q2 == "YES")
                publicServicesScore += q.yesVal;
            else if (a.q2 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(3);
            if (a.q3 == "YES")
                publicServicesScore += q.yesVal;
            else if (a.q3 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(4);
            if (a.q4 == "YES")
                publicServicesScore += q.yesVal;
            else if (a.q4 == "NO")
                publicServicesScore += q.noVal;

            return (publicServicesScore * 100) / MAX_SCORE;
        }

        // uses answers from questions 5-6
        public int GetInternalServicesScore(string entryId)
        {
            int internalServicesScore = 0;
            Answer a = aController.GetA(entryId);

            Question q = qController.GetQ(5);
            if (a.q5 == "YES")
                internalServicesScore += q.yesVal;
            else if (a.q5 == "NO")
                internalServicesScore += q.noVal;

            q = qController.GetQ(6);
            if (a.q6 == "YES")
                internalServicesScore += q.yesVal;
            else if (a.q6 == "NO")
                internalServicesScore += q.noVal;

            return (internalServicesScore * 100) / MAX_SCORE;
        }

        //uses answers from questions 7-8 (7.1-7.2 on Handout)
        public int GetAccessibilityScore(string entryId)
        {
            int accessibilityScore = 0;
            Answer a = aController.GetA(entryId);

            Question q = qController.GetQ(7);
            if (a.q7 == "YES")
                accessibilityScore += q.yesVal;
            else if (a.q7 == "NO")
                accessibilityScore += q.noVal;

            q = qController.GetQ(8);
            if (a.q8 == "YES")
                accessibilityScore += q.yesVal;
            else if (a.q8 == "NO")
                accessibilityScore += q.noVal;

            return (accessibilityScore * 100) / MAX_SCORE;
        }

        //uses answers from questions 9-10 (8-9 on Handout) 
        public int GetLifeSpanScore(string entryId)
        {
            int lifeSpanScore = 0;
            Answer a = aController.GetA(entryId);

            Question q = qController.GetQ(9);
            if (a.q9 == "YES")
                lifeSpanScore += q.yesVal;
            else if (a.q9 == "NO")
                lifeSpanScore += q.noVal;

            q = qController.GetQ(10);
            if (a.q10 == "YES")
                lifeSpanScore += q.yesVal;
            else if (a.q10 == "NO")
                lifeSpanScore += q.noVal;

            return (lifeSpanScore * 100) / MAX_SCORE;
        }

        public int GetTotalScore(string entryId)
        {
            return GetPublicServicesScore(entryId) + GetInternalServicesScore(entryId) +
                GetAccessibilityScore(entryId) + GetLifeSpanScore(entryId);
        }
        #endregion
    }
}*/