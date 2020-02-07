using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public interface IPrefabFactory
	{
        IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey);
		IBuilding CreateBuilding(IBuildableWrapper<IBuilding> buildingWrapperPrefab, IUIManager uiManager, IFactoryProvider factoryProvider);

		IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey);
        IUnit CreateUnit(IBuildableWrapper<IUnit> unitWrapperPrefab, IUIManager uiManager, IFactoryProvider factoryProvider);

		Cruiser GetCruiserPrefab(IPrefabKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);

        CountdownController CreateDeleteCountdown(Transform parent);

        IExplosion CreateExplosion(ExplosionKey explosionKey);
        IShipDeath CreateShipDeath(ShipDeathKey shipDeathKey);

        TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(ProjectileKey prefabKey, IFactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats;

        IDroneController CreateDrone();
    }
}
