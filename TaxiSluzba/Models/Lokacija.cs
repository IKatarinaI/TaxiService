using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Lokacija
    {
        private double xKoord;
        private double yKoord;
        private Adresa adress;

        public Lokacija() { }

        public Lokacija(double xKoord, double yKoord, Adresa adresa)
        {
            this.xKoord = xKoord;
            this.yKoord = yKoord;
            this.adress = adresa;
        }

        public double XKoord
        {
            get { return xKoord; }
            set { xKoord = value; }
        }
        public double YKoord
        {
            get { return yKoord; }
            set { yKoord = value; }
        }
        public Adresa Adress
        {
            get { return adress; }
            set { adress = value; }
        }
    }
}