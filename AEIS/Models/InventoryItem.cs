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
        
        public InventoryItem(string uId, string aId)
        {
            DBAContext dBAContext = new DBAContext();
            Environment e = new Environment();
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT * FROM Answers WHERE AId='" + aId + "' AND UId='" + uId + "';");
            SectionScores = new int[Environment.NumSec];

            foreach (Answer a in answers)
            {
                Name = a.programName;
                int sectionNum = e.GetQuestionSection(a.QId);
                int scoreDivisor = 9;

                if (a.Value == true)
                    SectionScores[sectionNum] += e.GetQuestionYV(a.QId - 1) / scoreDivisor;
                else if (a.Value == false)
                    SectionScores[sectionNum] += e.GetQuestionNV(a.QId - 1) / scoreDivisor;
            }
        }
    }
}