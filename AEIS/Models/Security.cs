using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StateTemplateV5Beta.Models
{
    public class Security
    {
        public string ID { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool Remember { get; set; }
        public Security()
        {
            IsLoggedIn = false;
            ID = null;
            Remember = false;
        }
        public Security(string id, bool log, bool rem)
        {
            ID = id;
            Remember = rem;
            IsLoggedIn = log;
        }
    }
}