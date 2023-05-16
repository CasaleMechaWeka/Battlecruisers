using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPPrefabCache : IPvPPrefabCache
    {
        private readonly IPvPMultiCache<PvPBuildableWrapper<IPvPBuilding>> _buildings;
        private readonly IPvPMultiCache<PvPBuildableWrapper<IPvPUnit>> _units;
        private readonly IPvPMultiCache<PvPCruiser> _cruisers;
        private readonly IPvPMultiCache<PvPExplosionController> _explosions;
        private readonly IPvPMultiCache<PvPShipDeathInitialiser> _shipDeaths;
        private readonly IPvPUntypedMultiCache<PvPProjectile> _projectiles;

        public PvPDroneController Drone { get; }
        public PvPAudioSourceInitialiser AudioSource { get; }

        public PvPPrefabCache(
            IPvPMultiCache<PvPBuildableWrapper<IPvPBuilding>> buildings,
            IPvPMultiCache<PvPBuildableWrapper<IPvPUnit>> units,
            IPvPMultiCache<PvPCruiser> cruisers,
            IPvPMultiCache<PvPExplosionController> explosions,
            IPvPMultiCache<PvPShipDeathInitialiser> shipDeaths,
            IPvPUntypedMultiCache<PvPProjectile> projectiles,
            PvPDroneController drone,
            PvPAudioSourceInitialiser audioSource)
        {
            PvPHelper.AssertIsNotNull(buildings, units, cruisers, explosions, shipDeaths, projectiles, drone, audioSource);

            _buildings = buildings;
            _units = units;
            _cruisers = cruisers;
            _explosions = explosions;
            _shipDeaths = shipDeaths;
            _projectiles = projectiles;
            Drone = drone;
            AudioSource = audioSource;
        }

        public PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPvPPrefabKey key)
        {
            return _buildings.GetPrefab(key);
        }

        public PvPBuildableWrapper<IPvPUnit> GetUnit(IPvPPrefabKey key)
        {
            return _units.GetPrefab(key);
        }

        public PvPCruiser GetCruiser(IPvPPrefabKey key)
        {
            return _cruisers.GetPrefab(key);
        }

        public PvPExplosionController GetExplosion(IPvPPrefabKey key)
        {
            return _explosions.GetPrefab(key);
        }

        public PvPShipDeathInitialiser GetShipDeath(IPvPPrefabKey key)
        {
            return _shipDeaths.GetPrefab(key);
        }

        public TProjectile GetProjectile<TProjectile>(IPvPPrefabKey prefabKey) where TProjectile : PvPProjectile
        {
            return _projectiles.GetPrefab<TProjectile>(prefabKey);
        }
    }
}