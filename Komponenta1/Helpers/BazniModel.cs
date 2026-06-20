using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    public abstract class BazniModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string imaSvojstva = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(imaSvojstva));
        }
    }
}
