using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index"); 
        }

        [HttpPost]
        public ActionResult Registracija(string user, string pass, string ime, string prezime, string pol, string jmbg, string telefon, string email)
        {
            Enumi.Pol gender;

            if (pol == "ZENSKI")
                gender = Enumi.Pol.ZENSKI;
            else
                gender = Enumi.Pol.MUSKI;

            if(!Baza.registrovaniKorisnici.ContainsKey(user))
            {
                Korisnik k = new Korisnik(user, pass, ime, prezime, gender, jmbg, telefon, email);
                Baza.registrovaniKorisnici.Add(user, k);
                Baza.UpisiRegKorisnike();
                return View("UspesnaRegistracija");
            }
            else
            {
                Greska gr = new Greska("Operacija nije uspela!");
                return View("Greska", gr);
            }
        }

      [HttpPost]
        public ActionResult Logovanje(string user, string pass)
        {
            Korisnik k = (Korisnik)Session["korisnikUser"];

            if(k == null)
            {
                k = new Korisnik();
                Session["korisnikUser"] = k;
            }

            if(Baza.registrovaniKorisnici.ContainsKey(user))
            {
                if (Baza.registrovaniKorisnici[user].password == pass)
                {
                    if (Baza.registrovaniKorisnici[user].uloga == Enumi.Uloga.DISPECER)
                    {
                        Session["korisnikUser"] = Baza.registrovaniKorisnici[user];
                        return View("Dispecer");
                    }
                    else if (Baza.registrovaniKorisnici[user].uloga == Enumi.Uloga.VOZAC)
                    {
                        Adresa a = new Adresa("NULL", "NULL", "NULL", "NULL");
                        Lokacija l = new Lokacija(1, 1, a);
                        Baza.vozaci[user].lokacija = l;
                        Vozac v = Baza.vozaci[user];
                        Session["korisnikUser"] = Baza.registrovaniKorisnici[user];
                        return View("Vozac", v);
                    }
                    else
                    {
                        Korisnik m = new Musterija();
                        m = Baza.registrovaniKorisnici[user];
                        Session["korisnikUser"] = Baza.registrovaniKorisnici[user];
                        return View("Musterija", m);
                    }
                }
                else
                {
                    Greska gr = new Greska("Netacan password!");
                    return View("Greska", gr); //napraviti view za pogresan unos lozinke!!!!!
                }
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult Odjava()
        {
            Session.Abandon();
            Korisnik d = new Korisnik();
            Session["korisnik"] = d;
            return View("Index");
        }
        
        //izmena podataka kod musterije
        [HttpPost]
        public ActionResult IzmeniPodatke(string user, string pass, string ime, string prezime, string pol, string jmbg, string telefon, string email)
        {
            Korisnik k = (Korisnik)Session["korisnikUser"];

            if (k == null)
            {
                k = new Korisnik();
                Session["korisnikUser"] = k;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k.username && kor.uloga == Enumi.Uloga.MUSTERIJA)
                {
                    Baza.registrovaniKorisnici[user].password = pass;
                    Baza.registrovaniKorisnici[user].ime = ime;
                    Baza.registrovaniKorisnici[user].prezime = prezime;

                    Enumi.Pol gender;
                    if (pol == "MUSKI")
                        gender = Enumi.Pol.MUSKI;
                    else
                        gender = Enumi.Pol.ZENSKI;

                    Baza.registrovaniKorisnici[user].pol = gender;
                    Baza.registrovaniKorisnici[user].jmbg = jmbg;
                    Baza.registrovaniKorisnici[user].telefon = telefon;
                    Baza.registrovaniKorisnici[user].email = email;

                    Greska odg = new Greska("Podaci su uspesno izmenjeni.");
                    return View("Greska", odg);
                }

            }

            Greska o = new Greska("Podaci nisu izmenjeni, doslo je do greske!");
            return View("Greska"); //smisli gresku
        }

        [HttpPost]
        public ActionResult PoruciVoznju(string username, string ulica, string broj, string grad, string poBroj, string tipPrevoza)
        {
            Korisnik k = (Korisnik)Session["korisnikUser"];

            if (k == null)
            {
                k = new Korisnik();
                Session["korisnikUser"] = k;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k.username && kor.uloga == Enumi.Uloga.MUSTERIJA)
                {
                    Adresa a = new Adresa(ulica, broj, grad, poBroj);
                    Lokacija l = new Lokacija(1, 1, a);
                    Enumi.TipAuta tipAuto;
                    if (tipPrevoza == "KOMBI")
                        tipAuto = Enumi.TipAuta.KOMBI;
                    else
                        tipAuto = Enumi.TipAuta.PUTNICKI_AUTOMOBIL;

                    Musterija m = new Musterija(Baza.registrovaniKorisnici[username].username, Baza.registrovaniKorisnici[username].password, Baza.registrovaniKorisnici[username].ime, Baza.registrovaniKorisnici[username].prezime, Baza.registrovaniKorisnici[username].pol, Baza.registrovaniKorisnici[username].jmbg, Baza.registrovaniKorisnici[username].telefon, Baza.registrovaniKorisnici[username].email);
                    Voznja v = new Voznja(DateTime.Now, l, tipAuto, m);
                    v.statusVoznje = Enumi.StatusVoznje.KREIRANA_NA_CEKANJU;
                    v.komentar = new Komentar("-", DateTime.Now, Baza.registrovaniKorisnici[username], v, Enumi.OcenaVoznje.NULA);
                    //
                    Baza.registrovaniKorisnici[username].voznje.Add(v);
                    AzurirajVoznju(v, k.username);

                    Baza.UpisiVoznje();

                    Greska odg = new Greska("Voznja je uspesno porucena!");
                    return View("Greska", odg);
                }

            }

            Greska o = new Greska("Voznja nije narucena, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult KreirajVozaca(string user, string pass, string ime, string prezime, string pol, string jmbg, string telefon, string email, string godisteAuto, string regAuto, string brAuto, string tipAuto)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.DISPECER)
                {
                    Enumi.Pol gender;
                    if (pol == "MUSKI")
                        gender = Enumi.Pol.MUSKI;
                    else
                        gender = Enumi.Pol.ZENSKI;

                    Korisnik k = new Korisnik(user, pass, ime, prezime, gender, jmbg, telefon, email);
                    k.uloga = Enumi.Uloga.VOZAC;
                    Enumi.TipAuta tip;
                    if (tipAuto == "KOMBI")
                        tip = Enumi.TipAuta.KOMBI;
                    else
                        tip = Enumi.TipAuta.PUTNICKI_AUTOMOBIL;
                    Auto a = new Auto(null, godisteAuto, regAuto, brAuto, tip);
                    Vozac v = new Vozac(k, null, a);
                    a.vozac = v;
                    v.lokacija = new Lokacija(1, 1, new Adresa("-", "-", "-", "-"));

                    Baza.vozaci.Add(v.username, v);
                    Baza.registrovaniKorisnici.Add(k.username, k);
                    //Database.slobodniVozaci.Add(v.username, v);

                    Baza.UpisiVozace();
                    Baza.UpisiRegKorisnike();

                    Greska o = new Greska("Vozac uspesno kreiran.");
                    return View("Greska", o);
                }

            }

            Greska odg = new Greska("Vozac nije kreiran, doslo je do greske!");
            return View("Greska");
        }

        [HttpPost]
        public ActionResult KreirajVoznju(string ulica, string broj, string grad, string poBroj, string tipPrevoza, string izabraniVozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.DISPECER)
                {
                    Adresa a = new Adresa(ulica, broj, grad, poBroj);
                    Lokacija l = new Lokacija(1, 1, a);
                    Enumi.TipAuta tip;
                    if (tipPrevoza == "KOMBI")
                        tip = Enumi.TipAuta.KOMBI;
                    else
                        tip = Enumi.TipAuta.PUTNICKI_AUTOMOBIL;
                    Voznja v = new Voznja(DateTime.Now, l, tip, null);

                    v.dispecer = (Dispecer)Baza.registrovaniKorisnici[k123.username];
                    //
                    v.dispecer.voznje.Add(v);
                    
                    v.statusVoznje = Enumi.StatusVoznje.FORMIRANA;
                    v.musterija = new Musterija("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                    //
                    AzurirajVoznju(v, k123.username);
                    if (izabraniVozac != null)
                    {
                        v.vozac = Baza.vozaci[izabraniVozac];
                        Baza.vozaci[izabraniVozac].voznje.Add(v);
                        break;
                    }

                    Greska odg = new Greska("Voznja uspesno kreirana.");
                    return View("Greska", odg);
                }

            }

            Greska o = new Greska("Voznja nije kreirana, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult DodeliVozacaVoznji(string voznja, string vozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.DISPECER)
                {
                    if (voznja == null)
                        break;
                    Voznja retVoznja = Baza.sveVoznje[voznja];
                    retVoznja.statusVoznje = Enumi.StatusVoznje.OBRADJENA;
                    retVoznja.dispecer = (Dispecer)Baza.registrovaniKorisnici[k123.username];
                    Baza.registrovaniKorisnici[k123.username].voznje.Add(retVoznja);

                    Vozac retVozac = Baza.vozaci[vozac];
                    retVozac.voznje.Add(retVoznja);

                    if (vozac == null)
                    {
                        retVoznja.vozac = retVozac;
                    }
                    //
                    AzurirajVoznju(retVoznja, k123.username);

                    Greska odg = new Greska("Voznja uspesno kreirana.");
                    return View("Greska", odg);
                }

            }

            Greska o = new Greska("Voznja nije kreirana, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult DetaljiVoznje(string datumVoznje, string vozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username)
                {
                    if (kor.uloga == Enumi.Uloga.VOZAC || kor.uloga == Enumi.Uloga.DISPECER)
                    {
                        Voznja vo = new Voznja();
                        Korisnik k = new Korisnik("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                        Musterija m = new Musterija("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                        Adresa a = new Adresa("-", "-", "-", "-");
                        Lokacija l = new Lokacija(1, 1, a);

                        vo = Baza.sveVoznje[datumVoznje];

                        if (vo.dispecer == null)
                            vo.dispecer = new Dispecer("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");

                        if (vo.iznos == null)
                            vo.iznos = "-";

                        if (vo.komentar == null)
                            vo.komentar = new Komentar("-", DateTime.Now, k, vo, Enumi.OcenaVoznje.NULA);

                        if (vo.musterija == null)
                            vo.musterija = m;

                        if (vo.odrediste == null)
                            vo.odrediste = l;

                        //
                        AzurirajVoznju(vo, k123.username);
                        return View("DetaljiVoznje", vo);
                    }
                }

            }

            Greska o = new Greska("Nemoguce je otvoriti detalje voznje, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult PocniSaVoznjom(string datumVoznje, string vozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    Voznja vo = new Voznja();

                    foreach (Voznja v in Baza.vozaci[vozac].voznje)
                    {
                        if (v.datum.ToString() == datumVoznje)
                        {
                            Baza.vozaci[vozac].slobodan = false;
                            v.statusVoznje = Enumi.StatusVoznje.U_TOKU;
                            vo = v;
                            AzurirajVoznju(vo, k123.username);
                        }
                    }

                    return View("PocniSaVoznjom", vo);
                }

            }

            Greska o = new Greska("Nemoguce je zapoceti sa voznjom, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult UspesnaVoznja(string datumVoznje, string vozac, string musterija)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    Voznja vo = new Voznja();
                    bool ret = false;

                    if (musterija != "-")
                    {
                        ret = true;
                        foreach (Voznja v in Baza.registrovaniKorisnici[musterija].voznje)
                        {
                            if (datumVoznje == v.datum.ToString())
                            {
                                v.statusVoznje = Enumi.StatusVoznje.USPESNA;
                                vo = v;
                                AzurirajVoznju(vo, k123.username);
                            }
                        }
                    }

                    foreach (Voznja v in Baza.vozaci[vozac].voznje)
                    {
                        if (datumVoznje == v.datum.ToString())
                        {
                            if (v.dispecer == null)
                            {
                                Dispecer d = new Dispecer("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                                v.dispecer = d;
                                AzurirajVoznju(v, k123.username);
                            }
                            v.statusVoznje = Enumi.StatusVoznje.USPESNA;
                            if (!ret)
                            {
                                vo = v;
                                AzurirajVoznju(vo, k123.username);
                            }
                        }
                    }

                    return View("UspesnaVoznja", vo);
                }

            }

            Greska o = new Greska("Doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult NeuspesnaVoznja(string datumVoznje, string musterija)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    Voznja vo = new Voznja();

                    if (musterija != "-")
                    {
                        foreach (Voznja v in Baza.registrovaniKorisnici[musterija].voznje)
                        {
                            if (datumVoznje == v.datum.ToString())
                            {
                                vo = v;
                                AzurirajVoznju(vo, k123.username);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            foreach (KeyValuePair<string, Voznja> v in Baza.sveVoznje)
                            {
                                if (datumVoznje == v.Value.datum.ToString())
                                {
                                    vo = v.Value;
                                    AzurirajVoznju(vo, k123.username);
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }
                }
                    return View("KomentarVozac", vo);
                }

            }

            Greska o = new Greska("Nemoguce je ostaviti komentar, doslo je do greske!");
            return View("Greska");
        }

        [HttpPost]
        public ActionResult KomentarVozac(string comment, string datumVoznje, string userKorisnika)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    if (userKorisnika != "-")
                    {
                        foreach (Voznja v in Baza.registrovaniKorisnici[userKorisnika].voznje)
                        {
                            if (datumVoznje == v.datum.ToString())
                            {
                                Komentar k = new Komentar(comment, DateTime.Now, v.musterija, v, Enumi.OcenaVoznje.NULA);
                                v.komentar = k;
                                v.statusVoznje = Enumi.StatusVoznje.NEUSPESNA;
                                AzurirajVoznju(v, k123.username);
                            }
                        }
                    }
                    else
                    {
                        foreach (Voznja v in Baza.neuspesneVoznje.Values)
                        {
                            if (datumVoznje == v.datum.ToString())
                            {
                                Komentar k = new Komentar(comment, DateTime.Now, v.musterija, v, Enumi.OcenaVoznje.NULA);
                                v.komentar = k;
                                v.statusVoznje = Enumi.StatusVoznje.NEUSPESNA;
                                AzurirajVoznju(v, k123.username);
                                Baza.slobodniVozaci.Add(v.vozac.username, v.vozac);
                            }
                        }
                    }

                    Baza.UpisiVoznje();

                    return View("Vozac");
                }

            }

            Greska o = new Greska("Nemoguce postaviti komentar, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult ZavrsiVoznju(string ulica, string broj, string grad, string poBroj, string iznos, string datumVoznje, string musterija, string vozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    if (musterija != "-")
                    {
                        foreach (Voznja v in Baza.registrovaniKorisnici[musterija].voznje)
                        {
                            if (datumVoznje == v.datum.ToString())
                            {
                                Adresa a = new Adresa(ulica, broj, grad, poBroj);
                                Lokacija l = new Lokacija(1, 1, a);
                                v.odrediste = l;
                                v.iznos = iznos;
                                if (v.dispecer.username == "-")
                                    Baza.voznjeNepoznatihDisp.Add(v.datum.ToString(), v);
                                //Database.vozaci[vozac].voznje.Add(v);
                                //dodaj voznju vozacu u registrovanim korisnicima!!!!!
                                AzurirajVoznju(v, k123.username);
                            }
                        }
                    }

                    foreach (Voznja v in Baza.vozaci[vozac].voznje)
                    {
                        if (datumVoznje == v.datum.ToString())
                        {
                            Adresa a = new Adresa(ulica, broj, grad, poBroj);
                            Lokacija l = new Lokacija(1, 1, a);
                            v.odrediste = l;
                            v.iznos = iznos;
                            AzurirajVoznju(v, k123.username);
                        }
                    }

                    Baza.UpisiVoznje();
                }
            }
            return View("Vozac");
        }

        [HttpPost]
        public ActionResult LokacijaTaksiste(string vozac, string ulica, string broj, string grad, string poBroj)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    Adresa a = new Adresa(ulica, broj, grad, poBroj);
                    Lokacija l = new Lokacija(1, 1, a);
                    Baza.vozaci[vozac].lokacija = l;

                    if (Baza.slobodniVozaci.ContainsKey(vozac))
                        Baza.slobodniVozaci[vozac].lokacija = l;

                    Greska odg = new Greska("Lokacija uspesno dodata.");
                    return View("Greska", odg);
                }
            }

            Greska o = new Greska("Nemoguce dodati lokaciju, doslo je do greske!");
            return View("Greska", o);
        }

        [HttpPost]
        public ActionResult PreuzimanjeVoznje(string voznja, string vozac)
        {
            Korisnik k123 = (Korisnik)Session["korisnikUser"];

            if (k123 == null)
            {
                k123 = new Korisnik();
                Session["korisnikUser"] = k123;
            }

            foreach (Korisnik kor in Baza.registrovaniKorisnici.Values)
            {
                if (kor.username == k123.username && kor.uloga == Enumi.Uloga.VOZAC)
                {
                    Voznja voz = new Voznja();

                    foreach (Korisnik k in Baza.registrovaniKorisnici.Values)
                    {
                        if (k.uloga == Enumi.Uloga.MUSTERIJA)
                        {
                            foreach (Voznja vo in k.voznje)
                            {
                                if (vo.datum.ToString() == voznja)
                                {
                                    if (vo.statusVoznje == Enumi.StatusVoznje.KREIRANA_NA_CEKANJU || vo.statusVoznje == Enumi.StatusVoznje.FORMIRANA)
                                    {
                                        vo.statusVoznje = Enumi.StatusVoznje.PRIHVACENA;
                                        voz = vo;
                                        voz.vozac = Baza.vozaci[vozac];
                                        Baza.voznjeNaCekanju.Remove(vo.datum.ToString());
                                        Baza.vozaci[vozac].voznje.Add(voz);
                                        AzurirajVoznju(voz, k123.username);
                                    }
                                }
                            }
                        }
                    }

                    Baza.slobodniVozaci.Remove(vozac);

                    return View("PocniSaVoznjom", voz);
                }

            }

            Greska o = new Greska("Nemoguce preuzeti voznju, doslo je do greske");
            return View("Greska");
        }

        public void AzurirajVoznju(Voznja voznja, string korisnik)
        {
            Voznja retVoznja = new Voznja();
            retVoznja = PopuniPolja(voznja);

            if (Baza.sveVoznje.ContainsKey(voznja.datum.ToString()))
                Baza.sveVoznje[voznja.datum.ToString()] = retVoznja;
            else
                Baza.sveVoznje.Add(voznja.datum.ToString(), retVoznja);
        }

        public Voznja PopuniPolja(Voznja v)
        {
            if (v.dispecer == null)
                v.dispecer = new Dispecer("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");

            if (v.musterija == null)
                v.musterija = new Musterija("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");

            if (v.odrediste == null)
                v.odrediste = new Lokacija(1, 1, new Adresa("-", "-", "-", "-"));

            if (v.vozac == null)
            {
                Korisnik k = new Korisnik("-", "-", "-", "-", Enumi.Pol.MUSKI, "-", "-", "-");
                Lokacija l = new Lokacija(1, 1, new Adresa("-", "-", "-", "-"));
                Auto a = new Auto(new Vozac(), "-", "-", "-", Enumi.TipAuta.PUTNICKI_AUTOMOBIL);
                v.vozac = new Vozac(k, l, a);
            }

            if (v.komentar == null)
                v.komentar = new Komentar("-", DateTime.Now, v.musterija, v, Enumi.OcenaVoznje.NULA);

            if (v.iznos == null)
                v.iznos = "-1";

            return v;
        }
    }
}