using System.Collections.Generic;
using Komponenta2.Models;

namespace Komponenta2.Services.Interfaces
{
    /// <summary>
    /// Servis za izvoz rezultata statističke obrade u CSV datoteku.
    /// </summary>
    public interface ICsvServis
    {
        void Izvezi(string putanja, IEnumerable<StatistickiRezultat> rezultati);
    }
}
