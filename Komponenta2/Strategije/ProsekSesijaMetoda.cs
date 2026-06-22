using System.Collections.Generic;
using System.Linq;
using Komponenta2.Models;

namespace Komponenta2.Strategije
{
    /// <summary>
    /// Strategija koja računa prosečan broj sesija u periodu.
    /// </summary>
    public class ProsekSesijaMetoda : IStatistickaMetoda
    {
        public string Naziv => "Prosečan broj sesija";

        public List<StatistickiRezultat> Izracunaj(List<KonkretnaStatistika> podaci, string period)
        {
            if (podaci.Count == 0)
            {
                return new List<StatistickiRezultat>();
            }

            return new List<StatistickiRezultat>
            {
                new StatistickiRezultat
                {
                    Metoda = Naziv,
                    Period = period,
                    Stavka = "Prosečan broj sesija",
                    Vrednost = podaci.Average(s => s.BrojSesija)
                }
            };
        }
    }
}
