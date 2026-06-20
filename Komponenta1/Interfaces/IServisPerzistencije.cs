using Komponenta1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Interfaces
{
    public interface IServisPerzistencije
    {
        PodaciAplikacije UcitajPodatke();
        void SacuvajPodatke(PodaciAplikacije podaci);
    }
}
