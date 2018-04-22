using System;
using System.Data.Entity;

namespace StateTemplateV5Beta.Models
{
    public class User
    {
        public string ID { get; set; }
        public string Organization { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string PassHash { get; set; }
        public string PassSalt { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUsed { get; set; }

        public User() {}
    }

    public class DBUContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}