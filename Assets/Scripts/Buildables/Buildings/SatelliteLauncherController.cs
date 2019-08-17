using BattleCruisers.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
	public abstract class SatelliteLauncherController : Building
	{
        private IUnit _satellite;

        public UnitWrapper satellitePrefab;

        protected abstract Vector3 SpawnPositionAdjustment { get; }

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

			Assert.IsNotNull(satellitePrefab);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_satellite = _factoryProvider.PrefabFactory.CreateUnit(satellitePrefab);
			_satellite.Position = transform.position + SpawnPositionAdjustment;
			_satellite.Initialise(ParentCruiser, _enemyCruiser, _uiManager, _factoryProvider, _cruiserSpecificFactories);
			_satellite.StartConstruction();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

            if (BuildableState == BuildableState.Completed)
            {
                _satellite.Destroy();
			}
		}
	}
}
