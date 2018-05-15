using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Controllers;

namespace StateTemplateV5Beta.Models
{
    public class SurveyQuestion
    {
        public string QuestionText { get; set; }
        public int AId { get; set; }
        public int QId { get; set; }
        public string ProgramName { get; set; }
        public Nullable<bool> Value { get; set; }
        public int Percent { get; set; }
        public int NumberofQuestions { get; set; }
        public Security Active { get; set; }

        public SurveyQuestion(int qId)
        {

        }
    }
}