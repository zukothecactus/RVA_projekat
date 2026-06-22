using System;
using System.Collections.Generic;
using Komponenta2.Models;
using Ugovori;

namespace Komponenta2.Services.Interfaces
{
    /// <summary>
    /// Adapter koji prevodi DTO objekte iz ugovora (KonferencijskaStatistikaDto)
    /// u domenske snimke (KonkretnaStatistika) i akumulira ih po periodima.
    /// </summary>
    public interface IStatistikaAdapter
    {
        // Dodaje statistike za jedan period u POSTOJEĆI rečnik (akumulacija).
        void DodajUStrukturu(
            Dictionary<string, List<KonkretnaStatistika>> ciljniRecnik,
            List<KonferencijskaStatistikaDto> primljeneStatistike,
            DateTime od,
            DateTime doDatuma);

        // Pomoćna: pravi mašinski ključ "yyyy-MM-dd-yyyy-MM-dd".
        string NapraviKljuc(DateTime od, DateTime doDatuma);
    }
}
