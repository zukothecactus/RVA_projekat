using System;
using System.Windows.Input;

namespace Komponenta2.Helpers
{
    /// <summary>
    /// Generička implementacija ICommand interfejsa koja delegira izvršavanje
    /// i proveru mogućnosti izvršavanja na prosleđene delegate.
    /// </summary>
    public class RelejKomanda : ICommand
    {
        private readonly Action<object?> _izvrsi;
        private readonly Func<object?, bool>? _mozeDaSeIzvrsi;

        public RelejKomanda(Action<object?> izvrsi, Func<object?, bool>? mozeDaSeIzvrsi = null)
        {
            _izvrsi = izvrsi ?? throw new ArgumentNullException(nameof(izvrsi));
            _mozeDaSeIzvrsi = mozeDaSeIzvrsi;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _mozeDaSeIzvrsi?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _izvrsi(parameter);
    }
}
