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
        UsersController UController = new UsersController();
        public ActionResult NameSurvey(string actives, string activeLog, string activeRem)
        {
            Security active = session(actives.Trim(), activeLog.Trim(), activeRem.Trim());
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index", "Home");
            }

            QuestionVM model = new QuestionVM(active);
            return View(model);
        }

        [HttpPost]
        public ActionResult StartSurvey(string actives, string activeLog, string activeRem, QuestionVM model)
        {

            Security active = session(actives.Trim(), activeLog.Trim(), activeRem.Trim());
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                //LoginVM model = new LoginVM(active.IsLoggedIn, active);
                return RedirectToAction("Index", "Home");
            }

            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();
            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);
            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = Active.GetID();

            if (Request.Form["btnEditSurvey"] != null)
            {
                Answer a = aController.GetAnswer(userId, int.Parse(Request.Form["btnEditSurvey"]));
                surveyQuestionVM.QuestionText = eController.GetQuestionText(1);
                surveyQuestionVM.AId = a.AId;
                surveyQuestionVM.QId = a.QId;
                surveyQuestionVM.ProgramName = a.programName;
            }
            else
            {
                surveyQuestionVM.QuestionText = eController.GetQuestionText(1);
                surveyQuestionVM.AId = aController.GetNextAId(userId);
                surveyQuestionVM.QId = 1;
                surveyQuestionVM.ProgramName = model.Name;
            }

            using (var context = new DBAContext())
            {
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (1 == t.QId) & (surveyQuestionVM.AId == t.AId)) select t).FirstOrDefault();

                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;
            }

            return RedirectToAction("SurveyQuestions", surveyQuestionVM);
        }

        public ActionResult PreviousQuestion(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {
            if (!ModelState.IsValid)
                return View("SurveyQuestions", model);


            Security active = session(actives.Trim(), activeLog.Trim(), activeRem.Trim());
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                //LoginVM lmodel = new LoginVM(active.IsLoggedIn, active);
                return RedirectToAction("Index", "Home");
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = cookie.Values["ID"];
            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();
            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);

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
                {
                    previousAnswer.Created = CheckAnswer.Created;
                    aController.PutAnswer(previousAnswer.UId, previousAnswer);
                }
                else
                    aController.PostAnswer(previousAnswer);
            }

            surveyQuestionVM.ProgramName = model.ProgramName;
            surveyQuestionVM.QId = model.QId;
            i--;

            if (i <= 0)
                return RedirectToAction("Inventory", "Home");

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

            return RedirectToAction("SurveyQuestions", surveyQuestionVM);
        }

        [HttpPost]
        public ActionResult NextQuestion(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {
            if (!ModelState.IsValid)
                return View("SurveyQuestions", model);

            Security active = session(actives.Trim(), activeLog.Trim(), activeRem.Trim());
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                //LoginVM lmodel = new LoginVM(active.IsLoggedIn, active);
                return RedirectToAction("Index", "Home");
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = cookie.Values["ID"];
            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);
            var eController = new EnvironmentController();
            var aController = new AnswersController();

            surveyQuestionVM.AId = model.AId;
            surveyQuestionVM.ProgramName = model.ProgramName;
            int i = model.QId;
            //Save the Answer to the question just answered.
            using (var context = new DBAContext())
            {
                Answer previousAnswer = new Answer();
                previousAnswer.QId = model.QId;
                previousAnswer.Value = model.Value;
                previousAnswer.programName = model.ProgramName;
                previousAnswer.UId = userId;
                previousAnswer.AId = model.AId;

                //checks to see if the answer exists
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (i == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();
                //if the answer exists use Put, otherwise use Post
                if (CheckAnswer != null)
                {
                    previousAnswer.Created = CheckAnswer.Created;
                    aController.PutAnswer(previousAnswer.UId, previousAnswer);
                }
                else
                    aController.PostAnswer(previousAnswer);
            }

            // gets the next question
            surveyQuestionVM.QId = model.QId;
            i = surveyQuestionVM.QId;
            i++;

            //redirects to the summary when it reachs the end
            int End = eController.GetQuestionCount();
            if (i > End)
                return RedirectToAction("Inventory", "Home");

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

            
            return View(summaryVM);
        }

        private Security session(Security active)
        {
            if (active == null)
                active = new Security();
            return active;
        }

        private Security session(string active, string activeLog, string rem)
        {
            Security Active;
            if (active == null)
            {
                active = "";
            }
            if (activeLog == null)
            {
                activeLog = "False";
            }
            if (rem == null)
            {
                rem = "False";
            }
            Active = new Security(active, activeLog.Equals("True"), rem.Equals("True"));
            return Active;
        }

        public SecurityController IsLoggedIn(SecurityController active)
        {
            bool value = false;
            string decodedUser = "";
            bool remember = false;
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                decodedUser = HttpUtility.HtmlDecode(cookie.Values["ID"]);
                value = HttpUtility.HtmlDecode(cookie.Values["LoggedIn"]).Equals("True");
                remember = HttpUtility.HtmlDecode(cookie.Values["Remember"]).Equals("True");
            }
            if (value)
            {
                active.Login(decodedUser);
                active.SetRemember(remember);
            }
            return active;
        }
    }
}