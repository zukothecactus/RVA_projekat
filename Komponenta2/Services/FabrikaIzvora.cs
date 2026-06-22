using System;
using System.ServiceModel;
using Ugovori;

namespace Komponenta2.Services
{
    /// <summary>
    /// Bira izvor statistike: prvo pokušava WCF servis Komponente 1, a ako veza
    /// nije dostupna, tiho pada na lokalni (fallback) izvor podataka.
    /// </summary>
    public static class FabrikaIzvora
    {
        public static IStatistikaServis NapraviIzvor(out string opisIzvora)
        {
            StatistikaKlijent? klijent = null;
            try
            {
                klijent = new StatistikaKlijent();
                // Probni poziv radi provere veze (kratak period).
                klijent.PreuzmiStatistikePoPeriodu(new DateTime(2026, 1, 1), new DateTime(2026, 1, 1));
                opisIzvora = "WCF servis (Komponenta 1)";
                return klijent;
            }
            catch (Exception)
            {
                klijent?.Dispose();
                opisIzvora = "Lokalni podaci (nema veze sa Komponentom 1)";
                return new LokalniIzvorStatistike();
            }
        }
    }
}
