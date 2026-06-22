using System;
using System.Collections.Generic;
using System.Linq;
using Ugovori;

namespace Komponenta2.Services
{
    /// <summary>
    /// Lokalni (fallback) izvor statistike koji implementira ugovor IStatistikaServis
    /// i služi kada WCF servis nije dostupan. Podaci su fiksni i nepromenljivi.
    /// </summary>
    public class LokalniIzvorStatistike : IStatistikaServis
    {
        private static readonly Guid IdKonferencijeA = new Guid("11111111-1111-1111-1111-111111111111");
        private static readonly Guid IdKonferencijeB = new Guid("22222222-2222-2222-2222-222222222222");
        private static readonly Guid IdKonferencijeC = new Guid("33333333-3333-3333-3333-333333333333");

        private readonly List<KonferencijskaStatistikaDto> _seedPodaci;

        public LokalniIzvorStatistike()
        {
            _seedPodaci = new List<KonferencijskaStatistikaDto>
            {
                // Konf A — "Konferencija o veštačkoj inteligenciji"
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeA,
                    NazivKonferencije = "Konferencija o veštačkoj inteligenciji",
                    DatumOdrzavanja = new DateTime(2026, 5, 25),
                    BrojRadova = 50,
                    BrojUcesnika = 150,
                    BrojSesija = 5,
                    Status = StatusKonferencije.VelikoInteresovanje
                },
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeA,
                    NazivKonferencije = "Konferencija o veštačkoj inteligenciji",
                    DatumOdrzavanja = new DateTime(2026, 5, 26),
                    BrojRadova = 75,
                    BrojUcesnika = 100,
                    BrojSesija = 7,
                    Status = StatusKonferencije.OtvorenaPrijava
                },
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeA,
                    NazivKonferencije = "Konferencija o veštačkoj inteligenciji",
                    DatumOdrzavanja = new DateTime(2026, 5, 27),
                    BrojRadova = 30,
                    BrojUcesnika = 20,
                    BrojSesija = 5,
                    Status = StatusKonferencije.UPripremi
                },

                // Konf B — "Konferencija o bazama podataka"
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeB,
                    NazivKonferencije = "Konferencija o bazama podataka",
                    DatumOdrzavanja = new DateTime(2026, 5, 26),
                    BrojRadova = 50,
                    BrojUcesnika = 100,
                    BrojSesija = 6,
                    Status = StatusKonferencije.UPripremi
                },
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeB,
                    NazivKonferencije = "Konferencija o bazama podataka",
                    DatumOdrzavanja = new DateTime(2026, 5, 27),
                    BrojRadova = 35,
                    BrojUcesnika = 15,
                    BrojSesija = 5,
                    Status = StatusKonferencije.Odrzana
                },
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeB,
                    NazivKonferencije = "Konferencija o bazama podataka",
                    DatumOdrzavanja = new DateTime(2026, 5, 28),
                    BrojRadova = 40,
                    BrojUcesnika = 60,
                    BrojSesija = 4,
                    Status = StatusKonferencije.VelikoInteresovanje
                },

                // Konf C — "Simpozijum o računarskim mrežama"
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeC,
                    NazivKonferencije = "Simpozijum o računarskim mrežama",
                    DatumOdrzavanja = new DateTime(2026, 5, 25),
                    BrojRadova = 20,
                    BrojUcesnika = 45,
                    BrojSesija = 3,
                    Status = StatusKonferencije.UPripremi
                },
                new KonferencijskaStatistikaDto
                {
                    KonferencijaId = IdKonferencijeC,
                    NazivKonferencije = "Simpozijum o računarskim mrežama",
                    DatumOdrzavanja = new DateTime(2026, 5, 28),
                    BrojRadova = 60,
                    BrojUcesnika = 200,
                    BrojSesija = 8,
                    Status = StatusKonferencije.Odrzana
                }
            };
        }

        /// <summary>
        /// Vraća novu listu statistika čiji datum održavanja pada u zadati period
        /// (granice uključene). Originalna seed lista se ne menja.
        /// </summary>
        public List<KonferencijskaStatistikaDto> PreuzmiStatistikePoPeriodu(DateTime od, DateTime doDatuma)
        {
            return _seedPodaci
                .Where(s => s.DatumOdrzavanja.Date >= od.Date && s.DatumOdrzavanja.Date <= doDatuma.Date)
                .ToList();
        }
    }
}
