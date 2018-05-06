using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class QuestionVM : IVM
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Survey Name")]
        public string Name { get; set; }
        public string Id { get; set; }
        //[Display(Name = "Survey Answer")]
        public string Answer { get; set; }
        public Security Active { get; set; }

        public QuestionVM()
        { }
        public QuestionVM(Security active)
        {
            Active = active;
        }
    }

    public class SurveyQuestionVM : IVM
    {
        [Display(Name = "Survey Question")]
        [Required]
        public string QuestionText { get; set; }
        public int AId { get; set; }
        public int QId { get; set; }
        //[Display(Name = "Survey?")]
        public string ProgramName { get; set; }
        public Nullable<bool> Value { get; set; }
        public int Percent { get; set; }
        public int NumberofQuestions { get; set; }
        public Security Active { get; set; }
        public SurveyQuestionVM()
        {

        }
        public SurveyQuestionVM(Security active)
        {
            Active = active;
        }
    }
}