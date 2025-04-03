using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPPrefabCache
    {
        private readonly PvPMultiCache<PvPBuildableWrapper<IPvPBuilding>> _buildings;
        private readonly PvPMultiCache<PvPBuildableWrapper<IPvPUnit>> _units;
        private readonly PvPMultiCache<PvPCruiser> _cruisers;
        private readonly PvPMultiCache<PvPExplosionController> _explosions;
        private readonly PvPMultiCache<PvPShipDeathInitialiser> _shipDeaths;
        private readonly PvPMultiCache<PvPPrefab> _projectiles;
        private readonly PvPMultiCache<PvPBuildableOutlineController> _outlines;

        IDictionary<string, PvPPrefab> _allPrefabs = new Dictionary<string, PvPPrefab>();

        public PvPDroneController Drone { get; }
        public PvPAudioSourceInitialiser AudioSource { get; }

        public PvPPrefabCache(
            PvPMultiCache<PvPBuildableWrapper<IPvPBuilding>> buildings,
            PvPMultiCache<PvPBuildableWrapper<IPvPUnit>> units,
            PvPMultiCache<PvPCruiser> cruisers,
            PvPMultiCache<PvPExplosionController> explosions,
            PvPMultiCache<PvPShipDeathInitialiser> shipDeaths,
            PvPMultiCache<PvPPrefab> projectiles,
            PvPDroneController drone,
            PvPAudioSourceInitialiser audioSource,
            PvPMultiCache<PvPBuildableOutlineController> outlines)
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

            foreach (IPrefabKey key in _buildings.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _buildings.GetPrefab(key));
            }

            foreach (IPrefabKey key in _units.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _units.GetPrefab(key));
            }

            foreach (IPrefabKey key in _cruisers.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _cruisers.GetPrefab(key));
            }

            foreach (IPrefabKey key in _explosions.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _explosions.GetPrefab(key));
            }

            foreach (IPrefabKey key in _shipDeaths.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _shipDeaths.GetPrefab(key));
            }

            foreach (IPrefabKey key in _projectiles.GetKeys())
            {
                _allPrefabs.Add(key.PrefabPath, _projectiles.GetPrefab(key));
            }

            _allPrefabs.Add(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone.PrefabPath, Drone);
            _allPrefabs.Add(PvPStaticPrefabKeys.AudioSource.PrefabPath, AudioSource);
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

        public PvPPrefab GetPrefab(string _prefabPath)
        {
            return _allPrefabs[_prefabPath];
        }
    }
}