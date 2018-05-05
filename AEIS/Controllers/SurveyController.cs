using StateTemplateV5Beta.Models; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace StateTemplateV5Beta.Controllers
{

    public class SurveyController : Controller
    {
        private DBAContext db = new DBAContext();

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Page1(QuestionViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View("Index");
            }
            else
            {
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();

                var Controller = new StateTemplateV5Beta.Controllers.EnvironmentController();
                var AnswerController = new StateTemplateV5Beta.Controllers.AnswersController();
                var User = new StateTemplateV5Beta.Controllers.UsersController();

                viewModel.Question = Controller.GetQuestionText(1);
                viewModel.CurrentID = 1;
                viewModel.ProgramName = model.Name;
                //TODO: switch to the correct stuff after testing
                viewModel.aID = AnswerController.Next(Security.ID);
                
                //viewModel.aID = 9007;

                using (var context = new DBAContext())
                {
                    Answer CheckAnswer = (from t in context.Answers where ((Security.ID == t.UId) & (1 == t.QId) & (viewModel.aID == t.AId)) select t).FirstOrDefault();
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
        public ActionResult PreviousQuestion(SurveyQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View("SurveyQuestions", model);
            }
            else
            {
                var PreviousQuestionPress = new StateTemplateV5Beta.Controllers.EnvironmentController();
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                var AnswerController = new StateTemplateV5Beta.Controllers.AnswersController();

                int i = model.CurrentID;
                viewModel.aID = model.aID;

                //checks to see if the answer exists
                using (var context = new DBAContext())
                {

                    Answer CheckAnswer = (from t in context.Answers where ((Security.ID == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

                    //If the question was not answered then it gets a value of null
                    Answer PreviousAnswer = new Answer();
                    PreviousAnswer.QId = model.CurrentID;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.programName = model.ProgramName;                      
                    PreviousAnswer.UId = Security.ID;
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
                    Answer CheckAnswer = (from t in context.Answers where ((Security.ID == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

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
        public ActionResult AnswerQuestion(SurveyQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View("SurveyQuestions", model);
            }
            else
            {

                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                var Controller = new StateTemplateV5Beta.Controllers.EnvironmentController();
                var AnswerController = new StateTemplateV5Beta.Controllers.AnswersController();

                viewModel.ProgramName = model.ProgramName;
                viewModel.aID = model.aID;
                int i = model.CurrentID;

                //Save the Answer to the question just answered.
                using (var context = new DBAContext())
                {
                    //checks to see if the answer exists
                    Answer CheckAnswer = (from t in context.Answers where ((Security.ID == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

                    //Checks to see if the question was answered if it wasnt then it should save an answer
                    //If the question was not answered then it get a value of null
                    Answer PreviousAnswer = new Answer();
                    PreviousAnswer.QId = model.CurrentID;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.programName = model.ProgramName;            
                    PreviousAnswer.UId = Security.ID;
                    PreviousAnswer.AId = model.aID;
                    //PreviousAnswer.UId = "Moo5"; for testing

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
                

                // gets the next question
                viewModel.CurrentID = model.CurrentID;
                i = viewModel.CurrentID;
                i++;
                viewModel.Question = Controller.GetQuestionText(i);
                viewModel.CurrentID = i;

                //checks to see if the next question has an answer already
                using (var context = new DBAContext())
                {
                    Answer CheckAnswer = (from t in context.Answers where ((Security.ID == t.UId) & (i == t.QId) & (model.aID == t.AId)) select t).FirstOrDefault();

                    int Answers = (from t in context.Answers where ((Security.ID == t.UId) & (model.aID == t.AId)) select t).Count();

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
    }
}
    
    

