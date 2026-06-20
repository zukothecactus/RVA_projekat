using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Interfaces
{
    public interface IStanjeKonferencije
    {
        StatusKonferencije TrenutniStatus { get; }
        void PredjiUSledeceStanje(KonferencijskaStatistika kontekst);
    }
}
