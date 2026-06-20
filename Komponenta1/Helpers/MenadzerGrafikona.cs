using Komponenta1.Interfaces;
using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    //prima celu listu statistika i proracumnava podatke za grafikon
    public class MenadzerGrafikona: IPosmatrac<IEnumerable<KonferencijskaStatistika>>
    {
        // bindovana na korisnički interfejs za LiveCharts
        public int BrojOtvorenihPrijava { get; private set; }
        public int BrojVelikoInteresovanje { get; private set; }
        public int BrojUPripremi { get; private set; }
        public int BrojOdrzanih { get; private set; }

        public void Azuriraj(IEnumerable<KonferencijskaStatistika> statistike)
        {
            if (statistike == null) return;

            BrojOtvorenihPrijava = statistike.Count(s => s.Status == StatusKonferencije.OtvorenaPrijava);
            BrojVelikoInteresovanje = statistike.Count(s => s.Status == StatusKonferencije.VelikoInteresovanje);
            BrojUPripremi = statistike.Count(s => s.Status == StatusKonferencije.UPripremi);
            BrojOdrzanih = statistike.Count(s => s.Status == StatusKonferencije.Odrzana);
            // ovde ce se dodavati logika za obavestavanje grafikona u viewmodelu da preume nove vrednosti
            //faza6
        }
    }
}
