using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StateTemplateV5Beta.Models
{


    public class Question2
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public Question2 reliesOn { get; set; }
        public int yesPointValue { get; set; }
        public int noPointValue { get; set; }
    }
    public class DBQContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Question2> Questions2 { get; set; }
    }

    public class SurveyContext : DbContext
    {

        public DbSet<Question2> Q2 {get;set;}
        public DbSet<Answer2> A2 { get; set; }
        public DbSet<User2> U2 { get; set; }

    }
}