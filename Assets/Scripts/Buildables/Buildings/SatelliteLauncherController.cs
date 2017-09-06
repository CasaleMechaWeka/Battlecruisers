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

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(satellitePrefab);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_satellite = _factoryProvider.PrefabFactory.CreateUnit(satellitePrefab);
			_satellite.Position = transform.position + SpawnPositionAdjustment;
			_satellite.Initialise(_parentCruiser, _enemyCruiser, _uiManager, _factoryProvider);
			_satellite.StartConstruction();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			_satellite.Destroy();
		}
	}
}
