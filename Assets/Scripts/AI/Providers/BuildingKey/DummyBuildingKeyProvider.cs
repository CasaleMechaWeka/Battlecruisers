using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class DummyBuildingKeyProvider : IBuildingKeyProvider
    {
        public IPrefabKey Next { get; private set; }

        public DummyBuildingKeyProvider(IPrefabKey buildingKey)
        {
            Next = buildingKey;
        }
    }
}