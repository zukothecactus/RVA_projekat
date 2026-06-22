using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Komponenta1.Helpers;
using Komponenta1.Interfaces;
using Komponenta1.Model;
using Komponenta1.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;


namespace Komponenta1.ViewModel
{
    //spajamo sve slojeve lol
    public class GlavniViewModel: BazniModel
    {
        public static GlavniViewModel Instanca { get; private set; }
        // servisi i menadzeri 
        private readonly IServisPerzistencije _servisPerzistencije;
        private readonly MenadzerKomandi _menadzerKomandi;
        private readonly ObavestivacGrafikona _obavestivacGrafikona;
        private MenadzerGrafikona _menadzerGrafikona = new MenadzerGrafikona();



        // kolekcije koje sadrže sve podatke -----------------------------------------------
        public ObservableCollection<Konferencija> SveKonferencije { get; set; }
        public ObservableCollection<KonferencijskaStatistika> SveStatistike { get; set; }



        // Prikazne kolekcije koje reaguju na pretragu --------------------------------------
        private ObservableCollection<Konferencija> _prikazaneKonferencije;
        public ObservableCollection<Konferencija> PrikazaneKonferencije
        {
            get => _prikazaneKonferencije;
            set { _prikazaneKonferencije = value; OnPropertyChanged(); }
        }

        //Za dodavanje novih konferencija
        private Konferencija _novaKonferencija;
        public Konferencija NovaKonferencija
        {
            get => _novaKonferencija;
            set { _novaKonferencija = value; OnPropertyChanged(); }
        }

        //Za dodavanje novih statistika
        private KonferencijskaStatistika _novaStatistika;
        public KonferencijskaStatistika NovaStatistika
        {
            get => _novaStatistika;
            set { _novaStatistika = value; OnPropertyChanged(); }
        }

        public ICommand KomandaDodajStatistiku { get; }

        // svojstva za pretragu
        private string _tekstPretrageKonferencija = string.Empty;
        public string TekstPretrageKonferencija
        {
            get => _tekstPretrageKonferencija;
            set
            {
                _tekstPretrageKonferencija = value;
                OnPropertyChanged();
                FiltrirajKonferencije();
            }
        }

        private ObservableCollection<KonferencijskaStatistika> _prikazaneStatistike;
        public ObservableCollection<KonferencijskaStatistika> PrikazaneStatistike
        {
            get => _prikazaneStatistike;
            set { _prikazaneStatistike = value; OnPropertyChanged(); }
        }

        private string _tekstPretrageStatistika = string.Empty;
        public string TekstPretrageStatistika
        {
            get => _tekstPretrageStatistika;
            set
            {
                _tekstPretrageStatistika = value;
                OnPropertyChanged();
                FiltrirajStatistike();
            }
        }

        

        //GRAFIKON-------------------------------------------------------------------------
        // Kolekcija serija za grafikon
        private ISeries[] _serijeGrafikona;
        public ISeries[] SerijeGrafikona
        {
            get => _serijeGrafikona;
            set { _serijeGrafikona = value; OnPropertyChanged(); }
        }

        // Pozivamo ovu metodu svaki put kada se promeni stanje neke statistike ili se doda/obriše
        public void OsveziGrafikon()
        {
            _obavestivacGrafikona.ObavestiSve(SveStatistike);

            // Kreiramo kružni grafikon (Pie Chart) na osnovu preračunatih vrednosti iz Observera
            SerijeGrafikona = new ISeries[]
            {
        new PieSeries<int> { Values = new int[] { _menadzerGrafikona.BrojOtvorenihPrijava }, Name = "Otvorena prijava" },
        new PieSeries<int> { Values = new int[] { _menadzerGrafikona.BrojVelikoInteresovanje }, Name = "Veliko interesovanje" },
        new PieSeries<int> { Values = new int[] { _menadzerGrafikona.BrojUPripremi }, Name = "U pripremi" },
        new PieSeries<int> { Values = new int[] { _menadzerGrafikona.BrojOdrzanih }, Name = "Održana" }
            };
        }


        // WPF Komande (Binding za dugmice) ------------------------------------------------
        public ICommand KomandaPonisti { get; }
        public ICommand KomandaPonovi { get; }
        public ICommand KomandaDodajKonferenciju { get; }
        public ICommand KomandaObrisiKonferenciju { get; }
        public ICommand KomandaSacuvaj { get; }
        public ICommand KomandaSimulirajStanje { get; }
        public ICommand KomandaObrisiStatistiku { get; }

