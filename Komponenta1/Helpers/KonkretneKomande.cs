using Komponenta1.Interfaces;
using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    // Komanda za dodavanje novog entiteta
    public class KomandaDodaj<T> : IIzvrsnaKomanda
    {
        private ObservableCollection<T> _kolekcija;
        private T _stavka;

        public KomandaDodaj(ObservableCollection<T> kolekcija, T stavka)
        {
            _kolekcija = kolekcija;
            _stavka = stavka;
        }

        public void Izvrsi()
        {
            _kolekcija.Add(_stavka);
        }

        public void Ponisti()
        {
            _kolekcija.Remove(_stavka);
        }
    }

    // Komanda za brisanje postojećeg entiteta
    public class KomandaObrisi<T> : IIzvrsnaKomanda
    {
        private ObservableCollection<T> _kolekcija;
        private T _stavka;
        private int _indeks;

        //treba nam za kaskadno brisanje
        private List<KonferencijskaStatistika> _obrisaneStatistike;
        private ObservableCollection<KonferencijskaStatistika> _kolekcijaStatistika;

        public KomandaObrisi(ObservableCollection<T> kolekcija, T stavka, ObservableCollection<KonferencijskaStatistika> kolekcijaStatistika)
        {
            _kolekcija = kolekcija;
            _stavka = stavka;
            _kolekcijaStatistika = kolekcijaStatistika;
        }

        public void Izvrsi()
        {
            _indeks = _kolekcija.IndexOf(_stavka);
            if (_indeks >= 0)
            {
                // Ako brišemo konferenciju, nađi i obriši njene statistike
                if (_stavka is Konferencija konf && _kolekcijaStatistika != null)
                {
                    _obrisaneStatistike = _kolekcijaStatistika.Where(s => s.KonferencijaId == konf.Id).ToList();
                    foreach (var stat in _obrisaneStatistike)
                    {
                        _kolekcijaStatistika.Remove(stat);
                    }
                }
                _kolekcija.Remove(_stavka);
            }
        }

        public void Ponisti()
        {
            if (_indeks >= 0)
            {
                _kolekcija.Insert(_indeks, _stavka);

                // Vraćanje obrisanih statistika
                if (_obrisaneStatistike != null)
                {
                    foreach (var stat in _obrisaneStatistike)
                    {
                        _kolekcijaStatistika.Add(stat);
                    }
                }
            }
        }
    }

    // Komanda za izmenu postojećeg entiteta
    public class KomandaIzmeni<T> : IIzvrsnaKomanda
    {
        private ObservableCollection<T> _kolekcija;
        private T _staraStavka;
        private T _novaStavka;
        private int _indeks;

        public KomandaIzmeni(ObservableCollection<T> kolekcija, T staraStavka, T novaStavka)
        {
            _kolekcija = kolekcija;
            _staraStavka = staraStavka;
            _novaStavka = novaStavka;
        }

        public void Izvrsi()
        {
            _indeks = _kolekcija.IndexOf(_staraStavka);
            if (_indeks >= 0)
            {
                // Menjamo stari objekat novim na istom indeksu
                _kolekcija[_indeks] = _novaStavka;
            }
        }

        public void Ponisti()
        {
            if (_indeks >= 0)
            {
                // Vraćamo stari objekat
                _kolekcija[_indeks] = _staraStavka;
            }
        }
    }

    // Komanda specifična za State pattern
    public class KomandaSimuliraj : IIzvrsnaKomanda
    {
        private KonferencijskaStatistika _statistika;
        private StatusKonferencije _staroStanje;
        private StatusKonferencije _novoStanje;
        private bool _prvoIzvrsavanje = true;

        public KomandaSimuliraj(KonferencijskaStatistika statistika)
        {
            _statistika = statistika;
            // Pamtimo stanje pre simulacije kako bismo mogli da uradimo Undo
            _staroStanje = statistika.Status;
        }

        public void Izvrsi()
        {
            if (_prvoIzvrsavanje)
            {
                // Prvi put pozivamo originalnu logiku State šablona
                _statistika.SimulirajPromenu();
                _novoStanje = _statistika.Status; // Pamtimo u šta je prešlo zbog Redo akcije
                _prvoIzvrsavanje = false;
            }
            else
            {
                // Ako je Redo, ručno i bezbedno vraćamo novo stanje
                VratiStanje(_novoStanje);
            }
        }

        public void Ponisti()
        {
            // Ako je Undo, ručno i bezbedno vraćamo staro stanje
            VratiStanje(_staroStanje);
        }

        // Pomoćna metoda koja osigurava da State Pattern i UI budu ažurirani
        private void VratiStanje(StatusKonferencije status)
        {
            IStanjeKonferencije stanjeZaPostavljanje;

            switch (status)
            {
                case StatusKonferencije.OtvorenaPrijava:
                    stanjeZaPostavljanje = new StanjeOtvorenaPrijava();
                    break;
                case StatusKonferencije.VelikoInteresovanje:
                    stanjeZaPostavljanje = new StanjeVelikoInteresovanje();
                    break;
                case StatusKonferencije.UPripremi:
                    stanjeZaPostavljanje = new StanjeUPripremi();
                    break;
                case StatusKonferencije.Odrzana:
                    stanjeZaPostavljanje = new StanjeOdrzana();
                    break;
                default:
                    stanjeZaPostavljanje = new StanjeOtvorenaPrijava();
                    break;
            }

            // Metoda PostaviStanje iz tvog Modela provereno okida osvežavanje (ObavestiOPromeniSvojstva)
            _statistika.PostaviStanje(stanjeZaPostavljanje);
        }
    }
}
