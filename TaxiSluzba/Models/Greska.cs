using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Greska
    {
        public string tekst { get; set; }

        public Greska() { }

        public Greska(string tekst)
        {
            this.tekst = tekst;
        }
    }
}