using Komponenta1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Helpers
{
    public class MenadzerKomandi: IObavestavac<string>
    {
        private Stack<IIzvrsnaKomanda> _istorijaUndo = new Stack<IIzvrsnaKomanda>();
        private Stack<IIzvrsnaKomanda> _istorijaRedo = new Stack<IIzvrsnaKomanda>();

        private List<IPosmatrac<string>> _posmatraci = new List<IPosmatrac<string>>();

        // ---------------------nativne metode------------------------------- \\
        public void IzvrsiKomandu(IIzvrsnaKomanda komanda, string opisAktivnosti)
        {
            komanda.Izvrsi();
            _istorijaUndo.Push(komanda);
            _istorijaRedo.Clear(); // Očisti redo stablo
            ObavestiSve($"IZVRŠENO: {opisAktivnosti}");
        }

        public void Ponisti() //undo
        {
            if (_istorijaUndo.Count > 0)
            {
                var komanda = _istorijaUndo.Pop();
                komanda.Ponisti();
                _istorijaRedo.Push(komanda);
                ObavestiSve($"PONIŠTENO (Undo): Akcija je povučena.");
            }
        }

        public void Ponovi() //redo
        {
            if (_istorijaRedo.Count > 0)
            {
                var komanda = _istorijaRedo.Pop();
                komanda.Izvrsi();
                _istorijaUndo.Push(komanda);
                ObavestiSve($"PONOVLJENO (Redo): Akcija je ponovo primenjena.");
            }
        }

        // ---------------------Observer metode------------------------------- \\
        public void Pretplati(IPosmatrac<string> posmatrac)
        {
            if (!_posmatraci.Contains(posmatrac))
                _posmatraci.Add(posmatrac);
        }

        public void Odjavi(IPosmatrac<string> posmatrac)
        {
            _posmatraci.Remove(posmatrac);
        }

        public void ObavestiSve(string podaci)
        {
            foreach (var posmatrac in _posmatraci)
            {
                posmatrac.Azuriraj(podaci);
            }
        }
    }
}