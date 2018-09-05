using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Adresa
    {
        private string ulica;
        private string broj;
        private string grad;
        private string postanskiBroj;

        public Adresa() { }

        public Adresa(string ulica, string brojUlice, string grad, string postanskiBroj)
        {
            this.ulica = ulica;
            this.broj = brojUlice;
            this.grad = grad;
            this.postanskiBroj = postanskiBroj;
        }

        public string Ulica
        {
            get { return ulica; }
            set { ulica = value; }
        }

        public string Broj
        {
            get { return broj; }
            set { broj = value; }
        }

        public string Grad
        {
            get { return grad; }
            set { grad = value; }
        }

        public string PostanskiBroj
        {
            get { return postanskiBroj; }
            set { postanskiBroj = value; }
        }
    }
}