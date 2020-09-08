using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface IBuildableKeyFactory
    {
        BuildingKey CreateBuildingKey(IBuilding building);
        UnitKey CreateUnitKey(IUnit unit);
    }
}