        public GlavniViewModel()
        {
            Instanca = this;
            // Inicijalizacija servisa
            _servisPerzistencije = new JsonServisPerzistencije();
            _menadzerKomandi = new MenadzerKomandi();
            _obavestivacGrafikona = new ObavestivacGrafikona();

            // Povezivanje Observera
            var loger = new LogerServis();
            _menadzerKomandi.Pretplati(loger); // Menadžer javlja logeru akcije
            _obavestivacGrafikona.Pretplati(_menadzerGrafikona); // Observer za grafikon prati promene u statistici

            NovaKonferencija = new Konferencija(); // inicijalizacija za dodavanje novih konferencija
            NovaStatistika = new KonferencijskaStatistika(); // inicijalizacija za dodavanje novih statistika

            // Učitavanje podataka
            PodaciAplikacije podaci = _servisPerzistencije.UcitajPodatke();
            SveKonferencije = new ObservableCollection<Konferencija>(podaci.Konferencije);
            SveStatistike = new ObservableCollection<KonferencijskaStatistika>(podaci.Statistike);

            PrikazaneKonferencije = new ObservableCollection<Konferencija>(SveKonferencije);
            PrikazaneStatistike = new ObservableCollection<KonferencijskaStatistika>(SveStatistike);

            // Inicijalizacija komandi
            KomandaPonisti = new RelejKomanda(p =>
            {
                _menadzerKomandi.Ponisti();

                OsveziKompletanUI(); // Osvežavamo UI nakon undo akcije
            });

            KomandaPonovi = new RelejKomanda(p =>
            {
                _menadzerKomandi.Ponovi();

                OsveziKompletanUI();
            });
            KomandaDodajKonferenciju = new RelejKomanda(IzvrsiDodavanjeKonferencije);
            KomandaObrisiKonferenciju = new RelejKomanda(IzvrsiBrisanjeKonferencije, p => p is Konferencija);
            KomandaSacuvaj = new RelejKomanda(p => SacuvajPodatke());
            KomandaSimulirajStanje = new RelejKomanda(IzvrsiSimulacijuStanja, p => p is KonferencijskaStatistika);
            KomandaDodajStatistiku = new RelejKomanda(IzvrsiDodavanjeStatistike);
            KomandaObrisiStatistiku = new RelejKomanda(IzvrsiBrisanjeStatistike, p => p is KonferencijskaStatistika);

            // inicijalno osvežavanje grafikona
            OsveziGrafikon();
        }

        // pretraga i filtriranje
        private void FiltrirajKonferencije()
        {
            if (string.IsNullOrWhiteSpace(TekstPretrageKonferencija))
            {
                PrikazaneKonferencije = new ObservableCollection<Konferencija>(SveKonferencije);
            }
            else
            {
                var pretraga = TekstPretrageKonferencija.ToLower();
                var rezultat = SveKonferencije.Where(k =>
                    k.Naziv.ToLower().Contains(pretraga) ||
                    k.Oblast.ToLower().Contains(pretraga) ||
                    k.Grad.ToLower().Contains(pretraga) ||
                    k.Id.ToString().Contains(pretraga)).ToList();

                PrikazaneKonferencije = new ObservableCollection<Konferencija>(rezultat);
            }
        }

        // logika akcija (dodavanje, brisanje, undo/redo)
        private void IzvrsiDodavanjeKonferencije(object parametar)
        {
            // 1. Provera validacije (zahtev iz specifikacije)
            // Ne dozvoljavamo dodavanje ako postoje greške ili su polja prazna
            if (string.IsNullOrWhiteSpace(NovaKonferencija.Naziv) ||
                string.IsNullOrWhiteSpace(NovaKonferencija.Oblast) ||
                string.IsNullOrWhiteSpace(NovaKonferencija.Grad) ||
                NovaKonferencija.HasErrors)
            {
                _menadzerKomandi.ObavestiSve("GREŠKA: Unos nije validan. Proverite polja.");
                return;
            }

            // 2. Prepisujemo podatke u novu instancu
            var konfZaDodavanje = new Konferencija
            {
                Naziv = NovaKonferencija.Naziv,
                Oblast = NovaKonferencija.Oblast,
                Grad = NovaKonferencija.Grad
            };

            // 3. Izvršavamo dodavanje kroz Command Pattern (omogućava Undo)
            var komanda = new KomandaDodaj<Konferencija>(SveKonferencije, konfZaDodavanje);
            _menadzerKomandi.IzvrsiKomandu(komanda, $"Dodata konferencija: {konfZaDodavanje.Naziv}");

            FiltrirajKonferencije(); // Osvežavamo tabelu

            // 4. Resetujemo formu (praznimo TextBox-ove na ekranu)
            NovaKonferencija = new Konferencija();
        }

