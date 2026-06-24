using System;
using System.Collections.Generic;
using System.ServiceModel;
using Ugovori;

namespace Komponenta2.Services
{
    /// <summary>
    /// WCF klijent koji preko NetTcpBinding-a komunicira sa servisom Komponente 1.
    /// Implementira isti ugovor (IStatistikaServis) kao i lokalni izvor.
    /// </summary>
    public class StatistikaKlijent : IStatistikaServis, IDisposable
    {
        public const string PODRAZUMEVANA_ADRESA = "net.tcp://localhost:8080/StatistikaServis";
        // Usklađeno sa hostom Komponente 1 (CoreWCF NetTcp na portu 8080).

        private readonly ChannelFactory<IStatistikaServis> _fabrika;

        public StatistikaKlijent(string adresa = PODRAZUMEVANA_ADRESA)
        {
            // SecurityMode.None je obavezno jer K1 host mora da koristi isti režim — uskladiti sa kolegom.
            var binding = new NetTcpBinding(SecurityMode.None)
            {
                OpenTimeout = TimeSpan.FromSeconds(5),
                SendTimeout = TimeSpan.FromSeconds(5),
                ReceiveTimeout = TimeSpan.FromSeconds(5)
            };

            _fabrika = new ChannelFactory<IStatistikaServis>(binding, new EndpointAddress(adresa));
        }

        public List<KonferencijskaStatistikaDto> PreuzmiStatistikePoPeriodu(DateTime od, DateTime doDatuma)
        {
            var kanal = _fabrika.CreateChannel();
            try
            {
                var rezultat = kanal.PreuzmiStatistikePoPeriodu(od, doDatuma);
                ((IClientChannel)kanal).Close();
                return rezultat;
            }
            catch
            {
                ((IClientChannel)kanal).Abort();
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_fabrika.State == CommunicationState.Faulted)
                {
                    _fabrika.Abort();
                }
                else
                {
                    _fabrika.Close();
                }
            }
            catch
            {
                _fabrika.Abort();
            }
        }
    }
}
