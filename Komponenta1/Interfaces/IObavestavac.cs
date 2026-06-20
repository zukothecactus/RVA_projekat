using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Interfaces
{
    public interface IObavestavac<T>
    {
        void Pretplati(IPosmatrac<T> posmatrac);
        void Odjavi(IPosmatrac<T> posmatrac);
        void ObavestiSve(T podaci);
    }
}
