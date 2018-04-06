using StateTemplateV5Beta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StateTemplateV5Beta.Controllers
{

    public class SurveyController : Controller
    {
        private DBQContext db = new DBQContext();

        // GET: Default
        public ActionResult Index()
        {
            //using (var context = new SurveyContext())
            //{
            //    var q1 = new Question2() { Text = "My first question" };
            //    var q2 = new Question2() { Text = "My second question", reliesOn = q1 };
            //    context.Q2.Add(q1);
            //    context.Q2.Add(q2);
            //    context.SaveChanges();

            //    var a1 = new Answer2() { Question = q1, Value = true , Created = DateTime.Now};
            //    var a2 = new Answer2() { Question = q2, Value = false, Created = DateTime.Now };
            //    context.A2.Add(a1);
            //    context.A2.Add(a2);
            //    context.SaveChanges();

            //}
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

               
                //Question question = null;

                Question2 question = null;
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                viewModel.ProgramName = model.Name;
                using (var context = new SurveyContext())
                {
                    
                    question = (from s in context.Q2 where s.Id == 1 select s).FirstOrDefault();
                   

                    if (question != null)
                    {

                        viewModel.Question = question.Text;
                        viewModel.CurrentID = question.Id;

                    }

                }
                // TODO get first question
                return RedirectToAction("SurveQuestions", viewModel);
            }
            
        }

        public ActionResult SurveQuestions(SurveyQuestionViewModel model)
        {
               
            return View(model);
        }
        [HttpPost]
        public ActionResult AnswerQuestion(SurveyQuestionViewModel model)
        {
               
                if (!ModelState.IsValid)
            {
                return View("SurveQuestions", model);
            }
            else
            {
                
                SurveyQuestionViewModel viewModel = new SurveyQuestionViewModel();
                viewModel.ProgramName = model.ProgramName;
                using (var context = new SurveyContext())
                {
                    Question2 previousQuestion = (from s in context.Q2 where (model.CurrentID == s.Id) select s).FirstOrDefault();
                    Answer2 PreviousAnswer = new Answer2();
                    PreviousAnswer.Question = previousQuestion;
                    PreviousAnswer.Value = model.Answer;
                    PreviousAnswer.Created = DateTime.Now;
                    PreviousAnswer.programName = model.ProgramName;
                    context.A2.Add(PreviousAnswer);
                    context.SaveChanges();

                    Question2 nextQuestion = (from s in context.Q2 where (model.CurrentID < s.Id)  select s).FirstOrDefault();

                    if (nextQuestion != null)
                    {
                        viewModel.Question = nextQuestion.Text;
                        viewModel.CurrentID = nextQuestion.Id;

                    }
                    else
                    {
                       return RedirectToAction("Summary", "Survey", viewModel);
                    }

                }
                // TODO get next question.  if no more questions then thank you view
                return RedirectToAction("SurveQuestions", viewModel);
            }
        }
    
        public ActionResult Summary(SurveyQuestionViewModel model)
        {
            SummaryViewModel ViewModel = new SummaryViewModel();
            ViewModel.ProgramName = model.ProgramName;    

            using (var context = new SurveyContext())
            {
                int YesSum = (from SS in context.A2 where (SS.programName == model.ProgramName && SS.Value == true) select SS.Question.yesPointValue).Sum();
                int NoSum = (from SS in context.A2 where (SS.programName == model.ProgramName && SS.Value == false) select SS.Question.noPointValue).Sum();
                ViewModel.Points = (YesSum + NoSum);

            }
                return View(ViewModel);
        }

    }
    
}
