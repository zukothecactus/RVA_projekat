using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Komponenta1.Helpers
{
    //Omogucava WPF-u da poziva nase metode
    public class RelejKomanda: ICommand
    {
        private readonly Action<object> _izvrsi;
        private readonly Predicate<object> _mozeDaSeIzvrsi;

        public RelejKomanda(Action<object> izvrsi, Predicate<object> mozeDaSeIzvrsi = null)
        {
            _izvrsi = izvrsi ?? throw new ArgumentNullException(nameof(izvrsi));
            _mozeDaSeIzvrsi = mozeDaSeIzvrsi;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _mozeDaSeIzvrsi == null || _mozeDaSeIzvrsi(parameter);
        }

        public void Execute(object parameter)
        {
            _izvrsi(parameter);
        }
    }
}
