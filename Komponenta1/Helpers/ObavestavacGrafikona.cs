using Komponenta1.Interfaces;
using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    public class ObavestivacGrafikona : IObavestavac<IEnumerable<KonferencijskaStatistika>>
    {
        private List<IPosmatrac<IEnumerable<KonferencijskaStatistika>>> _posmatraci = new List<IPosmatrac<IEnumerable<KonferencijskaStatistika>>>();

        public void Pretplati(IPosmatrac<IEnumerable<KonferencijskaStatistika>> posmatrac)
        {
            if (!_posmatraci.Contains(posmatrac))
            {
                _posmatraci.Add(posmatrac);
            }
        }

        public void Odjavi(IPosmatrac<IEnumerable<KonferencijskaStatistika>> posmatrac)
        {
            _posmatraci.Remove(posmatrac);
        }

        public void ObavestiSve(IEnumerable<KonferencijskaStatistika> podaci)
        {
            foreach (var posmatrac in _posmatraci)
            {
                posmatrac.Azuriraj(podaci);
            }
        }
    }
}
