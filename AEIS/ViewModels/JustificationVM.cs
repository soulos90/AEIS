using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StateTemplateV5Beta.Models;

namespace StateTemplateV5Beta.ViewModels
{
    public class JustificationVM : VMP
    {
        #region Properties
        public string[] SectionTitles { get; }
        public string SystemName { get; }
        public int[] SectionScores { get; }
        public int ScoreTotal { get; }
        #endregion

        #region Constructor
        public JustificationVM(string uId, string aId, Security active) : base(active)
        {
            Inventory inventory = new Inventory(uId, aId);
            SectionTitles = inventory.SectionTitles;
            SystemName = inventory.Systems[0].Name;
            SectionScores = inventory.Systems[0].SectionScores;
            ScoreTotal = inventory.Systems[0].ScoreTotal;
        }
        #endregion
    }
}