using System.Collections.Generic;

namespace Komponenta2.ViewModels
{
    /// <summary>
    /// Display-model jednog perioda sa pripadajućim konferencijama. Gradi se iznova
    /// pri svakom osvežavanju prikaza, pa ne implementira INotifyPropertyChanged.
    /// </summary>
    public class PrikazPeriodaModel
    {
        public string Kljuc { get; set; } = "";              // mašinski ključ perioda
        public string ZaglavljePerioda { get; set; } = "";   // npr. "(2026-05-25, 2026-05-28)"
        public List<PrikazKonferencijeModel> Konferencije { get; set; } = new();
    }
}
