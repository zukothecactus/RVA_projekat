namespace Komponenta2.ViewModels
{
    /// <summary>
    /// Display-model jedne konferencije unutar perioda. Gradi se iznova pri svakom
    /// osvežavanju prikaza, pa ne implementira INotifyPropertyChanged.
    /// </summary>
    public class PrikazKonferencijeModel
    {
        public string Linija { get; set; } = "";
    }
}
