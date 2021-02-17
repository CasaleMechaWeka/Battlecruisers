using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        // FELIX  Remove.  Instead have what it's used for?  Eg, enemy name, background image
        int Num { get; }

        IPrefabKey Hull { get; }
        SoundKeyPair MusicKeys { get; }
        string SkyMaterialName { get; }
    }
}
