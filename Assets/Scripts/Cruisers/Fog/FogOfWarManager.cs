using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Fog
{
    public class FogOfWarManager : IFogOfWarManager
	{
        private readonly IFogOfWar _fog;
        private readonly ICruiserController _friendlyCruiser, _enemyCruiser;
        private readonly IList<StealthGenerator> _friendlyStealthGenerators;
        private readonly IList<SpySatelliteLauncher> _enemySpySatellites;

        private bool _isFogEnabled;
        public bool IsFogEnabled 
        { 
            get { return _isFogEnabled; } 
            private set
            {
                if (_isFogEnabled != value)
                {
                    _isFogEnabled = value;
                    _fog.SetEnabled(_isFogEnabled);

                    if (IsFogEnabledChanged != null)
                    {
                        IsFogEnabledChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

		public event EventHandler IsFogEnabledChanged;

        public FogOfWarManager(IFogOfWar fog, ICruiserController friendlyCruiser, ICruiserController enemyCruiser)
        {
            Helper.AssertIsNotNull(fog, friendlyCruiser, enemyCruiser);

            _fog = fog;
            _friendlyCruiser = friendlyCruiser;
            _enemyCruiser = enemyCruiser;

            _friendlyCruiser.BuildingCompleted += _friendlyCruiser_BuildingCompleted;
            _enemyCruiser.BuildingCompleted += _enemyCruiser_BuildingCompleted;

            _friendlyStealthGenerators = new List<StealthGenerator>();
            _enemySpySatellites = new List<SpySatelliteLauncher>();
        }

        private void _friendlyCruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
        {
            // Look for stealth generators
            AddBuilding(_friendlyStealthGenerators, e.Buildable, StealthGenerator_Destroyed);
        }

        private void StealthGenerator_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveBuilding(_friendlyStealthGenerators, e.DestroyedTarget, StealthGenerator_Destroyed);
		}

        private void _enemyCruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
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
            IsFogEnabled = _friendlyStealthGenerators.Count != 0 && _enemySpySatellites.Count == 0;
        }

        public void Dispose()
        {
            _friendlyCruiser.BuildingCompleted -= _friendlyCruiser_BuildingCompleted;
            _enemyCruiser.BuildingCompleted -= _enemyCruiser_BuildingCompleted;
        }
    }
}
