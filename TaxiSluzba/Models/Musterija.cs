using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Musterija : Korisnik
    {
        public Musterija() { voznje = new List<Voznja>(); }

        public Musterija(string username, string password, string ime, string prezime, Enumi.Pol pol, string jmbg, string telefon, string email)
        {
            this.username = username;
            this.password = password;
            this.ime = ime;
            this.prezime = prezime;
            this.pol = pol;
            this.jmbg = jmbg;
            this.telefon = telefon;
            this.email = email;
            this.uloga = Enumi.Uloga.MUSTERIJA;
            voznje = new List<Voznja>();
        }
    }
}