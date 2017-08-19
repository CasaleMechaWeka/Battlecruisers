using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    // FELIX  Rename
    public interface INewBuildingKeyProvider
    {
        bool HasNext { get; }
        IPrefabKey Next { get; }
    }
}
