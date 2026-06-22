using System.Collections.Generic;
using System.Linq;
using Komponenta2.Models;

namespace Komponenta2.Strategije
{
    /// <summary>
    /// Strategija koja pronalazi najveći broj radova i najveći broj učesnika u periodu.
    /// </summary>
    public class NajveciRadoviIUcesniciMetoda : IStatistickaMetoda
    {
        public string Naziv => "Najveći broj radova i učesnika";

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
                    Stavka = "Najveći broj radova",
                    Vrednost = podaci.Max(s => s.BrojRadova)
                },
                new StatistickiRezultat
                {
                    Metoda = Naziv,
                    Period = period,
                    Stavka = "Najveći broj učesnika",
                    Vrednost = podaci.Max(s => s.BrojUcesnika)
                }
            };
        }
    }
}
