using System;
using System.Data.Entity;
using StateTemplateV5Beta.Controllers;

namespace StateTemplateV5Beta.Models
{
    public class Answer
    {
        public int AId { get; set; }
        public string UId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUsed { get; set; }
        public string programName { get; set; }
        public string q1 { get; set; }
        public string q2 { get; set; }
        public string q3 { get; set; }
        public string q4 { get; set; }
        public string q5 { get; set; }
        public string q6 { get; set; }
        public string q7 { get; set; }
        public string q8 { get; set; }
        public string q9 { get; set; }
        public string q10 { get; set; }

        public Answer() {}

        #region Methods
        // uses answers from questions 1-4
        public int GetPublicServicesScore()
        {
            int publicServicesScore = 0;
            QuestionsController qController = new QuestionsController();
            Question q = qController.GetQ(1);

            if (q1 == "YES")
                publicServicesScore += q.yesVal;
            else if (q1 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(2);
            if (q2 == "YES")
                publicServicesScore += q.yesVal;
            else if (q2 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(3);
            if (q3 == "YES")
                publicServicesScore += q.yesVal;
            else if (q3 == "NO")
                publicServicesScore += q.noVal;

            q = qController.GetQ(4);
            if (q4 == "YES")
                publicServicesScore += q.yesVal;
            else if (q4 == "NO")
                publicServicesScore += q.noVal;

            return (publicServicesScore * 100) / qController.MAX_SCORE;
        }

        // uses answers from questions 5-6
        public int GetInternalServicesScore()
        {
            int internalServicesScore = 0;
            QuestionsController qController = new QuestionsController();
            Question q = qController.GetQ(1);

            if (q5 == "YES")
                internalServicesScore += q.yesVal;
            else if (q5 == "NO")
                internalServicesScore += q.noVal;

            q = qController.GetQ(6);
            if (q6 == "YES")
                internalServicesScore += q.yesVal;
            else if (q6 == "NO")
                internalServicesScore += q.noVal;

            return (internalServicesScore * 100) / qController.MAX_SCORE;
        }

        //uses answers from questions 7-8 (7.1-7.2 on Handout)
        public int GetAccessibilityScore()
        {
            int accessibilityScore = 0;
            QuestionsController qController = new QuestionsController();
            Question q = qController.GetQ(7);

            if (q7 == "YES")
            {
                q = qController.GetQ(8);
                if (q8 == "YES")
                    accessibilityScore += q.yesVal;
                else if (q8 == "NO")
                    accessibilityScore += q.noVal;
            }
            else if (q7 == "NO")
                accessibilityScore += q.noVal;

            return (accessibilityScore * 100) / qController.MAX_SCORE;
        }

        //uses answers from questions 9-10 (8-9 on Handout) 
        public int GetLifeSpanScore()
        {
            int lifeSpanScore = 0;
            QuestionsController qController = new QuestionsController();
            Question q = qController.GetQ(9);

            if (q9 == "YES")
                lifeSpanScore += q.yesVal;
            else if (q9 == "NO")
                lifeSpanScore += q.noVal;

            q = qController.GetQ(10);
            if (q10 == "YES")
                lifeSpanScore += q.yesVal;
            else if (q10 == "NO")
                lifeSpanScore += q.noVal;

            return (lifeSpanScore * 100) / qController.MAX_SCORE;
        }

        public int GetTotalScore()
        {
            return GetPublicServicesScore() + GetInternalServicesScore() +
                GetAccessibilityScore() + GetLifeSpanScore();
        }
        #endregion

    }
    public class DBAContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>().HasKey(c => new { c.UId, c.AId });
            base.OnModelCreating(modelBuilder);
        }
    }
}