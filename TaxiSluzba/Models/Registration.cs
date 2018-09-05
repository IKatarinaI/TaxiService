using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Registration
    {
        public string username { get; set; }
        public string password { get; set; }

        public Registration() { }

        public Registration(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}