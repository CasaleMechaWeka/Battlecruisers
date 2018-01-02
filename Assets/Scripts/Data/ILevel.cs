using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        int Num { get; }
        string Name { get; }
        IPrefabKey Hull { get; }
        string SkyMaterialName { get; }
    }
}
