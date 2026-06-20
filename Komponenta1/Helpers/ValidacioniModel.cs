using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    //kako ne bismo zagadivali klase kodom za proveru gresaka, kreiramo posebnu klasu za validaciju podataka
    public class ValidacioniModel: BazniModel, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _greske = new Dictionary<string, List<string>>();

        public bool HasErrors => _greske.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //INotifyDataErrorInfo implementacija
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_greske.ContainsKey(propertyName))
                return null;

            return _greske[propertyName];
        }

        //metode za dodavanje i uklanjanje gresaka
        protected void DodajGresku(string svojstvo, string poruka)
        {
            if (!_greske.ContainsKey(svojstvo))
                _greske[svojstvo] = new List<string>();

            if (!_greske[svojstvo].Contains(poruka))
            {
                _greske[svojstvo].Add(poruka);
                OkinutiErrorsChanged(svojstvo);
            }
        }

        protected void UkloniGreske(string svojstvo)
        {
            if (_greske.ContainsKey(svojstvo))
            {
                _greske.Remove(svojstvo);
                OkinutiErrorsChanged(svojstvo);
            }
        }

        private void OkinutiErrorsChanged(string svojstvo)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(svojstvo));
            OnPropertyChanged(nameof(HasErrors));
        }
    }
}
