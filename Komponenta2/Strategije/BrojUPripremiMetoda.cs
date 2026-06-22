using System.Collections.Generic;
using System.Linq;
using Komponenta2.Models;
using Ugovori;

namespace Komponenta2.Strategije
{
    /// <summary>
    /// Strategija koja broji statistike u stanju "U pripremi" u periodu.
    /// </summary>
    public class BrojUPripremiMetoda : IStatistickaMetoda
    {
        public string Naziv => "Broj konferencija u pripremi";

        public List<StatistickiRezultat> Izracunaj(List<KonkretnaStatistika> podaci, string period)
        {
            return new List<StatistickiRezultat>
            {
                new StatistickiRezultat
                {
                    Metoda = Naziv,
                    Period = period,
                    Stavka = "Broj statistika u stanju 'U pripremi'",
                    Vrednost = podaci.Count(s => s.Status == StatusKonferencije.UPripremi)
                }
            };
        }
    }
}
