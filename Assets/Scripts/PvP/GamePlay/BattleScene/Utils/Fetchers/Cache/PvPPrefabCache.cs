using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Utils.Fetchers.Cache;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPPrefabCache
    {
        private readonly MultiCache<PvPBuildableWrapper<IPvPBuilding>> _buildings;
        private readonly MultiCache<PvPBuildableWrapper<IPvPUnit>> _units;
        private readonly MultiCache<PvPCruiser> _cruisers;
        private readonly MultiCache<PvPExplosionController> _explosions;
        private readonly MultiCache<PvPShipDeathInitialiser> _shipDeaths;
        private readonly MultiCache<PvPPrefab> _projectiles;
        private readonly MultiCache<PvPBuildableOutlineController> _outlines;

        public PvPDroneController Drone { get; }
        public PvPAudioSourceInitialiser AudioSource { get; }

        public PvPPrefabCache(
            MultiCache<PvPBuildableWrapper<IPvPBuilding>> buildings,
            MultiCache<PvPBuildableWrapper<IPvPUnit>> units,
            MultiCache<PvPCruiser> cruisers,
            MultiCache<PvPExplosionController> explosions,
            MultiCache<PvPShipDeathInitialiser> shipDeaths,
            MultiCache<PvPPrefab> projectiles,
            PvPDroneController drone,
            PvPAudioSourceInitialiser audioSource,
            MultiCache<PvPBuildableOutlineController> outlines)
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
            _outlines = outlines;
        }

        public PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPrefabKey key)
        {
            return _buildings.GetPrefab(key);
        }
        public PvPBuildableOutlineController GetOutline(IPrefabKey key)
        {
            return _outlines.GetPrefab(key);
        }

        public PvPBuildableWrapper<IPvPUnit> GetUnit(IPrefabKey key)
        {
            return _units.GetPrefab(key);
        }

        public PvPCruiser GetCruiser(IPrefabKey key)
        {
            return _cruisers.GetPrefab(key);
        }

        public PvPExplosionController GetExplosion(IPrefabKey key)
        {
            return _explosions.GetPrefab(key);
        }

        public PvPShipDeathInitialiser GetShipDeath(IPrefabKey key)
        {
            return _shipDeaths.GetPrefab(key);
        }

        public PvPPrefab GetProjectile(IPrefabKey prefabKey)
        {
            return _projectiles.GetPrefab(prefabKey);
        }
    }
}