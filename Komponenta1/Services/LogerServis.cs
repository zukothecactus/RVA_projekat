using Komponenta1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta1.Services
{
    public class LogerServis: IPosmatrac<string>
    {
        private readonly string _putanjaDatoteke = "log_aktivnosti.txt";
        public void Azuriraj(string poruka)
        {
            string logZapis = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {poruka}{Environment.NewLine}";

            File.AppendAllText(_putanjaDatoteke, logZapis);
        }
    }
}
