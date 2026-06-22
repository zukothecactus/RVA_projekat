using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Ugovori
{
    [ServiceContract]
    public interface IStatistikaServis
    {
        [OperationContract]
        List<KonferencijskaStatistikaDto> PreuzmiStatistikePoPeriodu(DateTime od, DateTime doDatuma);
    }
}
