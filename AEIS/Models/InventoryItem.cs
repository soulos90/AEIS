using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StateTemplateV5Beta.Models
{
    public class InventoryItem
    {
        public string Name { get; }
        public int[] SectionScores { get; }
        public int ScoreTotal { get { return SectionScores.Sum(); } }
        public bool HasUnanswered { get; }
        
        public InventoryItem(string uId, string aId)
        {
            DBAContext dBAContext = new DBAContext();
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT * FROM Answers WHERE AId='" + aId + "' AND UId='" + uId + "';");
            Environment e = new Environment();
            SectionScores = new int[Environment.NumSec];

            foreach (Answer a in answers)
            {
                Name = a.programName.Trim();
                int sectionNum = e.GetQuestionSection(a.QId);

                if (a.Value == true)
                    SectionScores[sectionNum] += e.GetQuestionYV(a.QId - 1);
                else if (a.Value == false)
                    SectionScores[sectionNum] += e.GetQuestionNV(a.QId - 1);
                else if (e.GetQuestionRO(a.QId - 1) == "0" || e.GetQuestionRO(a.QId - 1) == null)
                    HasUnanswered = true;
            }

            if (answers.Count() < Environment.NumQus)
                HasUnanswered = true;

            int scoreDivisor = 9;
            for (int i = 0; i < SectionScores.Length; i++)
            {
                SectionScores[i] /= scoreDivisor;
            }
            
        }
    }
}