        private void IzvrsiBrisanjeKonferencije(object parametar)
        {
            if (parametar is Konferencija konfZaBrisanje)
            {
                var komanda = new KomandaObrisi<Konferencija>(SveKonferencije, konfZaBrisanje, SveStatistike);
                _menadzerKomandi.IzvrsiKomandu(komanda, $"Obrisana konferencija: {konfZaBrisanje.Naziv}");
                OsveziKompletanUI();
            }
        }

        private void SacuvajPodatke()
        {
            var podaci = new PodaciAplikacije
            {
                Konferencije = SveKonferencije.ToList(),
                Statistike = SveStatistike.ToList()
            };
            _servisPerzistencije.SacuvajPodatke(podaci);
            _menadzerKomandi.ObavestiSve("Svi podaci su uspešno sačuvani u datoteku.");
        }

        private void IzvrsiSimulacijuStanja(object parametar)
        {
            if (parametar is KonferencijskaStatistika statistika)
            {
                // Više ne zovemo statistika.SimulirajPromenu() direktno, 
                // već to prepuštamo Command šablonu!
                var komanda = new KomandaSimuliraj(statistika);

                _menadzerKomandi.IzvrsiKomandu(komanda, $"Simulirano novo stanje za statistiku konferencije ID: {statistika.KonferencijaId}");

                // Osvežavamo prikaz
                FiltrirajStatistike();
                OsveziGrafikon();
            }
        }

        private void IzvrsiDodavanjeStatistike(object parametar)
        {
            // Provera validacije
            if (NovaStatistika.KonferencijaId == Guid.Empty || NovaStatistika.HasErrors)
            {
                _menadzerKomandi.ObavestiSve("GREŠKA: Unos statistike nije validan.");
                return;
            }

            var statZaDodavanje = new KonferencijskaStatistika
            {
                KonferencijaId = NovaStatistika.KonferencijaId,
                DatumOdrzavanja = NovaStatistika.DatumOdrzavanja,
                BrojRadova = NovaStatistika.BrojRadova,
                BrojSesija = NovaStatistika.BrojSesija
            };
            // Napomena: Stanje se automatski postavlja na "OtvorenaPrijava" kroz konstruktor metrike.

            var komanda = new KomandaDodaj<KonferencijskaStatistika>(SveStatistike, statZaDodavanje);
            _menadzerKomandi.IzvrsiKomandu(komanda, $"Dodata statistika za konferenciju ID: {statZaDodavanje.KonferencijaId}");

            OsveziKompletanUI();  

            // Resetujemo formu
            NovaStatistika = new KonferencijskaStatistika();
        }

        private void FiltrirajStatistike()
        {
            if (string.IsNullOrWhiteSpace(TekstPretrageStatistika))
            {
                PrikazaneStatistike = new ObservableCollection<KonferencijskaStatistika>(SveStatistike);
            }
            else
            {
                var pretraga = TekstPretrageStatistika.ToLower();
                var rezultat = SveStatistike.Where(s =>
                    s.DatumOdrzavanja.ToString("d").Contains(pretraga) ||
                    s.BrojRadova.ToString().Contains(pretraga) ||
                    s.BrojSesija.ToString().Contains(pretraga) ||
                    s.Status.ToString().ToLower().Contains(pretraga) ||
                    s.KonferencijaId.ToString().ToLower().Contains(pretraga)
                ).ToList();

                PrikazaneStatistike = new ObservableCollection<KonferencijskaStatistika>(rezultat);
            }
        }

        private void IzvrsiBrisanjeStatistike(object parametar)
        {
            if (parametar is KonferencijskaStatistika statZaBrisanje)
            {
                var komanda = new KomandaObrisi<KonferencijskaStatistika>(SveStatistike, statZaBrisanje, SveStatistike);
                _menadzerKomandi.IzvrsiKomandu(komanda, $"Obrisana statistika za konferenciju ID: {statZaBrisanje.KonferencijaId}");

                FiltrirajStatistike(); // Osveži prikaz
                OsveziGrafikon();      // Obavesti Observer da osveži LiveCharts!
            }
        }

        private void OsveziKompletanUI()
        {
            // Osvežavamo obe tabele filtriranjem
            FiltrirajKonferencije();
            FiltrirajStatistike();

            // Obaveštavamo Observer da osveži LiveCharts grafikon
            OsveziGrafikon();
        }
    }
}
