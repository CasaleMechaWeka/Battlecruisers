using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
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
        private readonly IList<IStealthGenerator> _friendlyIStealthGenerators;
        private readonly IList<IBuilding> _enemySpySatellites;
        private readonly IList<ISpyPlaneController> _enemySpyPlanes;

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

            _friendlyBuildingMonitor.BuildingCompleted += _friendlyBuildingMonitor_BuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted += _enemyBuildingMonitor_BuildingCompleted;
            _enemyUnitMonitor.UnitCompleted += _enemyUnitMonitor_UnitCompleted;

            _friendlyIStealthGenerators = new List<IStealthGenerator>();
            _enemySpySatellites = new List<IBuilding>();
            _enemySpyPlanes = new List<ISpyPlaneController>();
        }

        private void _friendlyBuildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            // Look for stealth generators
            AddBuilding(_friendlyIStealthGenerators, e.CompletedBuilding, IStealthGenerator_Destroyed);
        }

        private void IStealthGenerator_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_friendlyIStealthGenerators, e.DestroyedTarget, IStealthGenerator_Destroyed);
        }

        private void _enemyBuildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            // Look for spy satellite launchers
            AddBuilding(_enemySpySatellites, e.CompletedBuilding, SatelliteLauncher_Destroyed);
        }

        private void _enemyUnitMonitor_UnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            AddUnit(_enemySpyPlanes, e.CompletedUnit, SpyPlane_Destroyed);
        }

        private void SatelliteLauncher_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_enemySpySatellites, e.DestroyedTarget, SatelliteLauncher_Destroyed);
        }

        private void SpyPlane_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveUnit(_enemySpyPlanes, e.DestroyedTarget, SpyPlane_Destroyed);
        }

        private void AddBuilding<T>(IList<T> buildings, IBuildable buildingCompleted, EventHandler<DestroyedEventArgs> destroyedHander)
            where T : class, IBuilding
        {
            T building = buildingCompleted as T;

            if (building != null)
            {
                buildings.Add(building);
                building.Destroyed += destroyedHander;

                UpdateFogState();
            }
        }

        private void AddUnit<T>(IList<T> units, IBuildable unitCompleted, EventHandler<DestroyedEventArgs> destroyedHander)
            where T : class, IUnit
        {
            T unit = unitCompleted as T;

            if (unit != null)
            {
                units.Add(unit);
                unit.Destroyed += destroyedHander;

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
            _friendlyBuildingMonitor.BuildingCompleted -= _friendlyBuildingMonitor_BuildingCompleted;
            _enemyBuildingMonitor.BuildingCompleted -= _enemyBuildingMonitor_BuildingCompleted;
            _enemyUnitMonitor.UnitCompleted -= _enemyUnitMonitor_UnitCompleted;
        }
    }
}
