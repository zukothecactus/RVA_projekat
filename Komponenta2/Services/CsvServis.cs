using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Komponenta2.Models;
using Komponenta2.Services.Interfaces;

namespace Komponenta2.Services
{
    /// <summary>
    /// Konkretna implementacija CSV izvoza. Sva polja se navode pod navodnicima
    /// (sa udvostručavanjem navodnika), a brojevi se pišu sa invarijantnom kulturom
    /// (decimalna tačka), radi ispravnog otvaranja u Excel-u.
    /// </summary>
    public class CsvServis : ICsvServis
    {
        public void Izvezi(string putanja, IEnumerable<StatistickiRezultat> rezultati)
        {
            var graditelj = new StringBuilder();

            // Zaglavlje
            graditelj.Append(SastaviRed("Metoda", "Period", "Stavka", "Vrednost"));

            foreach (var r in rezultati)
            {
                graditelj.Append(SastaviRed(
                    r.Metoda,
                    r.Period,
                    r.Stavka,
                    r.Vrednost.ToString(CultureInfo.InvariantCulture)));
            }

            File.WriteAllText(putanja, graditelj.ToString(), new UTF8Encoding(true)); // BOM za Excel/ćir-lat.
        }

        private static string SastaviRed(params string[] polja)
        {
            for (int i = 0; i < polja.Length; i++)
            {
                polja[i] = Obavij(polja[i]);
            }

            return string.Join(",", polja) + "\r\n";
        }

        private static string Obavij(string polje)
        {
            return "\"" + (polje ?? string.Empty).Replace("\"", "\"\"") + "\"";
        }
    }
}
