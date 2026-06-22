using System;
using System.Windows;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CoreWCF.Configuration;
using Komponenta1.Services;
using Ugovori;

namespace Komponenta1
{
    public partial class App : Application
    {
        // Koristimo moderni IHost umesto starog ServiceHost
        private IHost _wcfHost;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Kreiramo pozadinski proces koji će vrteti naš WCF servis
                _wcfHost = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        // 1. Postavljamo port za brzu lokalnu komunikaciju
                        webBuilder.UseNetTcp(8080);

                        // 2. Registrujemo WCF servise u sistem
                        webBuilder.ConfigureServices(services =>
                        {
                            services.AddServiceModelServices();
                        });

                        // 3. Konfigurišemo sam endpoint
                        webBuilder.Configure(app =>
                        {
                            app.UseServiceModel(builder =>
                            {
                                builder.AddService<StatistikaServis>();
                                builder.AddServiceEndpoint<StatistikaServis, IStatistikaServis>(
                                    new CoreWCF.NetTcpBinding(),
                                    "net.tcp://localhost:8080/StatistikaServis");
                            });
                        });
                    })
                    .Build();

                // Pokrećemo WCF u pozadini bez blokiranja korisničkog interfejsa (WPF-a)
                await _wcfHost.StartAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri pokretanju WCF servisa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Bezbedno gašenje servisa kada ugasimo program
            if (_wcfHost != null)
            {
                await _wcfHost.StopAsync();
                _wcfHost.Dispose();
            }

            base.OnExit(e);
        }
    }
}