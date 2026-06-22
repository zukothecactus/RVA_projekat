using System;
using System.Collections.Generic;
using System.Linq;
using Komponenta2.Models;
using Komponenta2.Services.Interfaces;
using Ugovori;

namespace Komponenta2.Adapteri
{
    /// <summary>
    /// Konkretna implementacija adaptera koja projektuje DTO statistike u domenske
    /// snimke i smešta ih u rečnik po ključu perioda.
    /// </summary>
    public class StatistikaAdapter : IStatistikaAdapter
    {
        public string NapraviKljuc(DateTime od, DateTime doDatuma)
        {
            return $"{od:yyyy-MM-dd}-{doDatuma:yyyy-MM-dd}";
        }

        public void DodajUStrukturu(
            Dictionary<string, List<KonkretnaStatistika>> ciljniRecnik,
            List<KonferencijskaStatistikaDto> primljeneStatistike,
            DateTime od,
            DateTime doDatuma)
        {
            string kljuc = NapraviKljuc(od, doDatuma);

            List<KonkretnaStatistika> projektovane = primljeneStatistike
                .Select(dto => new KonkretnaStatistika
                {
                    NazivKonferencije = dto.NazivKonferencije,
                    BrojRadova = dto.BrojRadova,
                    BrojUcesnika = dto.BrojUcesnika,
                    BrojSesija = dto.BrojSesija,
                    Status = dto.Status
                })
                .ToList();

            // Ako ključ već postoji, nadomesti ga novom listom (osvežavanje, ne duplanje).
            ciljniRecnik[kljuc] = projektovane;
        }
    }
}
