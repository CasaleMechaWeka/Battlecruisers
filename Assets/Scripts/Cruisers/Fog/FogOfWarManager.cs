using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Fog
{
    /// <summary>
    /// Determines when to re-evalute whether fog of war should be enabled or not.
    /// </summary>
    public class FogOfWarManager : IManagedDisposable
    {
        private readonly IGameObject _fog;
        private readonly IFogVisibilityDecider _visibilityDecider;
        private readonly ICruiserBuildingMonitor _friendlyBuildingMonitor, _enemyBuildingMonitor;
        private readonly ICruiserUnitMonitor _enemyUnitMonitor;
        private readonly IList<StealthGenerator> _friendlyIStealthGenerators;
        private readonly IList<SpySatelliteLauncherController> _enemySpySatellites;
        private readonly IList<SpyPlaneController> _enemySpyPlanes;

        public FogOfWarManager(
            IGameObject fog,
            IFogVisibilityDecider visibilityDecider,
            ICruiserBuildingMonitor friendlyBuildingMonitor,
            ICruiserBuildingMonitor enemyBuildingMonitor,
            ICruiserUnitMonitor enemyUnitMonitor)
        {
            Helper.AssertIsNotNull(fog, visibilityDecider, friendlyBuildingMonitor, enemyBuildingMonitor, enemyUnitMonitor);

            _fog = fog;
            _visibilityDecider = visibilityDecider;
            _friendlyBuildingMonitor = friendlyBuildingMonitor;
            _enemyBuildingMonitor = enemyBuildingMonitor;
            _enemyUnitMonitor = enemyUnitMonitor;

            _friendlyBuildingMonitor.BuildingCompleted += FriendlyBuildingMonitorBuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted += EnemyBuildingMonitorBuildingCompleted;
            _enemyUnitMonitor.UnitCompleted += EnemyUnitMonitorUnitCompleted;

            _friendlyIStealthGenerators = new List<StealthGenerator>();
            _enemySpySatellites = new List<SpySatelliteLauncherController>();
            _enemySpyPlanes = new List<SpyPlaneController>();
        }

        private void FriendlyBuildingMonitorBuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            // Look for stealth generators
            AddStealthGen(_friendlyIStealthGenerators, e.CompletedBuilding, StealthGeneratorDestroyed);

        }

        private void StealthGeneratorDestroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_friendlyIStealthGenerators, e.DestroyedTarget, StealthGeneratorDestroyed);
        }

        private void EnemyBuildingMonitorBuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            // Look for spy satellite launchers
            AddSpySat(_enemySpySatellites, e.CompletedBuilding, SatelliteLauncherDestroyed);
        }

        private void EnemyUnitMonitorUnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            AddSpyPlane(_enemySpyPlanes, e.CompletedUnit, SpyPlaneDestroyed);
        }

        private void SatelliteLauncherDestroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_enemySpySatellites, e.DestroyedTarget, SatelliteLauncherDestroyed);
        }

        private void SpyPlaneDestroyed(object sender, DestroyedEventArgs e)
        {
            RemoveUnit(_enemySpyPlanes, e.DestroyedTarget, SpyPlaneDestroyed);
        }

        private void AddStealthGen(
            IList<StealthGenerator> stealthGens,
            IBuilding stealthGenCompleted,
            EventHandler<DestroyedEventArgs> destroyedHander)
        {
            if (stealthGenCompleted is StealthGenerator stealthGen)
            {
                stealthGens.Add(stealthGen);
                stealthGen.Destroyed += destroyedHander;

                UpdateFogState();
            }
        }

        private void AddSpySat(
            IList<SpySatelliteLauncherController> spySats,
            IBuilding spySatCompleted,
            EventHandler<DestroyedEventArgs> destroyedHander)
        {
            if (spySatCompleted is SpySatelliteLauncherController spySat)
            {
                spySats.Add(spySat);
                spySat.Destroyed += destroyedHander;

                UpdateFogState();
            }
        }

        private void AddSpyPlane(
            IList<SpyPlaneController> spyPlanes,
            IBuildable unitCompleted,
            EventHandler<DestroyedEventArgs> destroyedHander)
        {
            if (unitCompleted is SpyPlaneController spyPlane)
            {
                spyPlanes.Add(spyPlane);
                spyPlane.Destroyed += destroyedHander;

                UpdateFogState();
            }
        }

        private void RemoveBuilding<T>(IList<T> buildings, ITarget destroyedTarget, EventHandler<DestroyedEventArgs> destroyedHandler)
            where T : class, IBuilding
        {
            T destroyedBuilding = destroyedTarget.Parse<T>();

            destroyedBuilding.Destroyed -= destroyedHandler;

            Assert.IsTrue(buildings.Contains(destroyedBuilding));
            buildings.Remove(destroyedBuilding);

            UpdateFogState();
        }

        private void RemoveUnit<T>(IList<T> units, ITarget destroyedTarget, EventHandler<DestroyedEventArgs> destroyedHandler)
            where T : class, IUnit
        {
            T destroyedUnit = destroyedTarget.Parse<T>();

            destroyedUnit.Destroyed -= destroyedHandler;

            Assert.IsTrue(units.Contains(destroyedUnit));
            units.Remove(destroyedUnit);

            UpdateFogState();
        }

        private void UpdateFogState()
        {
            _fog.IsVisible = _visibilityDecider.ShouldFogBeVisible(_friendlyIStealthGenerators.Count, _enemySpySatellites.Count, _enemySpyPlanes.Count);
        }

        public void DisposeManagedState()
        {
            _friendlyBuildingMonitor.BuildingCompleted -= FriendlyBuildingMonitorBuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted -= EnemyBuildingMonitorBuildingCompleted;
            _enemyUnitMonitor.UnitCompleted -= EnemyUnitMonitorUnitCompleted;
        }
    }
}
