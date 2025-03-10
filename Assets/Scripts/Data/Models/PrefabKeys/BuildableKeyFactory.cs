using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class BuildableKeyFactory : IBuildableKeyFactory
    {
        public UnitKey CreateUnitKey(IUnit unit)
        {
            return new UnitKey(unit.Category, unit.PrefabName);
        }

        public BuildingKey CreateBuildingKey(IBuilding building)
        {
            return new BuildingKey(building.Category, building.PrefabName);
        }
    }
}