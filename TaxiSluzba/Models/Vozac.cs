using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Vozac : Korisnik
    {
        public Lokacija lokacija { get; set; }
        public Auto auto { get; set; }
        public bool slobodan { get; set; }

        public Vozac() { voznje = new List<Voznja>(); }

        public Vozac(Korisnik k, Lokacija lokacija, Auto auto)
        {
            this.username = k.username;
            this.password = k.password;
            this.ime = k.ime;
            this.prezime = k.prezime;
            this.pol = k.pol;
            this.jmbg = k.jmbg;
            this.telefon = k.telefon;
            this.email = k.email;
            this.uloga = Enumi.Uloga.VOZAC;
            this.lokacija = lokacija;
            this.auto = auto;
            voznje = new List<Voznja>();
            slobodan = true;
        }
    }
}