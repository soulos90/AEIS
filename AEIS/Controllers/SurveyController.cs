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
        private DBAContext db = new DBAContext();

        // GET: Default
        public ActionResult Index(string actives, string activeLog, string activeRem)
        {
            SecurityController active = new SecurityController(session(actives, activeLog, activeRem));
            if (!(IsLoggedIn(active).CheckLogin()))
            {
                IVM pass = new LoginVM(active.GetActive().IsLoggedIn, active.GetActive());
                return View("Index", "Home",pass);
            }
            QuestionViewModel model = new QuestionViewModel();
            model.Active = active.GetActive();
            return View(model);
        }

        [HttpPost]
        public ActionResult Page1(QuestionViewModel model, string actives, string activeLog, string activeRem)
        {
            SecurityController active = new SecurityController(session(actives, activeLog, activeRem));
            if (!(IsLoggedIn(active).CheckLogin()))
            {
                IVM pass = new LoginVM(active.GetActive().IsLoggedIn, active.GetActive());
                return View("Index", "Home",pass);
            }
            if (!ModelState.IsValid)
            {
                return View("Index",model);
            }
            else
            {
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                viewModel.Active = active.GetActive();

                var Controller = MvcApplication.environment;
                var AnswerController = new AnswersController();
                var User = new UsersController();

                viewModel.Question = Controller.GetQuestionText(1);
                viewModel.CurrentID = 1;
                viewModel.ProgramName = model.Name;
                viewModel.aID = AnswerController.GetNextAId(active.GetID());
                
                {
                    Answer CheckAnswer = AnswerController.GetA(viewModel.Active.ID,viewModel.aID,1);
                    if (CheckAnswer != null)
                        viewModel.Answer = CheckAnswer.Value;
                }

                return RedirectToAction("SurveyQuestions", viewModel);
            }

        }

        public ActionResult SurveyQuestions(SurveyQuestionViewModel model)
        {

            return View(model);
        }

        //This is for when someone presses the Previous button
        public ActionResult PreviousQuestion(SurveyQuestionViewModel model, string actives, string activeLog, string activeRem)
        {
            SecurityController active = new SecurityController(session(actives, activeLog, activeRem));
            if (!(IsLoggedIn(active).CheckLogin()))
            {
                return View("Index", "Home");
            }
            if (!ModelState.IsValid)
            {

                return View("SurveyQuestions", model);
            }
            else
            {
                var PreviousQuestionPress = StateTemplateV5Beta.MvcApplication.environment;
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                viewModel.Active = active.GetActive();
                var AnswerController = new StateTemplateV5Beta.Controllers.AnswersController();

                int i = model.CurrentID;
                viewModel.aID = model.aID;

                //checks to see if the answer exists
                using (var context = new DBAContext())
                {

                    Answer CheckAnswer = (from t in context.Answers where ((active.GetID() == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

                    //If the question was not answered then it gets a value of null
                    Answer PreviousAnswer = new Answer();
                    PreviousAnswer.QId = model.CurrentID;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.programName = model.ProgramName;                      
                    PreviousAnswer.UId = active.GetID();
                    //PreviousAnswer.UId = "Moo5"; test

                    PreviousAnswer.AId = model.aID;

                    //if the answer exists use Put
                    if (CheckAnswer != null)
                    {
                        AnswerController.PutAnswer(PreviousAnswer.UId, PreviousAnswer);
                    }

                    //if not use Post
                    else
                    {
                        AnswerController.PostAnswer(PreviousAnswer);
                    }
                }
                

                viewModel.ProgramName = model.ProgramName;
                viewModel.CurrentID = model.CurrentID;
                i--;
                viewModel.Question = PreviousQuestionPress.GetQuestionText(i);
                viewModel.CurrentID = i;

                //Checks to see if the Answer exists
                using (var context = new DBAContext())
                {
                    Answer CheckAnswer = (from t in context.Answers where ((active.GetID() == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

                    //if it exists set the value for the question then load it in
                    if (CheckAnswer != null)
                        viewModel.Answer = CheckAnswer.Value;

                    int Answers = (from t in context.Answers where ((model.ProgramName == t.programName) & (model.aID == t.AId)) select t).Count();
                    viewModel.Percent = (Answers / PreviousQuestionPress.GetQuestionCount() * 100);
                    viewModel.NumberofQuestions = PreviousQuestionPress.GetQuestionCount();
                }

                if (i == 0)
                {
                    return RedirectToAction("Summary", "Survey", viewModel);

                }
                else
                    return RedirectToAction("SurveyQuestions", viewModel);
            }
        }

        [HttpPost]
        public ActionResult AnswerQuestion(SurveyQuestionViewModel model, string actives, string activeLog, string activeRem)
        {
            SecurityController active = new SecurityController(session(actives, activeLog, activeRem));
            if (!(IsLoggedIn(active).CheckLogin()))
            {
                return View("..Home/Index", new LoginVM(active.GetActive().IsLoggedIn, active.GetActive()));
            }
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            else
            {
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                viewModel.Active = active.GetActive();
                var Controller = MvcApplication.environment;
                var AnswerController = new AnswersController();

                viewModel.ProgramName = model.ProgramName;
                viewModel.aID = model.aID;
                int i = model.CurrentID;

                //Save the Answer to the question just answered.
                {
                    //checks to see if the answer exists
                    Answer CheckAnswer = AnswerController.GetA(model.Active.ID, model.aID, i);

                    //Checks to see if the question was answered if it wasnt then it should save an answer
                    //If the question was not answered then it get a value of null
                    Answer PreviousAnswer = new Answer();
                    PreviousAnswer.QId = i;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.programName = model.ProgramName;
                    PreviousAnswer.UId = model.Active.ID;
                    PreviousAnswer.AId = model.aID;

                    //PreviousAnswer.UId = "Moo5"; for testing

                    //if the answer exists use Put
                    if (CheckAnswer != null)
                    {
                        PreviousAnswer.Created = CheckAnswer.Created;
                        AnswerController.PutAnswer(PreviousAnswer.UId, PreviousAnswer);
                    }

                    //if not use Post
                    else
                    {
                        AnswerController.PostAnswer(PreviousAnswer);
                    }
                }
                

                // gets the next question
                viewModel.CurrentID = i = 1 + model.CurrentID;
                viewModel.Question = Controller.GetQuestionText(i);

                //checks to see if the next question has an answer already
                
                {
                    Answer CheckAnswer = AnswerController.GetA(viewModel.Active.ID, viewModel.aID, i);

                    int Answers = AnswerController.CountAPS(viewModel.Active.ID, viewModel.aID);

                    viewModel.Percent = (Answers / Controller.GetQuestionCount());
                    viewModel.NumberofQuestions = Controller.GetQuestionCount();

                    //sets the value for the next answer to the answer that exists
                    if (CheckAnswer != null)
                        viewModel.Answer = CheckAnswer.Value;

                }

                //redirects to the summary when it reachs the end
                int End = Convert.ToInt16(Controller.GetQuestionCount());
                if (i > End)
                {
                    return RedirectToAction("Summary", "Survey", viewModel);

                }

            return RedirectToAction("SurveyQuestions", viewModel);
            }

        }


        //TODO: Need to change it so that it is in line with the new models
        public ActionResult Summary(SurveyQuestionViewModel model)
        {
            SummaryViewModel ViewModel = new SummaryViewModel();
            ViewModel.ProgramName = model.ProgramName;
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
            return View(ViewModel);
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
    
    

