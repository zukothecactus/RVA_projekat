using System;
using System.Collections.Generic;
using System.Linq;
using Ugovori; // Projekat sa ugovorima koji si prosledio
using Komponenta1.ViewModel;

namespace Komponenta1.Services
{
    public class StatistikaServis : IStatistikaServis
    {
        public List<KonferencijskaStatistikaDto> PreuzmiStatistikePoPeriodu(DateTime od, DateTime doDatuma)
        {
            var rezultatDtoLista = new List<KonferencijskaStatistikaDto>();

            // Proveravamo da li je naš ViewModel živ i da li ima podataka
            if (GlavniViewModel.Instanca == null || GlavniViewModel.Instanca.SveStatistike == null)
            {
                return rezultatDtoLista; // Vraćamo praznu listu ako podaci nisu dostupni
            }

            // Filtriramo podatke iz memorije Komponente 1 na osnovu zadatog perioda
            var filtriraneStatistike = GlavniViewModel.Instanca.SveStatistike
                .Where(s => s.DatumOdrzavanja >= od && s.DatumOdrzavanja <= doDatuma)
                .ToList();

            // Mapiramo lokalne WPF modele (KonferencijskaStatistika) u WCF prenosive objekte (KonferencijskaStatistikaDto)
            foreach (var lokalnaStatistika in filtriraneStatistike)
            {
                // Pronalazimo naziv konferencije preko Id-ja kako bismo napunili NazivKonferencije u DTO-u
                string nazivKonferencije = GlavniViewModel.Instanca.SveKonferencije
                    .FirstOrDefault(k => k.Id == lokalnaStatistika.KonferencijaId)?.Naziv ?? "Nepoznata Konferencija";

                var dto = new KonferencijskaStatistikaDto
                {
                    KonferencijaId = lokalnaStatistika.KonferencijaId,
                    NazivKonferencije = nazivKonferencije,
                    DatumOdrzavanja = lokalnaStatistika.DatumOdrzavanja,
                    BrojRadova = lokalnaStatistika.BrojRadova,
                    BrojSesija = lokalnaStatistika.BrojSesija,
                    // Dodaj ako imaš i broj učesnika u lokalnom modelu, npr:
                    // BrojUcesnika = lokalnaStatistika.BrojUcesnika,
                    Status = (StatusKonferencije)lokalnaStatistika.Status // Automatski mapira tvoj enum
                };

                rezultatDtoLista.Add(dto);
            }

            return rezultatDtoLista;
        }
    }
}