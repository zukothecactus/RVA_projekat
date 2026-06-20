using Komponenta1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Model
{
    public class StanjeOtvorenaPrijava : IStanjeKonferencije
    {
        public StatusKonferencije TrenutniStatus => StatusKonferencije.OtvorenaPrijava;

        public void PredjiUSledeceStanje(KonferencijskaStatistika kontekst)
        {
            kontekst.PostaviStanje(new StanjeVelikoInteresovanje());
        }
    }

    public class StanjeVelikoInteresovanje : IStanjeKonferencije
    {
        public StatusKonferencije TrenutniStatus => StatusKonferencije.VelikoInteresovanje;

        public void PredjiUSledeceStanje(KonferencijskaStatistika kontekst)
        {
            kontekst.PostaviStanje(new StanjeUPripremi());
        }
    }

    public class StanjeUPripremi : IStanjeKonferencije
    {
        public StatusKonferencije TrenutniStatus => StatusKonferencije.UPripremi;

        public void PredjiUSledeceStanje(KonferencijskaStatistika kontekst)
        {
            kontekst.PostaviStanje(new StanjeOdrzana());
        }
    }

    public class StanjeOdrzana : IStanjeKonferencije
    {
        public StatusKonferencije TrenutniStatus => StatusKonferencije.Odrzana;

        public void PredjiUSledeceStanje(KonferencijskaStatistika kontekst)
        {
            // Održana je poslednje stanje, nema prelaska dalje. 
            // Ovde možemo ostaviti prazno ili u budućnosti dodati logiku za reset.
        }
    }
}
