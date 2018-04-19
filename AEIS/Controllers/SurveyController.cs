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
                
                viewModel.Question = Controller.GetQuestionText(1);
                viewModel.CurrentID = 1;
                viewModel.ProgramName = model.Name;
                // Need to check if the answer exists || old example
                        //Answer2 CheckAnswer = (from t in context.A2 where ((viewModel.CurrentID == t.Question.Id) & (viewModel.ProgramName == t.programName)) select t).FirstOrDefault();

                        // This is mostly for when you are modifing a previously answered survey, it checks to see if the answer exists already and sets the value
                        // and would get changed in the AnswerSurvey action.
                        //if (CheckAnswer != null)
                        //{
                        //    viewModel.Answer = CheckAnswer.Value;
                        //}

                    //}
                //}

                // This redirects to an "actionName", with an Object so that we can pass around the viewmodel
                return RedirectToAction("SurveyQuestions", viewModel);
            }
            
        }

        public ActionResult SurveyQuestions(SurveyQuestionViewModel model)
        {
               
            return View(model);
        }

        
        //This is for when someone presses the Previous button. Do i make this a httpost? it works without it.
        public ActionResult PreviousQuestion(SurveyQuestionViewModel model)
        {
            var PreviousQuestionPress = new StateTemplateV5Beta.Controllers.EnvironmentController();

            if (!ModelState.IsValid)
            {

                return View("SurveyQuestions", model);
            }
            else
            {
                
                //TODO: Check if answer exists. If it does update with new value if not create new answer
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                //      Check if the answer exists || an old example
                //    //TODO: to replace the answer in the database with the new answer
                //    if (CheckAnswer != null)
                //    {
                //        viewModel.Answer = CheckAnswer.Value;

                //    }

                //if the answer does not exists then
                //Saves the recently submitted answer
                //else
                //{

                //    Question2 previousQuestion = (from s in context.Q2 where (model.CurrentID == s.Id) select s).FirstOrDefault();
                //    Answer2 PreviousAnswer = new Answer2();

                //    PreviousAnswer.Question = previousQuestion;
                //    PreviousAnswer.Value = model.Answer;
                //    PreviousAnswer.Created = DateTime.Now;
                //    PreviousAnswer.programName = model.ProgramName;
                //    context.A2.Add(PreviousAnswer);
                //    context.SaveChanges();

                //}


                viewModel.ProgramName = model.ProgramName;

                //TODO: what happens when we reach the end
                viewModel.CurrentID = model.CurrentID;
                int i = viewModel.CurrentID; 
                i--;
                viewModel.Question = PreviousQuestionPress.GetQuestionText(i);
                viewModel.CurrentID = i;

                //Question2 PreviousQuestionPress = (from s in context.Q2 where (model.CurrentID == s.Id) select s).First();
                //CheckAnswer = (from t in context.A2 where ((PreviousQuestionPress.Id == t.Question.Id) & (model.ProgramName == t.programName)) select t).FirstOrDefault();

                //if (PreviousQuestionPress != null)
                //{
               // viewModel.Question = PreviousQuestionPress(i);

                    //    if (CheckAnswer != null)
                    //    {
                    //        viewModel.Answer = CheckAnswer.Value;
                    //    }

                    //}
                    //else
                    //{
                    //    return RedirectToAction("Summary", "Survey", viewModel);
                    //}

               // }
                return RedirectToAction("SurveyQuestions", viewModel);
            }
        }

        [HttpPost]    
        public ActionResult AnswerQuestion(SurveyQuestionViewModel model)
        {
            var Controller = new StateTemplateV5Beta.Controllers.EnvironmentController();
            SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();

            if (!ModelState.IsValid)
            {

                return View("SurveyQuestions", model);
            }
            else
            {
                //TODO: check to see if the answer exists already 
                //Save the Answer to the question just answered.
                var AnswerController = new StateTemplateV5Beta.Controllers.AnswersController();
                using (var context = new DBAContext())
                {
                    Answer PreviousAnswer = new Answer();
                    PreviousAnswer.QId = model.CurrentID;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.programName = model.ProgramName;
                    PreviousAnswer.UId = "testing";
                    //PreviousAnswer.AId = 199;

                    //context.Answers.Add(PreviousAnswer);
                    //context.SaveChanges();
                    AnswerController.PostAnswer(PreviousAnswer);

                //        Question2 previousQuestion = (from s in context.Q2 where (model.CurrentID == s.Id) select s).FirstOrDefault();
                //        Answer2 PreviousAnswer = new Answer2();                

                // gets the next question
                    viewModel.CurrentID = model.CurrentID;

                    int i = viewModel.CurrentID;
                    i++;
                    viewModel.Question = Controller.GetQuestionText(i);
                    viewModel.CurrentID = i;

                    //checks to see if the next question has an answer already
                    //CheckAnswer = (from t in context.A2 where ((nextQuestion.Id == t.Question.Id) & (model.ProgramName == t.programName)) & (currentuser.id == t. select t).FirstOrDefault();

                    //if (nextQuestion != null)
                    //{
                    //    //If there is an answer to the question already then set the value for the viewmodel to it.
                    //    if (CheckAnswer != null)
                    //    {
                    //        viewModel.Answer = CheckAnswer.Value;
                    //    }

                    // sets the Values for the next question
                    //viewModel.Question = nextQuestion.Text;
                    //        viewModel.CurrentID = nextQuestion.Id;                       

                }
            }
                    // TODO: when we reach the end it should redirect us to a summary page.
                    //else
                    //{
                    //    return RedirectToAction("Summary", "Survey", viewModel);
                    //}

                //}

                return RedirectToAction("SurveyQuestions", viewModel);
            }
        }

    //TODO: Need to change it so that it is in line with the new models
    //public ActionResult Summary(SurveyQuestionViewModel model)
    //{
    //    SummaryViewModel ViewModel = new SummaryViewModel();
    //    ViewModel.ProgramName = model.ProgramName;

    //    using (var context = new DBAContext())
    //    {
    //        //TODO: If there were questions that didnt have any yes or no. It would return an error
    //        int YesSum = (from SS in context.A2 where (SS.programName == model.ProgramName && SS.Value == true) select SS.Question.yesPointValue).Sum();
    //        int NoSum = (from SS in context.A2 where (SS.programName == model.ProgramName && SS.Value == false) select SS.Question.noPointValue).Sum();
    //        ViewModel.Points = (YesSum + NoSum);

    //    }
    //    return View(ViewModel);
    //}

}
    
//}
