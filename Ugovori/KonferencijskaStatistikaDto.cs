using System;
using System.Runtime.Serialization;

namespace Ugovori
{
    [DataContract]
    public class KonferencijskaStatistikaDto
    {
        [DataMember] public Guid KonferencijaId { get; set; }
        [DataMember] public string NazivKonferencije { get; set; } = string.Empty;
        [DataMember] public DateTime DatumOdrzavanja { get; set; }
        [DataMember] public int BrojRadova { get; set; }
        [DataMember] public int BrojUcesnika { get; set; }
        [DataMember] public int BrojSesija { get; set; }
        [DataMember] public StatusKonferencije Status { get; set; }
    }
}
