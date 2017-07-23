using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Fetchers
{
	public interface IPrefabFactory
	{
        IBuildableWrapper<Building> GetBuildingWrapperPrefab(IPrefabKey buildingKey);
		Building CreateBuilding(IBuildableWrapper<Building> buildingWrapperPrefab);

		UnitWrapper GetUnitWrapperPrefab(IPrefabKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);

		Cruiser GetCruiserPrefab(IPrefabKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);
	}
}
