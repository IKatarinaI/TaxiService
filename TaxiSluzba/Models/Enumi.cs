using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Enumi
    {
        public enum TipAuta
        {
            PUTNICKI_AUTOMOBIL,
            KOMBI
        }
        public enum StatusVoznje
        {
            KREIRANA_NA_CEKANJU,
            FORMIRANA,
            OBRADJENA,
            PRIHVACENA,
            U_TOKU,
            OTKAZANA,
            NEUSPESNA,
            USPESNA
        }
        public enum OcenaVoznje
        {
            NULA,
            JEDAN,
            DVA,
            TRI,
            CETIRI,
            PET
        }
        public enum Uloga
        {
            DISPECER,
            MUSTERIJA,
            VOZAC
        }
        public enum Pol
        {
            MUSKI,
            ZENSKI
        }
    }
}