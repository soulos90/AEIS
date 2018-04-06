﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace StateTemplateV5Beta.Models
{
    public class Answer
    {
        [Key]
        public int AId { get; set; }
        public string UId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUsed { get; set; }
        public string programName { get; set; }
        public string q1 { get; set; }
       
        public Answer()
        {

        }

    }

    public class Answer2
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime Created { get; set; }
        public string programName { get; set; }
        public Question2 Question { get; set; }
        public User2 User { get; set; }
        public bool Value { get; set; }

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