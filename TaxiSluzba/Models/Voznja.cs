using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Voznja
    {
        public DateTime datum { get; set; }
        public Lokacija lokacija { get; set; }
        public Enumi.TipAuta tipAuta { get; set; }
        public Musterija musterija { get; set; }
        public Lokacija odrediste { get; set; }
        public Dispecer dispecer { get; set; }
        public Vozac vozac { get; set; }
        public string iznos { get; set; }
        public Komentar komentar { get; set; }
        public Enumi.StatusVoznje statusVoznje { get; set; }

        public Voznja() { }

        public Voznja(DateTime datum, Lokacija lokacija, Enumi.TipAuta tipAuta, Musterija musterija)
        {
            this.datum = datum;
            this.lokacija = lokacija;
            this.tipAuta = tipAuta;
            this.musterija = musterija;
        }
    }
}