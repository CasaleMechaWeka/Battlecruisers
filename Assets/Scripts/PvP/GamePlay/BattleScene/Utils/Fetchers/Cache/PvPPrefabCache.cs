using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
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
        private readonly IPvPMultiCache<PvPBuildableOutlineController> _outlines;

        IDictionary<string, PvPPrefab> _allPrefabs = new Dictionary<string, PvPPrefab>();

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
            PvPAudioSourceInitialiser audioSource,
            IPvPMultiCache<PvPBuildableOutlineController> outlines)
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

            foreach (IPvPPrefabKey key in _buildings.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _buildings.GetPrefab(key));
            }

            foreach (IPvPPrefabKey key in _units.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _units.GetPrefab(key));
            }

            foreach (IPvPPrefabKey key in _cruisers.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _cruisers.GetPrefab(key));
            }

            foreach (IPvPPrefabKey key in _explosions.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _explosions.GetPrefab(key));
            }

            foreach (IPvPPrefabKey key in _shipDeaths.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _shipDeaths.GetPrefab(key));
            }

            foreach (IPvPPrefabKey key in _projectiles.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _projectiles.GetPrefab<PvPProjectile>(key));
            }

            _allPrefabs.Add(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone.PrefabPath, Drone);
            _allPrefabs.Add(PvPStaticPrefabKeys.AudioSource.PrefabPath, AudioSource);
        }




        public PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPvPPrefabKey key)
        {
            return _buildings.GetPrefab(key);
        }
        public PvPBuildableOutlineController GetOutline(IPvPPrefabKey key)
        {
            return _outlines.GetPrefab(key);
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

        public PvPPrefab GetPrefab(string _prefabPath)
        {
            return _allPrefabs[_prefabPath];
        }
    }
}