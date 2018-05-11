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
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index", "Home");
            }
            QuestionVM model = new QuestionVM(active);
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult StartSurvey(string actives, string activeLog, string activeRem, QuestionVM model)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin()|| !ModelState.IsValid))
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = cookie.Values["ID"];

            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);
            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();

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
                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();
                surveyQuestionVM.ProgramName = model.Name;

            }

            using (var context = new DBAContext())
            {
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (1 == t.QId) & (surveyQuestionVM.AId == t.AId)) select t).FirstOrDefault();

                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;

            }
            surveyQuestionVM.AnsweredQuestions = GetAnsweredList(userId, surveyQuestionVM.AId);
            surveyQuestionVM.DisableQuestion = GetDisable(userId, surveyQuestionVM.AId, surveyQuestionVM.AnsweredQuestions);

            ModelState.Clear();
            return View("SurveyQuestions", surveyQuestionVM);
        }

        public ActionResult PreviousQuestion(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {
            
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin() || !ModelState.IsValid))
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = cookie.Values["ID"];

            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);
            AnswersController aController = new AnswersController();
            EnvironmentController eController = new EnvironmentController();

            int i = model.QId;
            surveyQuestionVM.AId = model.AId;

            if (model.Value != null)
            {
                //checks to see if previous answer exists
                using (var context = new DBAContext())
                {
                    Answer CheckName = (from t in context.Answers where ((userId == t.UId) & (model.AId == t.AId)) select t).FirstOrDefault();
                    if (CheckName != null && CheckName.programName != surveyQuestionVM.ProgramName)
                        RenameProgram(userId, model.AId, surveyQuestionVM.ProgramName);

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
            }

            surveyQuestionVM.AnsweredQuestions = GetAnsweredList(userId, surveyQuestionVM.AId);
            surveyQuestionVM.DisableQuestion = GetDisable(userId, surveyQuestionVM.AId, surveyQuestionVM.AnsweredQuestions);
            surveyQuestionVM.ProgramName = model.ProgramName;
            surveyQuestionVM.QId = model.QId;

            i--;
            if (surveyQuestionVM.DisableQuestion.Exists(x => x == i))
            {
                i -= 1;
            }

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

                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();
            }
            ModelState.Clear();
            return View("SurveyQuestions", surveyQuestionVM);
        }

        [HttpPost]
        public ActionResult NextQuestion(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {

            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin() || !ModelState.IsValid))
            {
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
            if (model.Value != null)
            {
                //Save the Answer to the question just answered.
                using (var context = new DBAContext())
                {
                    Answer CheckName = (from t in context.Answers where ((userId == t.UId) & (model.AId == t.AId)) select t).FirstOrDefault();
                    if (CheckName != null && CheckName.programName != surveyQuestionVM.ProgramName)
                            RenameProgram(userId, model.AId, surveyQuestionVM.ProgramName);

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
            }

            surveyQuestionVM.AnsweredQuestions = GetAnsweredList(userId, surveyQuestionVM.AId);
            surveyQuestionVM.DisableQuestion = GetDisable(userId, surveyQuestionVM.AId, surveyQuestionVM.AnsweredQuestions);

            surveyQuestionVM.QuestionText = eController.GetQuestionText(i);

            // gets the next question
            surveyQuestionVM.QId = model.QId;
            i = surveyQuestionVM.QId;
            
            i++;
            if (surveyQuestionVM.DisableQuestion.Exists(x => x == i))
            {
                i += 1;
            }

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

                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();

                //sets the value for the next answer to the answer that exists
                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;
            }
            ModelState.Clear();
            return View("SurveyQuestions", surveyQuestionVM);
        }

        public ActionResult SkipQuestion(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);

            if (!(IsLoggedIn(Active).CheckLogin() || !ModelState.IsValid))
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            string userId = cookie.Values["ID"];

            SurveyQuestionVM surveyQuestionVM = new SurveyQuestionVM(active);
            var eController = new EnvironmentController();
            var aController = new AnswersController();

            surveyQuestionVM.AId = model.AId;
            surveyQuestionVM.ProgramName = model.ProgramName;

            if (model.Value != null)
            {
                //Save the Answer to the question just answered.
                using (var context = new DBAContext())
                {
                    Answer CheckName = (from t in context.Answers where ((userId == t.UId) & (model.AId == t.AId)) select t).FirstOrDefault();
                    if (CheckName != null && CheckName.programName != surveyQuestionVM.ProgramName)
                        RenameProgram(userId, model.AId, surveyQuestionVM.ProgramName);

                    Answer previousAnswer = new Answer();
                    previousAnswer.QId = model.QId;
                    previousAnswer.Value = model.Value;
                    previousAnswer.programName = model.ProgramName;
                    previousAnswer.UId = userId;
                    previousAnswer.AId = model.AId;

                    //checks to see if the answer exists
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
            }
            surveyQuestionVM.AnsweredQuestions = GetAnsweredList(userId, surveyQuestionVM.AId);
            surveyQuestionVM.DisableQuestion = GetDisable(userId, surveyQuestionVM.AId, surveyQuestionVM.AnsweredQuestions);

            int i = model.SkipTo;
            surveyQuestionVM.QuestionText = eController.GetQuestionText(i);
            surveyQuestionVM.QId = model.SkipTo;

            //checks to see if the next question has an answer already
            using (var context = new DBAContext())
            {
                Answer CheckAnswer = (from t in context.Answers where ((userId == t.UId) & (model.SkipTo == t.QId) & (model.AId == t.AId)) select t).FirstOrDefault();

                surveyQuestionVM.NumberofQuestions = eController.GetQuestionCount();

                //sets the value for the next answer to the answer that exists
                if (CheckAnswer != null)
                    surveyQuestionVM.Value = CheckAnswer.Value;
            }

            ModelState.Clear();
            return View("SurveyQuestions", surveyQuestionVM);

        }

        public ActionResult SurveyQuestions(string actives, string activeLog, string activeRem, SurveyQuestionVM model)
        {
            Security active = session(actives, activeLog, activeRem);
            SecurityController Active = new SecurityController(active);
            if (!(IsLoggedIn(Active).CheckLogin()))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.Clear();
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
        
        private List<int> GetDisable(string userID, int Aid, List<int> AnsweredQuestion)
        {
            var eController = new EnvironmentController();

            List<int> iDisable = new List<int>();
            for(int i =1; i <= eController.GetQuestionCount(); i++)
            {
                eController.GetQuestionReliesOn(i);
                if (Convert.ToInt16(eController.GetQuestionReliesOn(i)) != 0)
                {
                    using (var context = new DBAContext())
                    {
                        int AnswerForRlies = Convert.ToInt16(eController.GetQuestionReliesOn(i));
                        Answer CheckAnswer = (from t in context.Answers where ((userID == t.UId) & (AnswerForRlies == t.QId) & (Aid == t.AId)) select t).FirstOrDefault();
                        if (CheckAnswer == null || CheckAnswer.Value == false)
                            iDisable.Add(i);
                    }
                }                
            }

            return iDisable;
        }

        private List<int> GetAnsweredList(string userID, int Aid)
        {
            using (var context = new DBAContext())
            {
                IEnumerable<Answer> CheckName = (from t in context.Answers where ((userID == t.UId) & (Aid == t.AId)) select t).ToList();
                List<int> iList = new List<int>();
                foreach (var answer in CheckName)
                {
                    iList.Add(answer.QId);
                }
                return iList;
            }
        }

        private void RenameProgram(string userID, int Aid, string NewName)
        {
            using (var context = new DBAContext())
            {
                IEnumerable<Answer> CheckName = (from t in context.Answers where ((userID == t.UId) & (Aid == t.AId)) select t).ToList();

                foreach (var answer in CheckName)
                {
                    answer.programName = NewName;
                    context.SaveChanges();
                }
            }
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
                activeLog = "false";
            }
            if (rem == null)
            {
                rem = "false";
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