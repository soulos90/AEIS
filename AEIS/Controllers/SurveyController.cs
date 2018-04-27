using StateTemplateV5Beta.Models; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using StateTemplateV5Beta.ViewModels;

namespace StateTemplateV5Beta.Controllers
{
    public class SurveyController : Controller
    {
        // Name Survey
        public ActionResult NameSurvey(Security active)
        {
            if (!ModelState.IsValid)
                return View();

            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                IVM model = new QuestionVM();
                return RedirectToAction("Index", "Home", model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult StartSurvey(QuestionVM model, Security active)
        {
            if (!ModelState.IsValid)
                return View("NameSurvey");

            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                model = new QuestionVM();
                return RedirectToAction("Index", "Home", model);
            }
            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();
            string userId = Active.GetID();

            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM();
            surveyQuestionVM.QuestionText = eController.GetQuestionText(1);
            surveyQuestionVM.QId = 1;
            surveyQuestionVM.ProgramName = model.Name;
            surveyQuestionVM.AId = aController.GetNextAId(userId);

            using (var context = new DBAContext())
            {
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (1 == t.QId) & (surveyQuestionVM.AId == t.AId)) select t).FirstOrDefault();

                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;
            }

            return RedirectToAction("SurveyQuestions", surveyQuestionVM);
        }

        public ActionResult PreviousQuestion(SurveyQuestionVM model, Security active)
        {
            if (!ModelState.IsValid)           
                return View("SurveyQuestions", model);

            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                IVM modelo = new QuestionVM();
                return RedirectToAction("Index", "Home", modelo);
            }

            string userId = Active.GetID();
            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();
            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM();

            int i = model.QId;
            surveyQuestionVM.AId = model.AId;

            //checks to see if previous answer exists
            using (var context = new DBAContext())
            {
                //If the question was not answered then it gets a value of null
                Answer previousAnswer = new Answer();
                previousAnswer.AId = model.AId;
                previousAnswer.QId = model.QId;
                previousAnswer.UId = userId;
                previousAnswer.programName = model.ProgramName;
                previousAnswer.Value = model.Value;

                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (model.QId == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();
                //if the answer exists use Put, otherwise use Post
                if (CheckAnswer != null)
                    aController.PutAnswer(previousAnswer.UId, previousAnswer);
                else
                    aController.PostAnswer(previousAnswer);
            }
                
            surveyQuestionVM.ProgramName = model.ProgramName;
            surveyQuestionVM.QId = model.QId;
            i--;
            surveyQuestionVM.QuestionText = eController.GetQuestionText(i);
            surveyQuestionVM.QId = i;

            using (var context = new DBAContext())
            {
                //Checks to see if the Answer exists
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (i == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();

                //if it exists set the value for the question then load it in
                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;

                int Answers = (from t in context.Answers where ((model.ProgramName == t.programName) & (model.AId == t.AId)) select t).Count();
                surveyQuestionVM.Percent = (Answers / eController.GetQuestionCount() * 100);
                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();
            }

            if (i == 0)
                return RedirectToAction("Summary", surveyQuestionVM);
            
            return RedirectToAction("SurveyQuestions", surveyQuestionVM);
        }

        [HttpPost]
        public ActionResult NextQuestion(SurveyQuestionVM model, Security active)
        {
            if (!ModelState.IsValid)
                return View("SurveyQuestions", model);

            active = session(active);
            SecurityController Active = new SecurityController(active);

            if (!Active.CheckLogin())
            {
                IVM modelo = new QuestionVM();
                return RedirectToAction("Index", "Home", modelo);
            }

            string userId = Active.GetID();
            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM();
            var eController = new EnvironmentController();
            var aController = new AnswersController();

            surveyQuestionVM.ProgramName = model.ProgramName;
            surveyQuestionVM.AId = model.AId;
            int i = model.QId;

            //Save the Answer to the question just answered.
            using (var context = new DBAContext())
            {                
                Answer PreviousAnswer = new Answer();
                PreviousAnswer.QId = model.QId;
                PreviousAnswer.Value = model.Value;
                PreviousAnswer.programName = model.ProgramName;            
                PreviousAnswer.UId = userId;
                PreviousAnswer.AId = model.AId;

                //checks to see if the answer exists
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (i == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();
                //if the answer exists use Put, otherwise use Post
                if (CheckAnswer != null)
                    aController.PutAnswer(PreviousAnswer.UId, PreviousAnswer);
                else
                    aController.PostAnswer(PreviousAnswer);
            }
                
            // gets the next question
            surveyQuestionVM.QId = model.QId;
            i = surveyQuestionVM.QId;
            i++;
            surveyQuestionVM.QuestionText = eController.GetQuestionText(i);
            surveyQuestionVM.QId = i;

            //checks to see if the next question has an answer already
            using (var context = new DBAContext())
            {
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (i == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();

                int Answers = (from t in context.Answers where ((userId == t.UId) & (model.AId == t.AId)) select t).Count();

                surveyQuestionVM.Percent = (Answers / eController.GetQuestionCount());
                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();

                //sets the value for the next answer to the answer that exists
                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;
            }

            //redirects to the summary when it reachs the end
            int End = eController.GetQuestionCount();
            if (i > End)
                return RedirectToAction("Summary", surveyQuestionVM);

            return RedirectToAction("SurveyQuestions", surveyQuestionVM);
        }

        public ActionResult SurveyQuestions(SurveyQuestionVM model)
        {
            return View(model);
        }

        //TODO: Need to change it so that it is in line with the new models
        public ActionResult Summary(SurveyQuestionVM model)
        {
            SummaryVM summaryVM = new SummaryVM();
            summaryVM.ProgramName = model.ProgramName;
            var Controller = new StateTemplateV5Beta.Controllers.EnvironmentController();

            using (var context = new DBAContext())
            {
                //TODO: If there were questions that didnt have any yes or no. It would return an error
                //int End = Convert.ToInt16(Controller.GetQuestionCount());
                //int YesTotal = 0;
                //int NoTotal = 0;

                //for (int i=1; i <= End; i++)
                //{
                //    Answer CheckAnswer = (from t in context.Answers where ((model.ProgramName == t.programName) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();
                //    if (CheckAnswer.Value == null)
                //        break;
                //    else if (CheckAnswer.Value == true)
                //        YesTotal += Convert.ToInt16(Controller.GetQuestionYesVal(i));
                //    else if (CheckAnswer.Value == false)
                //        NoTotal += Convert.ToInt16(Controller.GetQuestionYesVal(i));
                //}
                //ViewModel.Points = (YesTotal + NoTotal);

            }
            return View(summaryVM);
        }

        private Security session(Security active)
        {
            if (active == null)
                active = new Security();
            return active;
        }
    }
}