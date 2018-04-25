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
            IEnumerable<Answer> answers = dBAContext.Answers.SqlQuery("SELECT * FROM Answers WHERE AId='" + aId + "' AND UId='" + uId + "';");
            Environment e = new Environment();
            SectionScores = new int[Environment.NumSec];

            foreach (Answer a in answers)
            {
                Name = a.programName;
                int sectionNum = e.GetQuestionSection(a.QId);

                if (a.Value == true)
                    SectionScores[sectionNum] += e.GetQuestionYV(a.QId - 1);
                else if (a.Value == false)
                    SectionScores[sectionNum] += e.GetQuestionNV(a.QId - 1);
            }

            int scoreDivisor = 9;
            for (int i = 0; i < SectionScores.Length; i++)
            {
                SectionScores[i] /= scoreDivisor;
            }
            
        }
    }
}