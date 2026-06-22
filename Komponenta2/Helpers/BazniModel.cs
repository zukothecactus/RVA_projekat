using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Komponenta2.Helpers
{
    /// <summary>
    /// Bazna klasa za sve modele/viewmodele koja obezbeđuje obaveštavanje
    /// o promeni svojstava (INotifyPropertyChanged).
    /// </summary>
    public abstract class BazniModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? ime = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ime));
        }

        /// <summary>
        /// Postavlja novu vrednost polja i, ukoliko se vrednost promenila,
        /// emituje obaveštenje o promeni svojstva. Vraća true ako je vrednost izmenjena.
        /// </summary>
        protected bool PostaviVrednost<T>(ref T polje, T vrednost, [CallerMemberName] string? ime = null)
        {
            if (EqualityComparer<T>.Default.Equals(polje, vrednost))
            {
                return false;
            }

            polje = vrednost;
            OnPropertyChanged(ime);
            return true;
        }
    }
}
