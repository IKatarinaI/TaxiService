using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaxiSluzba.Models;

namespace TaxiSluzba
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string pom = "";
            string[] res;
            Enumi.Pol gender;

            

            if (File.Exists($"{HttpRuntime.AppDomainAppPath}\\App_Data\\Dispeceri.txt"))
            {
                StreamReader sr = new StreamReader($"{HttpRuntime.AppDomainAppPath }\\App_Data\\Dispeceri.txt");
                while ((pom = sr.ReadLine()) != null)
                {
                    res = pom.Split('_');

                    if (res[4] == "MUSKI")
                        gender = Enumi.Pol.MUSKI;
                    else
                        gender = Enumi.Pol.ZENSKI;

                    Dispecer d = new Dispecer(res[0], res[1], res[2], res[3], gender, res[5], res[6], res[7]);
                    Baza.registrovaniKorisnici.Add(res[0], d);
                }

                sr.Close();

                UcitajBazu();
            }
        }

        public void UcitajBazu()
        {
            string pom = "";
            string[] ret;

            //ucitavanje korisnika
            if (File.Exists($"{HttpRuntime.AppDomainAppPath}\\App_Data\\RegistrovaniKorisnici.txt"))
            {
                StreamReader srKorisnici = new StreamReader($"{HttpRuntime.AppDomainAppPath}\\App_Data\\RegistrovaniKorisnici.txt");
                int br = 0;
                while ((pom = srKorisnici.ReadLine()) != null)
                {
                    br++;
                    ret = pom.Split('#');
                    if (ret.Length==1)
                    {
                        break;
                    }
                    Enumi.Pol gender;

                    if (ret[6] == "MUSKI")
                        gender = Enumi.Pol.MUSKI;
                    else
                        gender = Enumi.Pol.ZENSKI;

                    Enumi.Uloga uloga;
                    if (ret[8] == "MUSTERIJA")
                        uloga = Enumi.Uloga.MUSTERIJA;
                    else if (ret[8] == "VOZAC")
                        uloga = Enumi.Uloga.VOZAC;
                    else
                        uloga = Enumi.Uloga.DISPECER;

                    Korisnik k = new Korisnik(ret[0], ret[1], ret[2], ret[3], gender, ret[5], ret[7], ret[4]);
                    k.uloga = uloga;

                    if (!Baza.registrovaniKorisnici.ContainsKey(ret[0]))
                        Baza.registrovaniKorisnici.Add(k.username, k);
                }
                srKorisnici.Close();   
            }

            //ucitavanje vozaca
            if (File.Exists($"{HttpRuntime.AppDomainAppPath}\\App_Data\\SviVozaci.txt"))
            {
                StreamReader srVozaci = new StreamReader($"{HttpRuntime.AppDomainAppPath}\\App_Data\\SviVozaci.txt");
                while ((pom = srVozaci.ReadLine()) != null)
                {
                    ret = pom.Split('#');
                    if (ret.Length == 1)
                    {
                        break;
                    }
                    Enumi.Pol gender;
                    string[] auto;
                    string[] lokacija;

                    if (ret[4] == "MUSKI")
                        gender = Enumi.Pol.MUSKI;
                    else
                        gender = Enumi.Pol.ZENSKI;

                    Korisnik k = new Korisnik(ret[0], ret[1], ret[2], ret[3], gender, ret[6], ret[7], ret[5]);
                    k.uloga = Enumi.Uloga.VOZAC;
                    auto = ret[8].Split(':');
                    Enumi.TipAuta tipAuta;
                    if (auto[3] == "KOMBI")
                        tipAuta = Enumi.TipAuta.KOMBI;
                    else
                        tipAuta = Enumi.TipAuta.PUTNICKI_AUTOMOBIL;
                    Auto a = new Auto(null, auto[1], auto[2], auto[0], tipAuta);
                    lokacija = ret[9].Split(':');
                    Lokacija l = new Lokacija(1, 1, new Adresa(lokacija[0], lokacija[1], lokacija[2], lokacija[3]));
                    Vozac v = new Vozac(k, l, a);
                    a.vozac = v;

                    if (!Baza.vozaci.ContainsKey(v.username))
                        Baza.vozaci.Add(v.username, v);
                }
                srVozaci.Close();
            }

            //ucitavanje voznji
            if (File.Exists($"{HttpRuntime.AppDomainAppPath}\\App_Data\\SveVoznje.txt"))
            {
                StreamReader srKorisnici = new StreamReader($"{HttpRuntime.AppDomainAppPath}\\App_Data\\SveVoznje.txt");
                while ((pom = srKorisnici.ReadLine()) != null)
                {
                    ret = pom.Split('#');
                    if (ret.Length == 1 )
                    {
                        break;
                    }
                    DateTime dtVoznje = DateTime.Parse(ret[0]);

                    Enumi.OcenaVoznje ov;
                    if (ret[4] == "JEDAN")
                        ov = Enumi.OcenaVoznje.JEDAN;
                    else if (ret[4] == "DVA")
                        ov = Enumi.OcenaVoznje.DVA;
                    else if (ret[4] == "TRI")
                        ov = Enumi.OcenaVoznje.TRI;
                    else if (ret[4] == "CETIRI")
                        ov = Enumi.OcenaVoznje.CETIRI;
                    else if (ret[4] == "PET")
                        ov = Enumi.OcenaVoznje.PET;
                    else
                        ov = Enumi.OcenaVoznje.NULA;

                    DateTime dtKomentara = DateTime.Parse(ret[5]);

                    Enumi.StatusVoznje sv;
                    if (ret[15] == "KREIRANA_NA_CEKANJU")
                        sv = Enumi.StatusVoznje.KREIRANA_NA_CEKANJU;
                    else if (ret[15] == "FORMIRANA")
                        sv = Enumi.StatusVoznje.FORMIRANA;
                    else if (ret[15] == "OBRADJENA")
                        sv = Enumi.StatusVoznje.OBRADJENA;
                    else if (ret[15] == "PRIHVACENA")
                        sv = Enumi.StatusVoznje.PRIHVACENA;
                    else if (ret[15] == "U_TOKU")
                        sv = Enumi.StatusVoznje.U_TOKU;
                    else if (ret[15] == "OTKAZANA")
                        sv = Enumi.StatusVoznje.OTKAZANA;
                    else if (ret[15] == "NEUSPESNA")
                        sv = Enumi.StatusVoznje.NEUSPESNA;
                    else
                        sv = Enumi.StatusVoznje.USPESNA;

                    Enumi.TipAuta ta;
                    if (ret[16] == "KOMBI")
                        ta = Enumi.TipAuta.KOMBI;
                    else
                        ta = Enumi.TipAuta.PUTNICKI_AUTOMOBIL;

                    Lokacija l = new Lokacija(1, 1, new Adresa(ret[6], ret[7], ret[8], ret[9]));
                    Musterija m = new Musterija("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                    string musterija = ret[10];
                    if (ret[10] != "-")
                    {
                        Korisnik kor = Baza.registrovaniKorisnici[musterija];
                        m = new Musterija(kor.username, kor.password, kor.ime, kor.prezime, kor.pol, kor.jmbg, kor.telefon, kor.email);
                    }
                    else
                    {
                        m = new Musterija("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                    }
                    //vidi sta treba dodati musteriji da bude sve finoo, gde god cackam database obrati paznju
                    Voznja v = new Voznja(dtVoznje, l, ta, m);
                    v.odrediste = new Lokacija(1, 1, new Adresa(ret[11], ret[12], ret[13], ret[14]));
                    if (ret[1] != "-")
                        v.dispecer = (Dispecer)Baza.registrovaniKorisnici[ret[1]];
                    else
                        v.dispecer = new Dispecer("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                    //obrati paznju na to kad su musterija ili disp nepoznati
                    if (ret[17] != "-")
                        v.vozac = Baza.vozaci[ret[17]];
                    else
                    {
                        Lokacija lok = new Lokacija(1, 1, new Adresa("-", "-", "-", "-"));
                        v.vozac = new Vozac(new Korisnik("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-"), lok, new Auto(null, "-", "-", "-", Enumi.TipAuta.KOMBI));
                    }
                    v.iznos = ret[2];
                    if (musterija == "-")
                    {
                        v.komentar = new Komentar(ret[3], dtKomentara, m , null, ov);

                    }
                    else
                    {
                        v.komentar = new Komentar(ret[3], dtKomentara, Baza.registrovaniKorisnici[musterija], null, ov);
                    }
                    //dodaj voznju kad odradis u kontruktor komentara!!!! ***
                    v.statusVoznje = sv;
                    v.komentar.voznja = v;

                    Baza.sveVoznje.Add(v.datum.ToString(), v);

                    if (Baza.registrovaniKorisnici.ContainsKey(musterija))
                        Baza.registrovaniKorisnici[musterija].voznje.Add(v);

                    if (Baza.vozaci.ContainsKey(ret[17]))
                        Baza.vozaci[ret[17]].voznje.Add(v);
                }
                srKorisnici.Close();
            }
        }
    }
}
