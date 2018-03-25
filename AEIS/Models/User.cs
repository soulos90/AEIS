using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StateTemplateV5Beta.Models
{
    public class User
    {

        public string ID { get; set; }
        public string Orginization { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Passhash { get; set; }
        public string Cookie { get; set; }
        public DateTime created { get; set; }
        public DateTime LastUsed { get; set; }
    }
    public class DBUContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}