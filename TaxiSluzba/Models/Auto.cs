using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Auto
    {
        private object p;
        private string v1;
        private string v2;
        private string v3;

        public Vozac vozac { get; set; }
        public string godisteAuta { get; set; }
        public string registracija { get; set; }
        public string registarskiBroj { get; set; }
        public Enumi.TipAuta tipAuta { get; set; }

        public Auto() { }

        public Auto(Vozac vozac, string godisteAutomobila, string registracija, string brojTaksija, Enumi.TipAuta tipAutomobila)
        {
            this.vozac = vozac;
            this.godisteAuta = godisteAuta;
            this.registracija = registracija;
            this.registarskiBroj = registarskiBroj;
            this.tipAuta = tipAuta;
        }

        public Auto(object p, string v1, string v2, string v3, Enumi.TipAuta tipAuta)
        {
            this.p = p;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.tipAuta = tipAuta;
        }
    }
}