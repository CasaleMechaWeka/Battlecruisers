using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog
{
    /// <summary>
    /// Determines when to re-evalute whether fog of war should be enabled or not.
    /// </summary>
    public class PvPFogOfWarManager : IPvPManagedDisposable
    {
        private readonly IPvPGameObject _fog;
        private readonly IPvPFogVisibilityDecider _visibilityDecider;
        private readonly IPvPCruiserBuildingMonitor _friendlyBuildingMonitor, _enemyBuildingMonitor;
        private readonly IList<IPvPStealthGenerator> _friendlyIStealthGenerators;
        private readonly IList<IPvPSpySatelliteLauncher> _enemySpySatellites;

        public PvPFogOfWarManager(
            IPvPGameObject fog,
            IPvPFogVisibilityDecider visibilityDecider,
            IPvPCruiserBuildingMonitor friendlyBuildingMonitor,
            IPvPCruiserBuildingMonitor enemyBuildingMonitor)
        {
            PvPHelper.AssertIsNotNull(fog, visibilityDecider, friendlyBuildingMonitor, enemyBuildingMonitor);

            _fog = fog;
            _visibilityDecider = visibilityDecider;
            _friendlyBuildingMonitor = friendlyBuildingMonitor;
            _enemyBuildingMonitor = enemyBuildingMonitor;

            _friendlyBuildingMonitor.BuildingCompleted += _friendlyBuildingMonitor_BuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted += _enemyBuildingMonitor_BuildingCompleted;

            _friendlyIStealthGenerators = new List<IPvPStealthGenerator>();
            _enemySpySatellites = new List<IPvPSpySatelliteLauncher>();
        }

        private void _friendlyBuildingMonitor_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            // Look for stealth generators
            AddBuilding(_friendlyIStealthGenerators, e.CompletedBuilding, IStealthGenerator_Destroyed);
        }

        private void IStealthGenerator_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            RemoveBuilding(_friendlyIStealthGenerators, e.DestroyedTarget, IStealthGenerator_Destroyed);
        }

        private void _enemyBuildingMonitor_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            // Look for spy satellite launchers
            AddBuilding(_enemySpySatellites, e.CompletedBuilding, SatelliteLauncher_Destroyed);
        }

        private void SatelliteLauncher_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            RemoveBuilding(_enemySpySatellites, e.DestroyedTarget, SatelliteLauncher_Destroyed);
        }

        private void AddBuilding<T>(IList<T> buildings, IPvPBuildable buildingCompleted, EventHandler<PvPDestroyedEventArgs> destroyedHander)
            where T : class, IPvPBuilding
        {
            T building = buildingCompleted as T;

            if (building != null)
            {
                buildings.Add(building);
                building.Destroyed += destroyedHander;

                UpdateFogState();
            }
        }

        private void RemoveBuilding<T>(IList<T> buildings, IPvPTarget destroyedTarget, EventHandler<PvPDestroyedEventArgs> destroyedHandler)
            where T : class, IPvPBuilding
        {
            T destroyedBuilding = destroyedTarget.Parse<T>();

            destroyedBuilding.Destroyed -= destroyedHandler;

            Assert.IsTrue(buildings.Contains(destroyedBuilding));
            buildings.Remove(destroyedBuilding);

            UpdateFogState();
        }

        private void UpdateFogState()
        {
            _fog.IsVisible = _visibilityDecider.ShouldFogBeVisible(_friendlyIStealthGenerators.Count, _enemySpySatellites.Count);
        }

        public void DisposeManagedState()
        {
            _friendlyBuildingMonitor.BuildingCompleted -= _friendlyBuildingMonitor_BuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted -= _enemyBuildingMonitor_BuildingCompleted;
        }
    }
}
