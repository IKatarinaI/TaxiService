using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Korisnik
    {
        public string username { get; set; }
        public string password { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Enumi.Pol pol { get; set; }
        public string jmbg { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public Enumi.Uloga uloga { get; set; }
        public List<Voznja> voznje { get; set; }

        public Korisnik() { voznje = new List<Voznja>(); }

        public Korisnik(string username, string password, string ime, string prezime, Enumi.Pol pol, string jmbg, string telefon, string email)
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
            this.voznje = new List<Voznja>();
        }
    }
}