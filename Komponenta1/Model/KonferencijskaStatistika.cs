using Komponenta1.Interfaces;
using Komponenta1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Komponenta1.Model
{
    public class KonferencijskaStatistika: ValidacioniModel
    {
        private Guid _konferencijaId;
        private DateTime _datumOdrzavanja;
        private int _brojRadova;
        private int _brojSesija;

        // Klasa čuva referencu na trenutno stanje objekta (State Pattern)
        private IStanjeKonferencije _trenutnoStanje;


        public Guid KonferencijaId
        {
            get => _konferencijaId;
            set
            {
                _konferencijaId = value;
                if (_konferencijaId == Guid.Empty)
                    DodajGresku(nameof(KonferencijaId), "Morate izabrati konferenciju.");
                else
                    UkloniGreske(nameof(KonferencijaId));
                OnPropertyChanged();
            }
        }

        public DateTime DatumOdrzavanja
        {
            get => _datumOdrzavanja;
            set { 
                _datumOdrzavanja = value; 
                if (_datumOdrzavanja < DateTime.Now)
                    DodajGresku(nameof(DatumOdrzavanja), "Datum održavanja ne može biti u prošlosti.");
                else
                    UkloniGreske(nameof(DatumOdrzavanja));

                OnPropertyChanged(); 
            }
        }

        public int BrojRadova
        {
            get => _brojRadova;
            set { 
                _brojRadova = value; 
                if (_brojRadova <= 0)
                    DodajGresku(nameof(BrojRadova), "Broj radova ne može biti negativan i mora postojati bar 1 rad.");
                else
                    UkloniGreske(nameof(BrojRadova));
                OnPropertyChanged(); 
            }
        }

        public int BrojSesija
        {
            get => _brojSesija;
            set { 
                _brojSesija = value; 
                if (_brojSesija <= 0)
                    DodajGresku(nameof(BrojSesija), "Broj sesija ne može biti negativan i mora postojati bar 1 sesija.");
                else
                    UkloniGreske(nameof(BrojSesija));
                OnPropertyChanged(); 
            }
        }

        //Status se dinamicki cita iz trenutnog stanja, pa kada se promeni stanje, wpf ce osveziti podatak
        //[JsonInclude]
        public StatusKonferencije Status
        {
            get => _trenutnoStanje.TrenutniStatus;
            set => OnPropertyChanged(nameof(Status));
        }

        public KonferencijskaStatistika()
        {
            // Podrazumevano stanje pri kreiranju metrike
            _trenutnoStanje = new StanjeOtvorenaPrijava();
            DatumOdrzavanja = DateTime.Now;
        }

        [JsonConstructor]
        public KonferencijskaStatistika(Guid konferencijaId, DateTime datumOdrzavanja, int brojRadova, int brojSesija, StatusKonferencije status)
        {
            // Postavljamo osnovne vrednosti
            _konferencijaId = konferencijaId;
            _datumOdrzavanja = datumOdrzavanja;
            _brojRadova = brojRadova;
            _brojSesija = brojSesija;

            // Umesto da koristimo prazan konstruktor koji postavlja na 0,
            // odmah postavljamo ispravno stanje na osnovu učitanog enuma
            PostaviStanjeNaOsnovuStatusa(status);
        }

        // Metoda koju pozivaju konkretna stanja da bi ažurirala kontekst
        public void PostaviStanje(IStanjeKonferencije novoStanje)
        {
            _trenutnoStanje = novoStanje;
            OnPropertyChanged(nameof(Status)); // Bitno za osvežavanje UI-ja i grafikona!
        }

        // Metoda koju ćemo pozivati preko korisničkog interfejsa za simulaciju
        public void SimulirajPromenu()
        {
            _trenutnoStanje.PredjiUSledeceStanje(this);
        }

        //Samo na pocetku, kada se ucitaju podaci iz baze, potrebno je rekonstruisati konkretno stanje na osnovu enuma statusa
        private void PostaviStanjeNaOsnovuStatusa(StatusKonferencije status)
        {
            switch (status)
            {
                case StatusKonferencije.OtvorenaPrijava:
                    _trenutnoStanje = new StanjeOtvorenaPrijava();
                    break;
                case StatusKonferencije.VelikoInteresovanje:
                    _trenutnoStanje = new StanjeVelikoInteresovanje();
                    break;
                case StatusKonferencije.UPripremi:
                    _trenutnoStanje = new StanjeUPripremi();
                    break;
                case StatusKonferencije.Odrzana:
                    _trenutnoStanje = new StanjeOdrzana();
                    break;
                default:
                    _trenutnoStanje = new StanjeOtvorenaPrijava();
                    break;
            }
            OnPropertyChanged(nameof(Status));
        }
    }
}
