namespace Komponenta2.Models
{
    /// <summary>
    /// Ravan red rezultata pogodan za CSV prikaz/izvoz.
    /// </summary>
    public class StatistickiRezultat
    {
        public string Metoda { get; set; } = string.Empty;   // naziv statističke metode
        public string Period { get; set; } = string.Empty;   // ključ "d1-d2" za koji je računato
        public string Stavka { get; set; } = string.Empty;   // npr. "Najveći broj radova"
        public double Vrednost { get; set; }                 // double pokriva i prosek i celobrojne vrednosti
    }
}
