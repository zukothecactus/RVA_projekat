using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Komponenta2.Helpers;
using Komponenta2.Models;
using Komponenta2.Services.Interfaces;
using Komponenta2.Strategije;
using Ugovori;

namespace Komponenta2.ViewModels
{
    /// <summary>
    /// Glavni ViewModel Komponente 2 — spaja izvor podataka, adapter i statističke
    /// strategije, i izlaže ih korisničkom interfejsu.
    /// </summary>
    public class GlavniViewModel : BazniModel
    {
        private readonly IStatistikaServis _izvor;
        private readonly IStatistikaAdapter _adapter;
        private readonly ICsvServis _csvServis;
        private readonly string _opisIzvora;
        private readonly Dictionary<string, List<KonkretnaStatistika>> _struktura = new();

        private DateTime _od;
        private DateTime _do;
        private PrikazPeriodaModel? _izabraniPeriod;
        private IStatistickaMetoda? _izabranaMetoda;
        private string _statusPoruka = "";

        public GlavniViewModel(
            IStatistikaServis izvor,
            IStatistikaAdapter adapter,
            IEnumerable<IStatistickaMetoda> metode,
            ICsvServis csvServis,
            string opisIzvora)
        {
            _izvor = izvor;
            _adapter = adapter;
            _csvServis = csvServis;
            _opisIzvora = opisIzvora;

            PrikazaniPeriodi = new ObservableCollection<PrikazPeriodaModel>();
            DostupneMetode = new ObservableCollection<IStatistickaMetoda>();
            Rezultati = new ObservableCollection<StatistickiRezultat>();

            KomandaPreuzmi = new RelejKomanda(_ => Preuzmi(), _ => Od.Date <= Do.Date);
            KomandaObradi = new RelejKomanda(_ => Obradi(), _ => IzabraniPeriod != null && IzabranaMetoda != null);
            KomandaIzveziCsv = new RelejKomanda(_ => IzveziCsv(), _ => Rezultati.Count > 0);

            foreach (var metoda in metode)
            {
                DostupneMetode.Add(metoda);
            }

            _od = new DateTime(2026, 5, 25);
            _do = new DateTime(2026, 5, 28);

            Preuzmi();
        }

        public DateTime Od
        {
            get => _od;
            set
            {
                if (PostaviVrednost(ref _od, value))
                {
                    NaPromenuPerioda();
                }
            }
        }

        public DateTime Do
        {
            get => _do;
            set
            {
                if (PostaviVrednost(ref _do, value))
                {
                    NaPromenuPerioda();
                }
            }
        }

        public ObservableCollection<PrikazPeriodaModel> PrikazaniPeriodi { get; }

        public PrikazPeriodaModel? IzabraniPeriod
        {
            get => _izabraniPeriod;
            set => PostaviVrednost(ref _izabraniPeriod, value);
        }

        public ObservableCollection<IStatistickaMetoda> DostupneMetode { get; }

        public IStatistickaMetoda? IzabranaMetoda
        {
            get => _izabranaMetoda;
            set => PostaviVrednost(ref _izabranaMetoda, value);
        }

        public ObservableCollection<StatistickiRezultat> Rezultati { get; }

        public string StatusPoruka
        {
            get => _statusPoruka;
            set => PostaviVrednost(ref _statusPoruka, value);
        }

        public ICommand KomandaPreuzmi { get; }

        public ICommand KomandaObradi { get; }

        public ICommand KomandaIzveziCsv { get; }

        // Revalidira period nakon promene datuma: obaveštava o neispravnom rasponu i
        // tera ponovnu proveru CanExecute za sve komande (npr. "Preuzmi").
        private void NaPromenuPerioda()
        {
            if (Od.Date > Do.Date)
            {
                StatusPoruka = "Neispravan period: 'Od' ne sme biti posle 'Do'.";
            }

            CommandManager.InvalidateRequerySuggested();
        }

        private void Preuzmi()
        {
            try
            {
                var lista = _izvor.PreuzmiStatistikePoPeriodu(Od, Do);
                _adapter.DodajUStrukturu(_struktura, lista, Od, Do);
                OsveziPrikaz();
                StatusPoruka = $"Izvor: {_opisIzvora}. Učitano perioda: {_struktura.Count}.";
            }
            catch (Exception ex)
            {
                StatusPoruka = $"Greška pri preuzimanju: {ex.Message}";
            }
        }

        private void OsveziPrikaz()
        {
            PrikazaniPeriodi.Clear();
            foreach (var par in _struktura)
            {
                string kljuc = par.Key;
                string od = kljuc.Substring(0, 10);   // ključ je fiksni "yyyy-MM-dd-yyyy-MM-dd"
                string doD = kljuc.Substring(11);

                var konf = par.Value
                    .GroupBy(s => s.NazivKonferencije)
                    .Select(g => new PrikazKonferencijeModel
                    {
                        Linija = $"{g.Key} -> {string.Join(", ", g.Select(s => s.Trojka))}"
                    })
                    .ToList();

                PrikazaniPeriodi.Add(new PrikazPeriodaModel
                {
                    Kljuc = kljuc,
                    ZaglavljePerioda = $"({od}, {doD})",
                    Konferencije = konf
                });
            }
        }

        private void Obradi()
        {
            if (IzabraniPeriod == null || IzabranaMetoda == null)
            {
                return;
            }

            var lista = _struktura[IzabraniPeriod.Kljuc];
            var rez = IzabranaMetoda.Izracunaj(lista, IzabraniPeriod.ZaglavljePerioda);

            Rezultati.Clear();
            foreach (var r in rez)
            {
                Rezultati.Add(r);
            }
        }

        private void IzveziCsv()
        {
            var dijalog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV datoteka (*.csv)|*.csv",
                FileName = "rezultati_statistike.csv"
            };

            if (dijalog.ShowDialog() == true)
            {
                _csvServis.Izvezi(dijalog.FileName, Rezultati);
                StatusPoruka = $"Izvezeno u: {dijalog.FileName}";
            }
        }
    }
}
