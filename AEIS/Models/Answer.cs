using System;
using System.Data.Entity;

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
        public Answer()
        {

        }

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