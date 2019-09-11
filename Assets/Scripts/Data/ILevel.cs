using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        int Num { get; }
        string Name { get; }
        IPrefabKey Hull { get; }
        SoundKeyPair MusicKeys { get; }
        string SkyMaterialName { get; }
        ICloudGenerationStats CloudStats { get; }
    }
}
