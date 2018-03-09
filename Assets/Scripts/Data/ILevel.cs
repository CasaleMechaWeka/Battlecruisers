using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Clouds;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        int Num { get; }
        string Name { get; }
        IPrefabKey Hull { get; }
        string SkyMaterialName { get; }
        ICloudGenerationStats CloudStats { get; }
    }
}
