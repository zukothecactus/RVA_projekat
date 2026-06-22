using System.Collections.Generic;
using Komponenta2.Models;

namespace Komponenta2.Strategije
{
    /// <summary>
    /// Strategija za izračunavanje jedne statističke metode nad podacima jednog perioda.
    /// </summary>
    public interface IStatistickaMetoda
    {
        string Naziv { get; }   // čitljiv naziv za prikaz i CSV, npr. "Najveći broj radova i učesnika"

        // Računa nad listom jednog perioda; 'period' je ključ koji se upisuje u rezultate.
        List<StatistickiRezultat> Izracunaj(List<KonkretnaStatistika> podaci, string period);
    }
}
