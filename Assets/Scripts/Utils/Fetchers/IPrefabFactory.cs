using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
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

        IExplosion CreateExplosion(IExplosionStats explosionStats);
	}
}
