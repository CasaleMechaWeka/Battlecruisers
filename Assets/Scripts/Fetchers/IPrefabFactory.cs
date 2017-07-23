using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Fetchers
{
	public interface IPrefabFactory
	{
        BuildingWrapper GetBuildingWrapperPrefab(IPrefabKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingWrapperPrefab);

		UnitWrapper GetUnitWrapperPrefab(IPrefabKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);

		Cruiser GetCruiserPrefab(IPrefabKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);
	}
}
