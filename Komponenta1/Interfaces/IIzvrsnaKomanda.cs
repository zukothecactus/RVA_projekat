using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Interfaces
{
    //ne mozemo da ga nazovemo ICommand jer to WPF koristi
    public interface IIzvrsnaKomanda
    {
       void Izvrsi();
       void Ponisti();
    }
}
