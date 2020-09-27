using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        int Num { get; }
        IPrefabKey Hull { get; }
        SoundKeyPair MusicKeys { get; }
        string SkyMaterialName { get; }
    }
}
