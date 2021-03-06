﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace StateTemplateV5Beta.Models
{
    public class Answer
    {
        
        public int AId { get; set; }
        public int QId { get; set; }
        public string UId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUsed { get; set; }

        public string programName { get; set; }           
        public Nullable<bool> Value { get; set; }

        public Answer()
        {

        }
        public Answer(Answer a)
        {
            AId = a.AId;
            QId = a.QId;
            UId = a.UId;
            Created = a.Created;
            programName = a.programName;
            Value = a.Value;
        }
    

    }
    public class DBAContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>().HasKey(c => new { c.UId, c.AId, c.QId });
            base.OnModelCreating(modelBuilder);
        }
    }
}