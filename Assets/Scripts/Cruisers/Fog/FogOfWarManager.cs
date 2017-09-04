using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
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

        // FELIX  Generic methods :D
        private void _friendlyCruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
        {
            // Look for stealth generators
            StealthGenerator stealthGenerator = e.Buildable as StealthGenerator;

            if (stealthGenerator != null)
            {
                _friendlyStealthGenerators.Add(stealthGenerator);
                stealthGenerator.Destroyed += StealthGenerator_Destroyed;
				
                UpdateFogState();
			}
        }

        private void StealthGenerator_Destroyed(object sender, DestroyedEventArgs e)
        {
            StealthGenerator destroyedGenerator = e.DestroyedTarget.Parse<StealthGenerator>();

            destroyedGenerator.Destroyed -= StealthGenerator_Destroyed;

            Assert.IsTrue(_friendlyStealthGenerators.Contains(destroyedGenerator));
            _friendlyStealthGenerators.Remove(destroyedGenerator);
		
            UpdateFogState();
		}

        private void _enemyCruiser_BuildingCompleted(object sender, CompletedConstructionEventArgs e)
        {
            // Look for spy satellite launchers
            SpySatelliteLauncher satelliteLauncher = e.Buildable as SpySatelliteLauncher;

            if (satelliteLauncher != null)
            {
                _enemySpySatellites.Add(satelliteLauncher);
                satelliteLauncher.Destroyed += SatelliteLauncher_Destroyed;
				
                UpdateFogState();
			}
        }

        private void SatelliteLauncher_Destroyed(object sender, DestroyedEventArgs e)
        {
            SpySatelliteLauncher destroyedLauncher = e.DestroyedTarget.Parse<SpySatelliteLauncher>();

            destroyedLauncher.Destroyed -= SatelliteLauncher_Destroyed;

            Assert.IsTrue(_enemySpySatellites.Contains(destroyedLauncher));
            _enemySpySatellites.Remove(destroyedLauncher);

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
