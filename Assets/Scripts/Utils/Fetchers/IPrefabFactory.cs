using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	public interface IPrefabFactory
	{
        IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey);
		IBuilding CreateBuilding(IBuildableWrapper<IBuilding> buildingWrapperPrefab);

		IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey);
        IUnit CreateUnit(IBuildableWrapper<IUnit> unitWrapperPrefab);

		Cruiser GetCruiserPrefab(IPrefabKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);

        CountdownController CreateDeleteCountdown(Transform parent);

        Explosion CreateExplosion();
	}
}
