using Komponenta1.Interfaces;
using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Komponenta1.Services
{
    public class JsonServisPerzistencije : IServisPerzistencije
    {
        private readonly string _putanjaDoDatoteke = "podaci_konferencije.json";
        private readonly JsonSerializerOptions _opcije = new JsonSerializerOptions { WriteIndented = true };
        public void SacuvajPodatke(PodaciAplikacije podaci)
        {
            try
            {
                string jsonSadrzaj = JsonSerializer.Serialize(podaci, _opcije);

                //ako ne postoji datoteka, kreiramo je
                File.WriteAllText(_putanjaDoDatoteke, jsonSadrzaj);
            }
            catch (Exception greska)
            {
                throw new Exception($"Greška prilikom čuvanja podataka: {greska.Message}");
            }
        }

        public PodaciAplikacije UcitajPodatke()
        {
            // Ako datoteka ne postoji ili je prazna, generišemo početne podatke (Seed data)
            if (!File.Exists(_putanjaDoDatoteke) || new FileInfo(_putanjaDoDatoteke).Length == 0)
            {
                PodaciAplikacije pocetniPodaci = GenerisiPocetnePodatke();
                SacuvajPodatke(pocetniPodaci);
                return pocetniPodaci;
            }

            try
            {
                string jsonSadrzaj = File.ReadAllText(_putanjaDoDatoteke);
                PodaciAplikacije ucitaniPodaci = JsonSerializer.Deserialize<PodaciAplikacije>(jsonSadrzaj, _opcije);
                return ucitaniPodaci ?? GenerisiPocetnePodatke();
            }
            catch (Exception)
            {
                // U slučaju bilo kakve greške pri čitanju, vraćamo bezbedan inicijalni set podataka
                return GenerisiPocetnePodatke();
            }
        }

        public PodaciAplikacije GenerisiPocetnePodatke()
        {
            var podaci = new PodaciAplikacije();

            // Kreiranje 3 instance klase subjekta (Konferencija)
            var k1 = new Konferencija { Naziv = "IT_konferencija", Oblast = "Informacione tehnologije", Grad = "Beograd" };
            var k2 = new Konferencija { Naziv = "CS_konferencija", Oblast = "Racunarske nauke", Grad = "Kopaonik" };
            var k3 = new Konferencija { Naziv = "Telekom_konferencija", Oblast = "Telekomunikacije", Grad = "Beograd" };

            podaci.Konferencije.Add(k1);
            podaci.Konferencije.Add(k2);
            podaci.Konferencije.Add(k3);

            // Kreiranje 3 instance klase metrike (KonferencijskaStatistika) povezane sa konferencijama
            var s1 = new KonferencijskaStatistika
            {
                KonferencijaId = k1.Id,
                DatumOdrzavanja = DateTime.Now.AddMonths(2),
                BrojRadova = 45,
                BrojSesija = 8
            };

            // Postavićemo različita početna stanja kroz simulaciju radi raznolikosti
            s1.PostaviStanje(new StanjeOtvorenaPrijava());

            var s2 = new KonferencijskaStatistika
            {
                KonferencijaId = k2.Id,
                DatumOdrzavanja = DateTime.Now.AddMonths(1),
                BrojRadova = 60,
                BrojSesija = 12
            };
            s2.PostaviStanje(new StanjeVelikoInteresovanje());

            var s3 = new KonferencijskaStatistika
            {
                KonferencijaId = k3.Id,
                DatumOdrzavanja = DateTime.Now.AddMonths(4),
                BrojRadova = 30,
                BrojSesija = 6
            };
            s3.PostaviStanje(new StanjeUPripremi());

            podaci.Statistike.Add(s1);
            podaci.Statistike.Add(s2);
            podaci.Statistike.Add(s3);

            return podaci;
        }
    }
}
