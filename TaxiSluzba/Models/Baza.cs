using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Baza
    {
        public static Dictionary<string, Korisnik> registrovaniKorisnici = new Dictionary<string, Korisnik>();
        public static Dictionary<string, Vozac> vozaci = new Dictionary<string, Vozac>();
        public static Dictionary<string, Vozac> slobodniVozaci = new Dictionary<string, Vozac>();
        public static Dictionary<string, Voznja> voznjeNaCekanju = new Dictionary<string, Voznja>(); // voznje koje se prikazuju dispeceru(kljuc je vreme porudzbine voznje)
        public static Dictionary<string, Voznja> neuspesneVoznje = new Dictionary<string, Voznja>(); //kljuc je vreme voznje
        public static Dictionary<string, Voznja> voznjeNepoznatihDisp = new Dictionary<string, Voznja>(); //kljuce je vreme voznje
        public static Dictionary<string, Voznja> sveVoznje = new Dictionary<string, Voznja>(); //sve moguce voznje u sistemu, kljuc je datum voznje

        //baza
        public static void UpisiVoznje()
        {
            string ret = "";

            if (File.Exists(($@"{HttpRuntime.AppDomainAppPath}/App_Data/SveVoznje.txt")))
            {
                StreamReader sr = new StreamReader($@"{HttpRuntime.AppDomainAppPath}/App_Data/SveVoznje.txt");
                int br = 0;
                foreach (Voznja v in sveVoznje.Values)
                {
                    if (br == 0)
                    {
                        br++;
                        ret += string.Format(v.datum.ToString() + "#" + v.dispecer.username + "#" + v.iznos + "#" + v.komentar.opis + "#" +
                                        v.komentar.ocenaVoznje.ToString() + "#" + v.komentar.datumVreme.ToString() + "#" + v.lokacija.Adress.Ulica + "#" +
                                        v.lokacija.Adress.Broj + "#" + v.lokacija.Adress.Grad + "#" + v.lokacija.Adress.PostanskiBroj + "#" +
                                        v.musterija.username + "#" + v.odrediste.Adress.Ulica + "#" +
                                        v.odrediste.Adress.Broj + "#" + v.odrediste.Adress.Grad + "#" + v.odrediste.Adress.PostanskiBroj + "#" +
                                        v.statusVoznje.ToString() + "#" + v.tipAuta.ToString() + "#" + v.vozac.username);
                    }
                    else
                    {
                        ret += string.Format("\n" + v.datum.ToString() + "#" + v.dispecer.username + "#" + v.iznos + "#" + v.komentar.opis + "#" +
                                            v.komentar.ocenaVoznje.ToString() + "#" + v.komentar.datumVreme.ToString() + "#" + v.lokacija.Adress.Ulica + "#" +
                                            v.lokacija.Adress.Broj + "#" + v.lokacija.Adress.Grad + "#" + v.lokacija.Adress.PostanskiBroj + "#" +
                                            v.musterija.username + "#" + v.odrediste.Adress.Ulica + "#" +
                                            v.odrediste.Adress.Broj + "#" + v.odrediste.Adress.Grad + "#" + v.odrediste.Adress.PostanskiBroj + "#" +
                                            v.statusVoznje.ToString() + "#" + v.tipAuta.ToString() + "#" + v.vozac.username);
                    }
                }

                sr.Close();

                StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/SveVoznje.txt");
                sw.Write(ret);
                sw.Close();
            }
            else
            {
                int br = 0;
                foreach (Voznja v in sveVoznje.Values)
                {
                    if (br == 0)
                    {
                        br++;
                        ret += string.Format(v.datum.ToString() + "#" + v.dispecer.username + "#" + v.iznos + "#" + v.komentar.opis + "#" +
                                        v.komentar.ocenaVoznje.ToString() + "#" + v.komentar.datumVreme.ToString() + "#" + v.lokacija.Adress.Ulica + "#" +
                                        v.lokacija.Adress.Broj + "#" + v.lokacija.Adress.Grad + "#" + v.lokacija.Adress.PostanskiBroj + "#" +
                                        v.musterija.username + "#" + v.odrediste.Adress.Ulica + "#" +
                                        v.odrediste.Adress.Broj + "#" + v.odrediste.Adress.Grad + "#" + v.odrediste.Adress.PostanskiBroj + "#" +
                                        v.statusVoznje.ToString() + "#" + v.tipAuta.ToString() + "#" + v.vozac.username);
                    }
                    else
                    {
                        ret += string.Format("\n" + v.datum.ToString() + "#" + v.dispecer.username + "#" + v.iznos + "#" + v.komentar.opis + "#" +
                                            v.komentar.ocenaVoznje.ToString() + "#" + v.komentar.datumVreme.ToString() + "#" + v.lokacija.Adress.Ulica + "#" +
                                            v.lokacija.Adress.Broj + "#" + v.lokacija.Adress.Grad + "#" + v.lokacija.Adress.PostanskiBroj + "#" +
                                            v.musterija.username + "#" + v.odrediste.Adress.Ulica + "#" +
                                            v.odrediste.Adress.Broj + "#" + v.odrediste.Adress.Grad + "#" + v.odrediste.Adress.PostanskiBroj + "#" +
                                            v.statusVoznje.ToString() + "#" + v.tipAuta.ToString() + "#" + v.vozac.username);
                    }
                }

                StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/SveVoznje.txt");
                sw.Write(ret);
                sw.Close();
            }
        }

        public static void UpisiVozace()
        {
            string ret = "";

            if (File.Exists($@"{HttpRuntime.AppDomainAppPath}/App_Data/SviVozaci.txt"))
            {
                StreamReader sr = new StreamReader($@"{HttpRuntime.AppDomainAppPath}/App_Data/SviVozaci.txt");
                int br = 0;
                foreach (Vozac v in vozaci.Values)
                {
                    if (br == 0)
                    {
                        br++;
                        ret += string.Format(v.username + "#" + v.password + "#" + v.ime + "#" + v.prezime + "#" + v.pol.ToString() + "#" +
                                          v.email + "#" + v.jmbg + "#" + v.telefon + "#" + v.auto.registarskiBroj + ":" + v.auto.godisteAuta + ":" +
                                          v.auto.registracija + ":" + v.auto.tipAuta.ToString() + "#" +
                                          v.lokacija.Adress.Ulica + ":" + v.lokacija.Adress.Broj + ":" + v.lokacija.Adress.Grad + ":" + v.lokacija.Adress.PostanskiBroj + "#" +
                                          v.uloga.ToString());
                    }
                    else
                    {
                        ret += string.Format("\n" + v.username + "#" + v.password + "#" + v.ime + "#" + v.prezime + "#" + v.pol.ToString() + "#" +
                                          v.email + "#" + v.jmbg + "#" + v.telefon + "#" + v.auto.registarskiBroj + ":" + v.auto.godisteAuta + ":" +
                                          v.auto.registracija + ":" + v.auto.tipAuta.ToString() + "#" +
                                          v.lokacija.Adress.Ulica + ":" + v.lokacija.Adress.Broj + ":" + v.lokacija.Adress.Grad + ":" + v.lokacija.Adress.PostanskiBroj + "#" +
                                          v.uloga.ToString());
                    }
                }

                sr.Close();

                StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/SviVozaci.txt");
                sw.Write(ret);
                sw.Close();
            }
            else
            {
                if (vozaci.Count != 0)
                {
                    int br = 0;
                    foreach (Vozac v in vozaci.Values)
                    {
                        if (br == 0)
                        {
                            br++;
                            ret += string.Format(v.username + "#" + v.password + "#" + v.ime + "#" + v.prezime + "#" + v.pol.ToString() + "#" +
                                            v.email + "#" + v.jmbg + "#" + v.telefon + "#" + v.auto.registarskiBroj + ":" + v.auto.godisteAuta + ":" +
                                            v.auto.registracija + ":" + v.auto.tipAuta.ToString() + "#" +
                                            v.lokacija.Adress.Ulica + ":" + v.lokacija.Adress.Broj + ":" + v.lokacija.Adress.Grad + ":" + v.lokacija.Adress.PostanskiBroj + "#" +
                                            v.uloga.ToString());
                        }
                        else
                        {
                            ret += string.Format("\n" + v.username + "#" + v.password + "#" + v.ime + "#" + v.prezime + "#" + v.pol.ToString() + "#" +
                                            v.email + "#" + v.jmbg + "#" + v.telefon + "#" + v.auto.registarskiBroj + ":" + v.auto.godisteAuta + ":" +
                                            v.auto.registracija + ":" + v.auto.tipAuta.ToString() + "#" +
                                            v.lokacija.Adress.Ulica + ":" + v.lokacija.Adress.Broj + ":" + v.lokacija.Adress.Grad + ":" + v.lokacija.Adress.PostanskiBroj + "#" +
                                            v.uloga.ToString());
                        }
                    }

                    StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/SviVozaci.txt");
                    sw.WriteLine(ret);
                    sw.Close();
                }
            }
        }

        public static void UpisiRegKorisnike()
        {
            string ret = "";

            if (File.Exists($@"{HttpRuntime.AppDomainAppPath}/App_Data/RegistrovaniKorisnici.txt"))
            {
                StreamReader sr = new StreamReader($@"{HttpRuntime.AppDomainAppPath}/App_Data/RegistrovaniKorisnici.txt");
                int br = 0;
                foreach (Korisnik k in registrovaniKorisnici.Values)
                {
                    if (br == 0)
                    {
                        br++;
                        if (k.uloga != Enumi.Uloga.DISPECER)
                            ret += string.Format(k.username + "#" + k.password + "#" + k.ime + "#" + k.prezime + "#" + k.email + "#" + k.jmbg + "#" + k.pol.ToString() + "#" + k.telefon + "#" + k.uloga.ToString());

                    }
                    else
                    {
                        if (k.uloga != Enumi.Uloga.DISPECER)
                            ret += string.Format("\n" + k.username + "#" + k.password + "#" + k.ime + "#" + k.prezime + "#" + k.email + "#" + k.jmbg + "#" + k.pol.ToString() + "#" + k.telefon + "#" + k.uloga.ToString());
                    }
                }

                sr.Close();

                StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/RegistrovaniKorisnici.txt");
                sw.WriteLine(ret);
                sw.Close();
            }
            else
            {
                int br = 0;
                foreach (Korisnik k in registrovaniKorisnici.Values)
                {
                    if (br == 0)
                    {
                        br++;
                        if (k.uloga != Enumi.Uloga.DISPECER)
                            ret += string.Format(k.username + "#" + k.password + "#" + k.ime + "#" + k.prezime + "#" + k.email + "#" + k.jmbg + "#" + k.pol.ToString() + "#" + k.telefon + "#" + k.uloga.ToString());
                    }
                    else
                    {
                        if (k.uloga != Enumi.Uloga.DISPECER)
                            ret += string.Format("\n" + k.username + "#" + k.password + "#" + k.ime + "#" + k.prezime + "#" + k.email + "#" + k.jmbg + "#" + k.pol.ToString() + "#" + k.telefon + "#" + k.uloga.ToString());
                    }
                }

                StreamWriter sw = new StreamWriter($@"{HttpRuntime.AppDomainAppPath}/App_Data/RegistrovaniKorisnici.txt");
                sw.WriteLine(ret);
                sw.Close();
            }
        }
    }
}