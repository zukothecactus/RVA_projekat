using System.Collections.Generic;
using System.Windows;
using Komponenta2.Adapteri;
using Komponenta2.Services;
using Komponenta2.Services.Interfaces;
using Komponenta2.Strategije;
using Komponenta2.ViewModels;
using Komponenta2.Views;
using Ugovori;

namespace Komponenta2;

/// <summary>
/// Interakciona logika za App.xaml — kompoziciona tačka aplikacije.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IStatistikaServis izvor = FabrikaIzvora.NapraviIzvor(out string opisIzvora);
        IStatistikaAdapter adapter = new StatistikaAdapter();
        var metode = new List<IStatistickaMetoda>
        {
            new NajveciRadoviIUcesniciMetoda(),
            new ProsekSesijaMetoda(),
            new BrojUPripremiMetoda()
        };
        ICsvServis csv = new CsvServis();

        var vm = new GlavniViewModel(izvor, adapter, metode, csv, opisIzvora);
        new GlavniProzor { DataContext = vm }.Show();
    }
}
