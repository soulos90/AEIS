using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StateTemplateV5Beta.Models
{
    public class QuestionViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Survey Name")]
        public string Name { get; set; }
        public string Id { get; set; }
        [Display(Name = "Survey Answer")]
        public string Answer { get; set; }
    }
    public class SurveyQuestionViewModel
    {

        [Display(Name = "Survey Question")]
        [Required]
        public string Question { get; set; }
        //[Display(Name = "Survey?")]
        public bool Answer { get; set; }
        public int CurrentID { get; set; }
        public int Key { get; set; }
        public string ProgramName { get; set; }
        
    }
}