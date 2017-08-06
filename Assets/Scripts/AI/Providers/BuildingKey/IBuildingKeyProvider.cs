using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public interface IBuildingKeyProvider
    {
        IPrefabKey Next { get; }
    }
}