using Komponenta1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Model
{
    public class Konferencija: ValidacioniModel
    {
        private Guid _id;
        private string _naziv;
        private string _oblast;
        private string _grad;

        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                if (string.IsNullOrWhiteSpace(_naziv))
                    DodajGresku(nameof(Naziv), "Naziv ne sme biti prazan.");
                else
                    UkloniGreske(nameof(Naziv));

                OnPropertyChanged();
            }
        }

        public string Oblast
        {
            get => _oblast;
            set
            {
                _oblast = value;
                if (string.IsNullOrWhiteSpace(_oblast))
                    DodajGresku(nameof(Oblast), "Oblast mora biti uneta.");
                else
                    UkloniGreske(nameof(Oblast));

                OnPropertyChanged();
            }
        }

        public string Grad
        {
            get => _grad;
            set
            {
                _grad = value;
                if (string.IsNullOrWhiteSpace(_grad))
                    DodajGresku(nameof(Grad), "Grad mora biti unet.");
                else
                    UkloniGreske(nameof(Grad));

                OnPropertyChanged();
            }
        }

        public Konferencija()
        {
            Id = Guid.NewGuid(); // Automatsko generisanje jedinstvenog identifikatora
        }
    }
}
