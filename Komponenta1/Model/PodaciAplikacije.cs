using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Model
{
    //kontejner za sve podatke aplikacije, koristi se za serijalizaciju i deserijalizaciju
    public class PodaciAplikacije
    {
        public List<Konferencija> Konferencije { get; set; } = new List<Konferencija>();
        public List<KonferencijskaStatistika> Statistike { get; set; } = new List<KonferencijskaStatistika>();
    }
}
