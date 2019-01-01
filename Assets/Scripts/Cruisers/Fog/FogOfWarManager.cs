using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
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
        private readonly ICruiserController _friendlyCruiser, _enemyCruiser;
        private readonly IList<IStealthGenerator> _friendlyIStealthGenerators;
        private readonly IList<ISpySatelliteLauncher> _enemySpySatellites;

        public FogOfWarManager(
            IGameObject fog, 
            IFogVisibilityDecider visibilityDecider,
            ICruiserController friendlyCruiser, 
            ICruiserController enemyCruiser)
        {
            Helper.AssertIsNotNull(fog, visibilityDecider, friendlyCruiser, enemyCruiser);

            _fog = fog;
            _visibilityDecider = visibilityDecider;
            _friendlyCruiser = friendlyCruiser;
            _enemyCruiser = enemyCruiser;

            _friendlyCruiser.BuildingCompleted += _friendlyCruiser_BuildingCompleted;
            _enemyCruiser.BuildingCompleted += _enemyCruiser_BuildingCompleted;

            _friendlyIStealthGenerators = new List<IStealthGenerator>();
            _enemySpySatellites = new List<ISpySatelliteLauncher>();
        }

        private void _friendlyCruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            // Look for stealth generators
            AddBuilding(_friendlyIStealthGenerators, e.Buildable, IStealthGenerator_Destroyed);
        }

        private void IStealthGenerator_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_friendlyIStealthGenerators, e.DestroyedTarget, IStealthGenerator_Destroyed);
		}

        private void _enemyCruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            // Look for spy satellite launchers
            AddBuilding(_enemySpySatellites, e.Buildable, SatelliteLauncher_Destroyed);
        }

        private void SatelliteLauncher_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_enemySpySatellites, e.DestroyedTarget, SatelliteLauncher_Destroyed);
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

        private void RemoveBuilding<T>(IList<T> buildings, ITarget destroyedTarget, EventHandler<DestroyedEventArgs> destroyedHandler) 
            where T : class, IBuilding
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
            _friendlyCruiser.BuildingCompleted -= _friendlyCruiser_BuildingCompleted;
            _enemyCruiser.BuildingCompleted -= _enemyCruiser_BuildingCompleted;
        }
    }
}
