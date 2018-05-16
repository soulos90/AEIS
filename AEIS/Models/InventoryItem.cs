using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StateTemplateV5Beta.Models
{
    public class InventoryItem
    {
        public string Name { get; }
        public int AId { get; }
        public int[] SectionScores { get; }
        public int ScoreTotal { get { return SectionScores.Sum(); } }
        public DateTime LastUsed { get; }
        public bool HasUnanswered { get; }

        public InventoryItem(string uId, string aId)
        {
            DBAContext dBAContext = new DBAContext();
            Environment e = new Environment();
            SectionScores = new int[Environment.NumSec];
            int iaId = int.Parse(aId);

            for (int i = 1; i <= Environment.NumQus; i++)
            {
                Answer a = (from t in dBAContext.Answers where ((uId == t.UId) & (i == t.QId) & (iaId == t.AId)) select t).FirstOrDefault();

                if (a == null)
                {
                    if (e.GetQuestionRO(i - 1) == "0" || e.GetQuestionRO(i - 1) == null)
                        HasUnanswered = true;
                    else
                    {
                        int ro = int.Parse(e.GetQuestionRO(i - 1));
                        Answer aReliesOn = (from t in dBAContext.Answers where ((uId == t.UId) & (ro == t.QId) & (iaId == t.AId)) select t).FirstOrDefault();
                        
                        if (aReliesOn == null || aReliesOn.Value != e.GetQuestionROV(ro - 1))
                            HasUnanswered = true;
                    }
                }
                else
                {
                    if (a.programName != null)
                        Name = a.programName.Trim();
                    else
                        Name = "";

                    AId = a.AId;
                    int sectionNum = e.GetQuestionSection(a.QId);

                    if (a.Value == true)
                        SectionScores[sectionNum] += e.GetQuestionYV(a.QId - 1);
                    else if (a.Value == false)
                        SectionScores[sectionNum] += e.GetQuestionNV(a.QId - 1);

                    if (a.LastUsed > LastUsed)
                        LastUsed = a.LastUsed;
                }
            }

            int scoreDivisor = 9;
            for (int i = 0; i < SectionScores.Length; i++)
            {
                SectionScores[i] /= scoreDivisor;
            }
        }
    }
}