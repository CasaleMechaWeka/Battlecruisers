using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Fetchers
{
	public interface IPrefabFactory
	{
		BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingWrapperPrefab);

		UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);

		Cruiser GetCruiserPrefab(HullKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);
	}
}
