using System.Runtime.Serialization;

namespace Ugovori
{
    [DataContract]
    public enum StatusKonferencije
    {
        [EnumMember] OtvorenaPrijava,
        [EnumMember] VelikoInteresovanje,
        [EnumMember] UPripremi,
        [EnumMember] Odrzana
    }
}
