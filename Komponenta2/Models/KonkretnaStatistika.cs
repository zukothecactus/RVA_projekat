using Ugovori;

namespace Komponenta2.Models
{
    /// <summary>
    /// Nepromenljiv snimak statistike koji se samo prikazuje u korisničkom interfejsu.
    /// Namerno ne implementira INotifyPropertyChanged jer se vrednosti ne menjaju nakon učitavanja.
    /// </summary>
    public class KonkretnaStatistika
    {
        public string NazivKonferencije { get; set; } = string.Empty;
        public int BrojRadova { get; set; }
        public int BrojUcesnika { get; set; }
        public int BrojSesija { get; set; }
        public StatusKonferencije Status { get; set; }

        /// <summary>
        /// Izvedeni prikaz trojke (broj radova, učesnika i sesija).
        /// </summary>
        public string Trojka => $"[{BrojRadova}, {BrojUcesnika}, {BrojSesija}]";
    }
}
